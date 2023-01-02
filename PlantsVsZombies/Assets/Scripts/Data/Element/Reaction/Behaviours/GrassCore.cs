using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class GrassCore : MonoBehaviour, IDamageable
{
    private Animator animator;
    private CountDown countdown = new CountDown(Bloom.MilisecondsBeforeSeedExplode);
    private bool isTriggered;//是否已经触发
    /// <summary>
    /// 一个草原核的伤害接收器
    /// </summary>
    private GrassCoreReceiver receiver = new GrassCoreReceiver();
    public IDamageReceiver GetReceiver() => receiver;

    /// <summary>
    /// 动画系统触发：原绽放
    /// </summary>
    void Animator_Bloom()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Bloom.SeedExplodeRadius);
        foreach (var collider in colliders)
        {
            IDamageable target = collider.GetComponent<IDamageable>();
            if (target != null && target is Monster)
                target.GetReceiver().ReceiveDamage(new SystemDamage(Bloom.SeedExplodeDamage, Elements.Grass));
        }
    }
    /// <summary>
    /// 动画系统触发：触发超绽放
    /// </summary>
    void Animator_Hyper()
    {
        Bloom.TriggerHyperBloom(transform.position);
    }
    /// <summary>
    /// 动画系统触发：触发烈绽放
    /// </summary>
    void Animator_Pyro()
    {
        Bloom.TriggerPyroBloom(transform.position);
    }
    /// <summary>
    /// 监听雷元素
    /// </summary>
    /// <param name="damage"></param>
    void ElecticListen(IElementalDamage damage)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            animator.SetTrigger("Hyper");
            ElementsReaction.ShowReaction("HyperBloom", transform.position);
        }
    }
    /// <summary>
    /// 监听火元素
    /// </summary>
    /// <param name="damage"></param>
    void FireListen(IElementalDamage damage)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            animator.SetTrigger("Pyro");
            ElementsReaction.ShowReaction("PyroBloom", transform.position);
        }
    }
    /// <summary>
    /// 等到时间到时调用
    /// </summary>
    void Bloomed()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            animator.SetTrigger("Bloom");
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Update()
    {
        if (countdown.Available)
            Bloomed();
    }
    public void OnEnable()
    {
        isTriggered = false;
        animator.Play("Empty");
        countdown.StartCountDown();
        receiver.AddOnReceiveDamageListener(Elements.Electric, ElecticListen);
        receiver.AddOnReceiveDamageListener(Elements.Fire, FireListen);
    }
    private void DestroyMe()
    {
        receiver.RemoveOnReceiveDamageListener(Elements.Electric, ElecticListen);
        receiver.RemoveOnReceiveDamageListener(Elements.Fire, FireListen);
        Bloom.RemoveSeed(this);
    }


    /// <summary>
    /// 草原核伤害接收器
    /// 草原核不能添加效果，也不能受到元素附着
    /// </summary>
    class GrassCoreReceiver : IDamageReceiver
    {
        private ElementEvent<IElementalDamage> onReceiveDamage = new ElementEvent<IElementalDamage>();
        public int Health { get; set; } = 0;
        public int AtkPower { get; set; } = 0;
        public GameObject GameObject { get; set; }

        public GameObject OriginalReference => Bloom.Seed;

        public void AddEffect(IEffect effect)
        {

        }

        public void AddElement(Elements element)
        {

        }

        public void AddOnElementReactedListener(Elements element, Action<ElementsReaction> action)
        {

        }

        public void AddOnReceiveAllDamageListener(Action<IElementalDamage> action)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加草原核受到伤害监听
        /// </summary>
        /// <param name="element"></param>
        /// <param name="action"></param>
        public void AddOnReceiveDamageListener(Elements element, Action<IElementalDamage> action)
        {
            onReceiveDamage.AddListener(element, action);
        }

        public void Dispose()
        {
            onReceiveDamage = null;
        }

        public Elements[] GetAllElements()
        {
            return null;
        }

        public List<IEffect> GetEffects()
        {
            return null;
        }

        public bool ReceiveDamage(IElementalDamage damage)
        {
            onReceiveDamage.Trigger(damage.ElementType, damage);

            return false;//不要让子弹的物体消失
        }

        public void RemoveEffect(IEffect effect)
        {

        }

        public void RemoveElement(Elements element)
        {

        }

        public void RemoveOnElementReactedListener(Elements element, Action<ElementsReaction> action)
        {

        }

        public void RemoveOnReceiveAllDamageListener(Action<IElementalDamage> action)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移除草原核受到伤害监听
        /// </summary>
        /// <param name="element"></param>
        /// <param name="action"></param>
        public void RemoveOnReceiveDamageListener(Elements element, Action<IElementalDamage> action)
        {
            onReceiveDamage.RemoveListener(element, action);
        }
    }
}
