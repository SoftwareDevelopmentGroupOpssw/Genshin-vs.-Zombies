using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存放着反应信息图片的Sprite
/// </summary>
[CreateAssetMenu(fileName = "ReactionInfoSpriteDataCollection", menuName = "SODatabase/ReactionInfoSpriteDatabaseSO")]
public class ReactionInfoSpriteDatabase : ScriptableObject
{
    [SerializeField]
    private List<ReactionInfo> infos = new List<ReactionInfo>();
    /// <summary>
    /// 用给定的反应名字找寻反应信息图片
    /// </summary>
    /// <param name="reactionName"></param>
    /// <returns></returns>
    public Sprite GetSprite(string reactionName) => infos.Find((info) => info.Name == reactionName).Sprite;
    [System.Serializable]
    struct ReactionInfo
    {
        public Sprite Sprite;
        public string Name;
    }
}
