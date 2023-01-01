using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioCollection", menuName = "SODatabase/AudioSO")]
public class AudioDatabase : ScriptableObject,IEnumerable<AudioDatabase.Audio>
{
    [SerializeField]
    private List<Audio> audioSources = new List<Audio>();

    public AudioClip GetAudio(string audioName)
    {
        return audioSources.Find((audio) => audio.Name == audioName).AudioSource;
    }
    public AudioClip GetRandomAudio()
    {
        System.Random r = new System.Random();
        return audioSources[r.Next(audioSources.Count)].AudioSource;
    }
    public IEnumerator<Audio> GetEnumerator()
    {
        return audioSources.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [System.Serializable]
    public class Audio
    {
        public AudioClip AudioSource;
        public string Name;
    }
}
