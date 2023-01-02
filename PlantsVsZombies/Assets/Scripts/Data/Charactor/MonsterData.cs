using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MonsterData : IMonsterData, IDamageReceiver
{
    /// <summary>
    /// ԭʼԤ����
    /// </summary>
    private GameObject original;

    /// <summary>
    /// ���ϵ�Ч��
    /// </summary>
    private List <IEffect> effects = new List<IEffect>();

    /// <summary>
    /// ���ŵ�Ԫ��
    /// </summary>
    protected ElementalObject<bool> elements = new ElementalObject<bool>();

    /// <summary>
    /// ����ֵ
    /// </summary>
    protected ElementalObject<float> resistances = new ElementalObject<float>();

    /// <summary>
    /// ���ܵ���Ԫ�������˺�ʱ���õĺ���
    /// </summary>
    private ElementEvent<IElementalDamage> OnReceivedDamage = new ElementEvent<IElementalDamage>();
    public void AddOnReceiveDamageListener(Elements element, System.Action<IElementalDamage> action) => OnReceivedDamage.AddListener(element, action);
    public void RemoveOnReceiveDamageListener(Elements element, System.Action<IElementalDamage> action) => OnReceivedDamage.RemoveListener(element, action);
    public void AddOnReceiveAllDamageListener(Action<IElementalDamage> action) => OnReceivedDamage.AddAllListener(action);
    public void RemoveOnReceiveAllDamageListener(Action<IElementalDamage> action) => OnReceivedDamage.RemoveAllListener(action);

    /// <summary>
    /// ���ܵ���Ԫ�����͸���ʱ���õĺ���
    /// </summary>
    private ElementEvent OnAddElement = new ElementEvent();

    public void AddOnAddElementListener(Elements element, System.Action action) => OnAddElement.AddListener(element, action);
    public void RemoveOnAddElementListener(Elements element, System.Action action) => OnAddElement.RemoveListener(element, action);
    
    /// <summary>
    /// Ԫ�ظ��ű���Ӧ��ʱ���õĺ���
    /// </summary>
    private ElementEvent<ElementsReaction> OnElementReacted = new ElementEvent<ElementsReaction>();

    public void AddOnElementReactedListener(Elements element, System.Action<ElementsReaction> action) => OnElementReacted.AddListener(element, action);
    public void RemoveOnElementReactedListener(Elements element, System.Action<ElementsReaction> action) => OnElementReacted.RemoveListener(element, action);

    /// <summary>
    /// ��һ��ԭʼԤ�����������ʼ��
    /// </summary>
    /// <param name="original">���ħ���ԭʼԤ��������</param>
    protected MonsterData(GameObject original)
    {
        this.original = original;
    }


    protected int strength;
    public int Strength { get => strength; set => strength = value; }

    protected int speed;
    public int Speed { get => speed; set =>speed = value; }

    protected int health;
    public int Health { get => health; set => health = value; }

    protected int atkPower;
    public int AtkPower { get => atkPower; set => atkPower = value; }

    public GameObject GameObject { get; set; }
    public GameObject OriginalReference => original;

    public float GetResistance(Elements element) => resistances[element];
    public void SetResistance(float value, Elements element) => resistances[element] = value;

    public void AddEffect(IEffect effect) => effects.Add(effect);
    public void RemoveEffect(IEffect effect) => effects.Remove(effect);
    public List<IEffect> GetEffects() => effects;

    public virtual void OnAwake() { }

    public virtual void OnDestroy() { }

    public bool ReceiveDamage(IElementalDamage damage)
    {
        //���غ�����û�з�Ӧ���������ǿ��Ժ͸���
        void PlainDealsDamage(IElementalDamage damage)
        {
            //ΪԪ���˺����ܸ���Ԫ��
            if(damage.CanAddElement) //�����Ԫ��
            {
                //�����˺�����Ԫ���˺�����Ԫ���˺��������Ԫ�ظ���
                if(damage.ElementType != Elements.None && damage.ElementType != Elements.Wind && damage.ElementType != Elements.Stone)
                {
                    OnAddElement.Trigger(damage.ElementType);//�������Ԫ���߼�
                    elements[damage.ElementType] = true;
                }
            }
            OnReceivedDamage.Trigger(damage.ElementType,damage);//�����ܵ��˺��߼�
            OnReceivedDamage.TriggerAll(damage);//�����������͵ļ���

            float value = GetResistance(damage.ElementType);
            health -= (int)(damage.Damage * (1 - value));
        }
        //ΪԪ���˺������ܸ���Ԫ��
        if (damage.ElementType != Elements.None && damage.CanAddElement)//�����˺��������Ԫ��
        {
            Elements causerElement = damage.ElementType;
            
            foreach (var item in elements)
            {
                if (item.Value == false)//û�����Ԫ��
                    continue;
                Elements element = item.Key;
                ElementsReaction reaction = ElementsReaction.GetReaction(element, causerElement);//������Ԫ�ط�Ӧ
                if (reaction != null)//����Ԫ�ط�Ӧ�����䷢��
                {
                    OnElementReacted.Trigger(element,reaction);//����ԭ��Ԫ�صķ�Ӧ�߼�
                    OnElementReacted.Trigger(causerElement, reaction);//���������Ԫ�صķ�Ӧ�߼�


                    elements[element] = false;
                    damage.CanAddElement = false;//�Ѿ������귴Ӧ����˴˴ι������ٸ���Ԫ��

                    reaction.Action(damage, this);



                    break;
                }
            }
        }
        PlainDealsDamage(damage);
        return true;
    }


    public void AddElement(Elements element) => elements[element] = true;
    public void RemoveElement(Elements element) => elements[element] = false;
    
    public Elements[] GetAllElements()
    {
        List<Elements> total = new List<Elements>();
        foreach(var item in elements)
        {
            if (item.Value == true)
                total.Add(item.Key);
        }
        return total.ToArray();
    }

    public void Dispose()
    {
        effects.Clear();
        effects = null;
        OnAddElement.Clear();
        OnElementReacted.Clear();
        OnReceivedDamage.Clear();
        GameObject = null;
    }
}
