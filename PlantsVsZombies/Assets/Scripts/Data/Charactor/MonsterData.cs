using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterData : IMonsterData
{
    /// <summary>
    /// ���ϵ�Ч��
    /// </summary>
    private List<IEffect> effects = new List<IEffect>();
    
    /// <summary>
    /// ���ŵ�Ԫ��
    /// </summary>
    private bool[] elements = new bool[8];
    
    /// <summary>
    /// ����ֵ
    /// </summary>
    private float[] resistances = new float[8];

    /// <summary>
    /// �ڴ˴�Update�����У��Ƿ��ܽ���Action����
    /// ����ܵ���ѣ��Ч�����Ͳ��ܽ���Action����
    /// </summary>
    private bool canAction = true;

    protected int strength;
    public int Strength { get => strength; }
    
    protected int speed;
    public int Speed { get => speed; }
    
    protected int health;
    public int Health { get => health; }

    protected int atkPower;
    public int AtkPower { get => atkPower; }

    public GameObject GameObject { get; set; }
    public abstract string ResourcePath { get; }

    public float GetResistance(Elements element) => resistances[(int)element];

    protected float SetResistance(float value, Elements element) => resistances[(int)element] = value;

    public void AddEffect(IEffect effect) => effects.Add(effect);

    public abstract IGameobjectData Instantiate();

    public virtual void OnAwake() { }

    public virtual void OnDestroy() { }

    public void ReceiveDamage(IElementalDamage damage)
    {
        //���غ���������˺�
        void PlainDealsDamage(IElementalDamage damage)
        {
            float value = GetResistance(damage.ElementType);
            health -= (int)(damage.AtkDmg * (1-value));
            if(damage.ElementType != Elements.None && damage.CanAddElement)
                elements[(int)damage.ElementType] = true;
        }
        if (damage.ElementType != Elements.None && damage.CanAddElement)
        {
            Elements causerElement = damage.ElementType;
            int i = 0;
            for (; i < elements.Length; i++)
            {
                if (!elements[i])
                    continue;
                Elements element = (Elements)i;
                ElementsReaction reaction = ElementsReaction.GetReaction(element, causerElement);
                if (reaction != null)
                {
                    reaction.Action(damage, this);
                    elements[i] = false;
                    break;
                }
            }
            if (i == elements.Length)//û���ҵ��ܹ���Ӧ��Ԫ��
                PlainDealsDamage(damage);
        }
        else
            PlainDealsDamage(damage);
        
    }

    protected void AddElement(Elements element)
    {
        elements[(int)element] = true;
    }

    protected void RemoveElement(Elements element)
    {
        elements[(int)element] = false;
    }
    protected Elements[] GetAllElements()
    {
        List<Elements> total = new List<Elements>();
        for(int i = 0; i < elements.Length; i++)
        {
            if (elements[i])
                total.Add((Elements)i);
        }
        return total.ToArray();
    }
    /// <summary>
    /// ������������Ч����������ʵ�Ĳ���
    /// </summary>
    protected abstract void RealAction();
    /// <summary>
    /// ��������Ч��
    /// </summary>
    private void EnableAllEffect()
    {
        List<int> deletes = new List<int>();
        for(int i = 0;i < effects.Count; i++)
        {
            IEffect effect = effects[i];
            switch (effect.State)
            {
                case EffectState.Initialized:
                    EnableEffect(effect);
                    break;
                case EffectState.Processing:
                    UpdateEffect(effect);
                    break;
                case EffectState.End:
                    deletes.Add(i);
                    continue;
            }
        }
        foreach(int index in deletes)
        {
            IEffect effect = effects[index];
            effects.RemoveAt(index);
            DisableEffect(effect);
        }
    }
    /// <summary>
    /// �Զ������õ���Ч��
    /// </summary>
    /// <param name="effect">Ч��</param>
    private void EnableEffect(IEffect effect)
    {
        if (effect is SpeedEffect)
            (effect as SpeedEffect).EnableEffect(ref speed);
        else if(effect is StunEffect)
        {
            canAction = false;
            (effect as StunEffect).EnableEffect();
        }
    }
    /// <summary>
    /// �Զ�����Ч���ĳ���ʱ�������Ч��
    /// </summary>
    /// <param name="effect"></param>
    private void UpdateEffect(IEffect effect)
    {
        if (effect is StunEffect)//�����ѣ��Ч����ÿһ֡�����ܽ��в���
            canAction = false;
    }
    /// <summary>
    ///�Զ���ֹͣ����Ч��
    /// </summary>
    /// <param name="effect">Ч��</param>
    private void DisableEffect(IEffect effect)
    {
        if(effect is SpeedEffect)
            (effect as SpeedEffect).DisableEffect(ref speed);
    }
    public void Action()
    {
        EnableAllEffect();
        if (canAction)//û���յ�Ч�������Լ�������
            RealAction();
        else
            canAction = true;//���µ�������һ֡���ж�
    }
}
