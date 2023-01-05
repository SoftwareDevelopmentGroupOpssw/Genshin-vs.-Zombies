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
    public const int SeedExplodeDamage = 40;
    /// <summary>
    /// ���ӱ�ը�˺��ķ�Χ
    /// </summary>
    public const float SeedExplodeRadius = 1f;
    /// <summary>
    /// ���ӱ�ըǰ���ӳ�
    /// </summary>
    public const int MilisecondsBeforeSeedExplode = 6000;
    /// <summary>
    /// �����Ŵ����������˺�
    /// </summary>
    public const int HyperBloomDamage = 7;
    /// <summary>
    /// �����Ŵ�����׷����������
    /// </summary>
    public const int HyperBloomCount = 12;
    /// <summary>
    /// �����Ŵ���ʱ���˺�
    /// </summary>
    public const int PyroBloomDamage = 85;
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

    private static readonly string bufferName = "Bloom";
    public static ObjectBuffer Pool
    {
        get
        {
            EntitiesController controller = GameController.Instance.EntitiesController;
            if (!controller.ContainsBuffer(bufferName))
            {
                controller.AddBufferToManagement(bufferName, new ObjectBuffer(FlyersController.FlyersFatherObject.transform));
                controller.AddBehaviourManagement<GrassCore>(bufferName, Seed); //�����Ӻ�GrassCore�ű��������
            }
            return controller[bufferName];
        }
    }
    /// <summary>
    /// ����һ�����Ӷ���
    /// </summary>
    /// <param name="worldPos"></param>
    public static GameObject AddSeed(Vector3 worldPos)
    {
        GameObject newSeed = Pool.Get(Seed);
        newSeed.transform.position = worldPos;
        return newSeed;
    }
    public static void RemoveSeed(GrassCore core)
    {
        Pool.Put(Seed,core.gameObject);
    }

    /// <summary>
    /// ��ָ�����������д���һ�γ�����
    /// </summary>
    /// <param name="worldPos">��������</param>
    public static void TriggerHyperBloom(Vector3 worldPos)
    {
        GameObject obj = GameObject.Instantiate(HyperExplosion);
        obj.transform.position = worldPos;
        obj.GetComponent<HyperExplosion>().Explode(HyperBloomCount);
    }

    /// <summary>
    /// ��ָ�����괦����һ��������
    /// </summary>
    /// <param name="worldPos">��������</param>
    public static void TriggerPyroBloom(Vector3 worldPos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPos, PyroBloomRadius);
        foreach (var collider in colliders)
        {
            IDamageable target = collider.GetComponent<IDamageable>();
            if (target != null && target is Monster)
                target.GetReceiver().ReceiveDamage(new SystemDamage(PyroBloomDamage, Elements.Grass));
        }
    }
    
    
    public override string ReactionName => "Bloom";

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        IEnumerator SeedFlyingCoroutine(GameObject seed)
        {
            //���ӱĳ����ľ���
            float offset = 0.7f;
            float farestLocation = 0.35f;
            float nearestLocation = 0.2f;
            int sign = Random.value - offset > 0 ? 1 : -1;
            float now = Random.Range(sign * nearestLocation,sign * farestLocation);

            Vector3 startPos = seed.transform.position;
            float flySpeed = 1;
            float polonomialArg = 15;

            for(float x = 0; Mathf.Abs(x) < Mathf.Abs(now); x += sign * flySpeed * Time.deltaTime)
            {
                float y = -polonomialArg *  x * (x - now);
                seed.transform.position = startPos + new Vector3(x, y, 0);
                yield return 1;
            }
        }
        //�ڹ���Ľ�������һ��������,�����ӵľ���λ�����Ŷ�
        GameObject seed =  AddSeed(target.GameObject.transform.position);
        seed.GetComponent<GrassCore>().StartCoroutine(SeedFlyingCoroutine(seed));

        AudioManager.Instance.PlayRandomEffectAudio("throw1", "throw2");
    }
} 
