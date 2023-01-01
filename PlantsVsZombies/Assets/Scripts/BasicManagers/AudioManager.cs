using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

/// <summary>
/// 用于管理音频
/// </summary>
public class AudioManager:Singleton<AudioManager>
{
    private const string AUDIODATA_PATH = "SO/";
    private const string PLAYERPREFS_NAME_OF_MUISC_VOLUME = "MusicVolume";
    private const string PLAYERPREFS_NAME_OF_EFFECTVOLUME = "EffectVolume";
    private AudioDatabase backgroundMusics;
    private AudioDatabase effectAudios;

    private AudioSource musicSource;

    private const string EFFECTSOURCE_PATH = "Prefabs/Audios/EffectSource";
    /// <summary>
    /// 原始预制体
    /// </summary>
    private GameObject effectSource;

    private ObjectBuffer effectSourceBuffer;

    private List<AudioSource> effectSourceList = new List<AudioSource>();
    public AudioManager()
    {
        GameObject obj = new GameObject("AudioSource");
        
        GameObject musicSource = new GameObject("MusicSource");
        musicSource.transform.SetParent(obj.transform);
        this.musicSource = musicSource.AddComponent<AudioSource>();
        this.musicSource.loop = true;

        GameObject effectSources = new GameObject("EffectSources");
        effectSources.transform.SetParent(obj.transform);
        
        effectSource = ResourceManager.Instance.Load<GameObject>(EFFECTSOURCE_PATH);
        
        effectSourceBuffer = new ObjectBuffer(effectSources.transform);

        if(PlayerPrefs.HasKey(PLAYERPREFS_NAME_OF_MUISC_VOLUME))
            musicVolume = PlayerPrefs.GetFloat(PLAYERPREFS_NAME_OF_MUISC_VOLUME);
       if(PlayerPrefs.HasKey(PLAYERPREFS_NAME_OF_EFFECTVOLUME))
            effectVolume = PlayerPrefs.GetFloat(PLAYERPREFS_NAME_OF_EFFECTVOLUME);
    }
    
    private AudioDatabase BackgroundMusicDatabase
    {
        get
        {
            if (backgroundMusics == null)
            {
                backgroundMusics = ResourceManager.Instance.Load<AudioDatabase>(AUDIODATA_PATH + "BackgroundMusicDatabase");
            }
            return backgroundMusics;
        }
    }
    private float musicVolume = 1;
    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            if (value > 1)
                musicVolume = 1;
            else if (value < 0)
                musicVolume = 0;
            else
                musicVolume = value;
            musicSource.volume = musicVolume;
            PlayerPrefs.SetFloat(PLAYERPREFS_NAME_OF_MUISC_VOLUME, musicVolume);//写入注册表
        }
    }

    private float effectVolume = 1;
    public float EffectVolume
    {
        get => effectVolume;
        set
        {
            if (value > 1)
                effectVolume = 1;
            else if (value < 0)
                effectVolume = 0;
            else
                effectVolume = value;
            foreach (var item in effectSourceList)
            {
                item.volume = effectVolume;
            }

            PlayerPrefs.SetFloat(PLAYERPREFS_NAME_OF_MUISC_VOLUME, effectVolume);//写入注册表
        }
    }


    #region BackgroundMusic
    public AudioClip PlayRandomBackgroundMusic()
    {
        AudioClip audio = BackgroundMusicDatabase.GetRandomAudio();
        musicSource.clip = audio;
        musicSource.volume = musicVolume;
        return audio;
    }
    public AudioClip PlayBackgroundMusic(string musicName)
    {
        AudioClip audio = BackgroundMusicDatabase.GetAudio(musicName);
        musicSource.clip = audio;
        musicSource.volume = musicVolume;
        return audio;
    }

    public void PauseBackgroundAudio()
    {
        musicSource.Pause();
    }

    public void StopBackgroundAuido()
    {
        musicSource.Stop();
    }

    #endregion

    #region EffectMusic

    public AudioSource PlayEffectAudio(string name)
    {
        IEnumerator CollectCouroutine(AudioSource source, float time)
        {
            yield return new WaitForSecondsRealtime(time);
            effectSourceBuffer.Put(effectSource, source.gameObject);
        }
        AudioSource source = effectSourceBuffer.Get(effectSource).GetComponent<AudioSource>();
        source.clip =  effectAudios.GetAudio(name);
        effectSourceList.Add(source);
        MonoManager.Instance.StartCoroutine(CollectCouroutine(source, source.clip.length));
        return source;
    }

    public void StopEffectAudio(AudioSource source)
    {
        if (effectSourceList.Contains(source))
        {
            source.Stop();
            effectSourceList.Remove(source);
            effectSourceBuffer.Put(effectSource,source.gameObject);
        }
    }

    #endregion
}

