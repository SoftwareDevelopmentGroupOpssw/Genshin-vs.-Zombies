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
    private bool isTriggered;//�Ƿ��Ѿ�����
    /// <summary>
    /// һ����ԭ�˵��˺�������
    /// </summary>
    private GrassCoreReceiver receiver = new GrassCoreReceiver();
    public IDamageReceiver GetReceiver() => receiver;

    /// <summary>
    /// ����ϵͳ������ԭ����
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
    /// ����ϵͳ����������������
    /// </summary>
    void Animator_Hyper()
    {
        Bloom.TriggerHyperBloom(transform.position);
    }
    /// <summary>
    /// ����ϵͳ����������������
    /// </summary>
    void Animator_Pyro()
    {
        Bloom.TriggerPyroBloom(transform.position);
    }
    /// <summary>
    /// ������Ԫ��
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
    /// ������Ԫ��
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
    /// �ȵ�ʱ�䵽ʱ����
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
    /// ��ԭ���˺�������
    /// ��ԭ�˲������Ч����Ҳ�����ܵ�Ԫ�ظ���
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
        /// ��Ӳ�ԭ���ܵ��˺�����
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

            return false;//��Ҫ���ӵ���������ʧ
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
        /// �Ƴ���ԭ���ܵ��˺�����
        /// </summary>
        /// <param name="element"></param>
        /// <param name="action"></param>
        public void RemoveOnReceiveDamageListener(Elements element, Action<IElementalDamage> action)
        {
            onReceiveDamage.RemoveListener(element, action);
        }
    }
}
