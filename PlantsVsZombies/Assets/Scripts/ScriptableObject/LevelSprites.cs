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
    /// 查找特定名字的关卡sprite
    /// </summary>
    /// <param name="levelName">关卡名</param>
    /// <returns>关卡的sprite</returns>
    public Sprite SearchSprite(string levelName)
    {
        return Levels.Find((level) => level.Name == levelName).Sprite;
    }
    /// <summary>
    /// 数量
    /// </summary>
    public int Count => Levels.Count;
    /// <summary>
    /// 遍历
    /// </summary>
    /// <param name="action">遍历函数</param>
    public void Foreach(UnityAction<Level> action)
    {
        foreach (Level l in Levels)
            action.Invoke(l);
    }
}
