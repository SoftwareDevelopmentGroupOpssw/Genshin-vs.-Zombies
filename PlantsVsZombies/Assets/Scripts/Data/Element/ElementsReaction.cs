using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ԫ�ط�Ӧ����
/// </summary>
public abstract class ElementsReaction
{
    protected readonly static GameObject InformationFatherObject = new GameObject("ReactionInformation");
    protected static ObjectBuffer informationBuffer;
    protected static GameObject reactionText;
    protected static ReactionInfoSpriteDatabase database;
    public static void ShowReaction(string reactionName,Vector3 worldPos)
    {
        if (informationBuffer == null)
            informationBuffer = new ObjectBuffer(InformationFatherObject.transform);
        if (reactionText == null)
            reactionText = ResourceManager.Instance.Load<GameObject>("Prefabs/UI/UIElements/ReactInformation");
        if (database == null)
            database = ResourceManager.Instance.Load<ReactionInfoSpriteDatabase>("SO/ReactionInfoSpriteDatabase");
        GameObject text = informationBuffer.Get(reactionText);
        text.GetComponent<ReactInformation>().Text = database.GetSprite(reactionName);
        text.transform.position = worldPos;
    }
    
    /// <summary>
    /// ϵͳ����
    /// </summary>
    protected static IGameobjectData system = SystemObject.Instance;
    
    /// <summary>
    /// ��ȡһ��Ԫ�ط�Ӧ
    /// </summary>
    /// <param name="before">�Ѿ����ŵ�Ԫ��</param>
    /// <param name="after">�������ŵ�Ԫ��</param>
    /// <returns>Ԫ�ط�Ӧ�������Ԫ�ط�Ӧ��������Ϊ��</returns>
    public static ElementsReaction GetReaction(Elements before, Elements after)
    {
        //TODO:����ÿ�ֿ��е�Ԫ�ط�Ӧ������һ����Ӧ��Ԫ�ط�Ӧ��ȥ
        //���Ԫ�ط�Ӧ��Ȼ����Ҫ�̳�ElementsReaction�ಢ��дAction����
        switch (before)
        {
            case Elements.Water:
                switch (after)
                {
                    case Elements.Fire:
                        return new Vaporize();
                    case Elements.Ice:
                        return new Frozen();
                    case Elements.Electric:
                        return new ElectroCharged();
                    case Elements.Wind:
                        return new Swirl();
                    case Elements.Stone:
                        return new Crystallize();
                    case Elements.Grass:
                        return new Bloom();
                }
                break;
            case Elements.Fire:
                switch (after)
                {
                    case Elements.Water:
                        return new Vaporize();
                    case Elements.Ice:
                        return new Melt();
                    case Elements.Electric:
                        return new Overloaded();
                    case Elements.Wind:
                        return new Swirl();
                    case Elements.Stone:
                        return new Crystallize();
                    case Elements.Grass:
                        return new Burning();
                }
                break;
            case Elements.Ice:
                switch (after)
                {
                    case Elements.Water:
                        return new Frozen();
                    case Elements.Fire:
                        return new Melt();
                    case Elements.Electric:
                        return new SuperConduct();
                    case Elements.Wind:
                        return new Swirl();
                    case Elements.Stone:
                        return new Crystallize();
                }
                break;
            case Elements.Electric:
                switch (after)
                {
                    case Elements.Water:
                        return new ElectroCharged();
                    case Elements.Fire:
                        return new Overloaded();
                    case Elements.Ice:
                        return new SuperConduct();
                    case Elements.Wind:
                        return new Swirl();
                    case Elements.Stone:
                        return new Crystallize();
                    case Elements.Grass:
                        return new Quicken();
                }
                break;
            //��Ԫ�غ���Ԫ�ز��ܱ����ţ�ǰԪ�ز�����������
            case Elements.Wind:
            case Elements.Stone:
                break;
            case Elements.Grass:
                switch (after)
                {
                    case Elements.Water:
                        return new Bloom();
                    case Elements.Fire:
                        return new Burning();
                    case Elements.Electric:
                        return new Quicken();
                }
                break;
        }
        return null;
    }
    /// <summary>
    /// Ԫ�ط�Ӧ����
    /// </summary>
    public abstract string ReactionName { get; }
    /// <summary>
    /// Ԫ�ط�Ӧ�ͷ�
    /// </summary>
    /// <param name="damage">Ԫ���˺���Դ</param>
    /// <param name="target">Ԫ�ط�ӦĿ��</param>
    protected abstract void RealAction(IElementalDamage damage, IDamageReceiver target);
    public void Action(IElementalDamage damage,IDamageReceiver target)
    {
        ShowReaction(ReactionName, target.GameObject.transform.position);
        RealAction(damage,target);
    }
}
