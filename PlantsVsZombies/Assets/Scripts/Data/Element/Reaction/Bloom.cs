using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����
/// </summary>
public class Bloom : ElementsReaction
{
    /// <summary>
    /// ������ʧʱ��ը�������˺�
    /// </summary>
    public const int SeedExplodeDamage = 20;
    /// <summary>
    /// ���ӱ�ը�˺��ķ�Χ
    /// </summary>
    public const float SeedExplodeRadius = 1f;
    /// <summary>
    /// ���ӱ�ըǰ���ӳ�
    /// </summary>
    public const int MilisecondsBeforeSeedExplode = 5000;
    /// <summary>
    /// �����Ŵ����������˺�
    /// </summary>
    public const int HyperBloomDamage = 5;
    /// <summary>
    /// �����Ŵ�����׷����������
    /// </summary>
    public const int HyperBloomCount = 10;
    /// <summary>
    /// �����Ŵ���ʱ���˺�
    /// </summary>
    public const int PyroBloomDamage = 30;
    /// <summary>
    /// �����Ŵ���ʱ�İ뾶
    /// </summary>
    public const float PyroBloomRadius = 1.2f;
    /// <summary>
    /// ����Ԥ�����·��
    /// </summary>
    private const string SEED_PATH = "ElementReaction/GrassCore";
    /// <summary>
    /// �����ű�ը��·��
    /// </summary>
    private const string HYPEREXPLOSION_PATH = "ElementReaction/HyperExplosion";
    private static GameObject seed;
    private static GameObject hyperExplosion;
    /// <summary>
    /// �������ɵ�����Ԥ����
    /// </summary>
    public static GameObject Seed
    {
        get
        {
            if (seed == null)
            {
                seed = ResourceManager.Instance.Load<GameObject>(SEED_PATH);//ȥ������������
            }
            return seed;
        }
    }
    /// <summary>
    /// ���������ɵ�׷�ٵ���Ԥ����
    /// </summary>
    public static GameObject HyperExplosion
    {
        get
        {
            if (hyperExplosion == null)
            {
                hyperExplosion = ResourceManager.Instance.Load<GameObject>(HYPEREXPLOSION_PATH);//ȥ������������
            }
            return hyperExplosion;
        }
    }

    private static ObjectBuffer seedPool = new ObjectBuffer(FlyersController.FlyersFatherObject.transform);

    /// <summary>
    /// ����һ�����Ӷ���
    /// </summary>
    /// <param name="worldPos"></param>
    public static void AddSeed(Vector3 worldPos)
    {
        GameObject newSeed = seedPool.Get(Seed);
        newSeed.transform.position = worldPos;
    }
    public static void RemoveSeed(GrassCore core)
    {
        seedPool.Put(Seed,core.gameObject);
    }

    /// <summary>
    /// ��ָ�����������д���һ�γ�����
    /// </summary>
    /// <param name="worldPos">��������</param>
    public static void TriggerHyperBloom(Vector3 worldPos)
    {
        GameObject obj = GameObject.Instantiate(HyperExplosion);
        obj.transform.position = worldPos;

        ShowReaction("HyperBloom", worldPos);

        obj.GetComponent<HyperExplosion>().Explode(HyperBloomCount);
    }

    /// <summary>
    /// ��ָ�����괦����һ��������
    /// </summary>
    /// <param name="worldPos">��������</param>
    public static void TriggerPyroBloom(Vector3 worldPos)
    {
        ShowReaction("PyroBloom", worldPos);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPos, PyroBloomRadius);
        foreach (var collider in colliders)
        {
            IDamageable target = collider.GetComponent<IDamageable>();
            if (target != null)
                target.GetReceiver().ReceiveDamage(new SystemDamage(PyroBloomDamage, Elements.Grass));
        }
    }
    
    
    public override string ReactionName => "Bloom";

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        //�ڹ���Ľ�������һ��������
        AddSeed(target.GameObject.transform.position);
    }
}
