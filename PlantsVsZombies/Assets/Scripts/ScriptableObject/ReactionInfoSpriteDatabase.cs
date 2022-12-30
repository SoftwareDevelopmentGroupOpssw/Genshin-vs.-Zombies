using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ŷ�Ӧ��ϢͼƬ��Sprite
/// </summary>
[CreateAssetMenu(fileName = "ReactionInfoSpriteDataCollection", menuName = "SODatabase/ReactionInfoSpriteDatabaseSO")]
public class ReactionInfoSpriteDatabase : ScriptableObject
{
    [SerializeField]
    private List<ReactionInfo> infos = new List<ReactionInfo>();
    /// <summary>
    /// �ø����ķ�Ӧ������Ѱ��Ӧ��ϢͼƬ
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
