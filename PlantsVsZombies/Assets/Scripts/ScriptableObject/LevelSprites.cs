using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "LevelSpritesCollection", menuName = "LevelSpritesSO")]
public class LevelSprites : ScriptableObject
{
    [SerializeField]
    private List<Level> Levels = new List<Level>();
    [System.Serializable]
    public class Level
    {
        public Sprite Sprite;
        public string Name;
    }
    /// <summary>
    /// �����ض����ֵĹؿ�sprite
    /// </summary>
    /// <param name="levelName">�ؿ���</param>
    /// <returns>�ؿ���sprite</returns>
    public Sprite SearchSprite(string levelName)
    {
        return Levels.Find((level) => level.Name == levelName).Sprite;
    }
    /// <summary>
    /// ����
    /// </summary>
    public int Count => Levels.Count;
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="action">��������</param>
    public void Foreach(UnityAction<Level> action)
    {
        foreach (Level l in Levels)
            action.Invoke(l);
    }
}
