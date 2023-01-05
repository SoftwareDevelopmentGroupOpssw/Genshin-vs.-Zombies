using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 绽放
/// </summary>
public class Bloom : ElementsReaction
{
    /// <summary>
    /// 种子消失时爆炸产生的伤害
    /// </summary>
    public const int SeedExplodeDamage = 40;
    /// <summary>
    /// 种子爆炸伤害的范围
    /// </summary>
    public const float SeedExplodeRadius = 1f;
    /// <summary>
    /// 种子爆炸前的延迟
    /// </summary>
    public const int MilisecondsBeforeSeedExplode = 6000;
    /// <summary>
    /// 超绽放触发的种子伤害
    /// </summary>
    public const int HyperBloomDamage = 7;
    /// <summary>
    /// 超绽放触发的追踪种子数量
    /// </summary>
    public const int HyperBloomCount = 12;
    /// <summary>
    /// 烈绽放触发时的伤害
    /// </summary>
    public const int PyroBloomDamage = 85;
    /// <summary>
    /// 烈绽放触发时的半径
    /// </summary>
    public const float PyroBloomRadius = 1.2f;
    /// <summary>
    /// 种子预制体的路径
    /// </summary>
    private const string SEED_PATH = "ElementReaction/GrassCore";
    /// <summary>
    /// 超绽放爆炸的路径
    /// </summary>
    private const string HYPEREXPLOSION_PATH = "ElementReaction/HyperExplosion";
    private static GameObject seed;
    private static GameObject hyperExplosion;

    /// <summary>
    /// 绽放生成的种子预制体
    /// </summary>
    public static GameObject Seed
    {
        get
        {
            if (seed == null)
            {
                seed = ResourceManager.Instance.Load<GameObject>(SEED_PATH);//去加载种子物体
            }
            return seed;
        }
    }
    /// <summary>
    /// 超绽放生成的追踪弹的预制体
    /// </summary>
    public static GameObject HyperExplosion
    {
        get
        {
            if (hyperExplosion == null)
            {
                hyperExplosion = ResourceManager.Instance.Load<GameObject>(HYPEREXPLOSION_PATH);//去加载种子物体
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
                controller.AddBehaviourManagement<GrassCore>(bufferName, Seed); //将种子和GrassCore脚本纳入管理
            }
            return controller[bufferName];
        }
    }
    /// <summary>
    /// 产生一个种子对象
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
    /// 在指定世界坐标中触发一次超绽放
    /// </summary>
    /// <param name="worldPos">世界坐标</param>
    public static void TriggerHyperBloom(Vector3 worldPos)
    {
        GameObject obj = GameObject.Instantiate(HyperExplosion);
        obj.transform.position = worldPos;
        obj.GetComponent<HyperExplosion>().Explode(HyperBloomCount);
    }

    /// <summary>
    /// 在指定坐标处触发一次烈绽放
    /// </summary>
    /// <param name="worldPos">世界坐标</param>
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
            //种子蹦出来的距离
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
        //在怪物的脚下生成一个草种子,草种子的具体位置有扰动
        GameObject seed =  AddSeed(target.GameObject.transform.position);
        seed.GetComponent<GrassCore>().StartCoroutine(SeedFlyingCoroutine(seed));

        AudioManager.Instance.PlayRandomEffectAudio("throw1", "throw2");
    }
} 
