using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MonsterData : IMonsterData
{
    /// <summary>
    /// ���ϵ�Ч��
    /// </summary>
    private List <IEffect> effects = new List<IEffect>();
    /// <summary>
    /// ����һ��֡����ʱ��Ҫɾ����Ч��
    /// </summary>
    private List<IEffect> removeList = new List<IEffect>();

    /// <summary>
    /// ���ŵ�Ԫ��
    /// </summary>
    private bool[] elements = new bool[8];

    /// <summary>
    /// ���ܵ���Ԫ�������˺�ʱ���õĺ���
    /// </summary>
    private UnityAction<IElementalDamage>[] OnReceivedDamage = new UnityAction<IElementalDamage>[8];
    /// <summary>
    /// ���ܵ���Ԫ�����͸���ʱ���õĺ���
    /// </summary>
    private UnityAction[] OnAddElement = new UnityAction[8];
    /// <summary>
    /// Ԫ�ظ��ű���Ӧ��ʱ���õĺ���
    /// </summary>
    private UnityAction<ElementsReaction>[] OnElementReacted = new UnityAction<ElementsReaction>[8];

    /// <summary>
    /// ����ֵ
    /// </summary>
    private float[] resistances = new float[8];

    /// <summary>
    /// �ڴ˴�Update�����У��Ƿ��ܽ���Action����
    /// ����ܵ���ѣ��Ч�����Ͳ��ܽ���Action����
    /// </summary>
    private bool canAction = true;
    public bool CanAction => canAction;

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
    public void RemoveEffect(IEffect effect) => removeList.Add(effect);

    public abstract IGameobjectData Instantiate();

    public virtual void OnAwake() { }

    public virtual void OnDestroy() { }

    public void ReceiveDamage(IElementalDamage damage)
    {
        //���غ�����û�з�Ӧ���������ǿ��Ժ͸���
        void PlainDealsDamage(IElementalDamage damage)
        {
            int elementNum = (int)damage.ElementType;
            //ΪԪ���˺����ܸ���Ԫ��
            if(damage.ElementType != Elements.None && damage.CanAddElement) //�����˺��������Ԫ��
            {
                OnAddElement[elementNum]?.Invoke();
                elements[elementNum] = true;
            }
            OnReceivedDamage[elementNum]?.Invoke(damage);

            float value = GetResistance(damage.ElementType);
            health -= (int)(damage.AtkDmg * (1 - value));
        }
        //ΪԪ���˺������ܸ���Ԫ��
        if (damage.ElementType != Elements.None && damage.CanAddElement)//�����˺��������Ԫ��
        {
            Elements causerElement = damage.ElementType;
            
            int i = 0;
            for (; i < elements.Length; i++)
            {
                if (!elements[i])//û�����Ԫ��
                    continue;
                Elements element = (Elements)i;
                ElementsReaction reaction = ElementsReaction.GetReaction(element, causerElement);//������Ԫ�ط�Ӧ
                if (reaction != null)//����Ԫ�ط�Ӧ�����䷢��
                {
                    OnElementReacted[(int)element]?.Invoke(reaction);
                    OnElementReacted[(int)causerElement]?.Invoke(reaction);
                    
                    reaction.Action(damage, this);
                    
                    elements[i] = false;
                    damage.CanAddElement = false;//�Ѿ������귴Ӧ����˴˴ι������ٸ���Ԫ��
                    break;
                }
            }
        }
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
    /// �������Ч��
    /// </summary>
    private void CheckAllEffect()
    {
        RemoveUselessEffect();
        for(int i = effects.Count - 1;i >= 0; i--)
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
                    RemoveEffect(effect);//��Ч����ӵ���ɾ���б���
                    continue;
            }
        }
    }
    private void RemoveUselessEffect()
    {
        foreach(var effect in removeList)
        {
            effects.Remove(effect);
            DisableEffect(effect);
        }
        removeList.Clear();
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
        else if(effect is Frozen)//������Ч��Ϊ����Ч��ʱ
        {
            void FrozenEffect_OnIceReacted(ElementsReaction reaction)//�ܵ�����Ч��ʱ ����Ԫ�ظ�����ʧ �������߼�
            {
                if (reaction.Name != "Frozen") //�����˱��Ԫ�ط�Ӧ
                {
                    RemoveEffect(effect);//ǿ���Ƴ�����Ч��
                    OnElementReacted[(int)Elements.Ice] -= FrozenEffect_OnIceReacted;
                }
            }
            void FrozenEffect_OnReceivePhysicalDamage(IElementalDamage damage)//���ڶ���������ܵ������˺�ʱ�������Ч��
            {
                RemoveEffect(effect);
                health -= Frozen.BROKE_ICE_DAMAGE;//ֱ�Ӽ�������ֵ�������ʵ�˺�

                OnReceivedDamage[(int)Elements.None] -= FrozenEffect_OnReceivePhysicalDamage;
            }
            Frozen frozen = effect as Frozen;
            //ѣ�κͼ���״̬�ĵ���
            EnableEffect(frozen.Speed);
            EnableEffect(frozen.Stun);
            ReceiveDamage(new SystemDamage(0, Elements.Ice, true));//��ӱ�Ԫ�ظ���
            OnElementReacted[(int)Elements.Ice] += FrozenEffect_OnIceReacted;//���Ԫ�ظ��ż���
            OnReceivedDamage[(int)Elements.None] += FrozenEffect_OnReceivePhysicalDamage;//��������˺�����
        }
    }

    /// <summary>
    /// �Զ�����Ч���ĳ���ʱ�������Ч��
    /// </summary>
    /// <param name="effect"></param>
    private void UpdateEffect(IEffect effect)
    {
        if (effect is StunEffect)
            canAction = false; //�����ѣ��Ч����ÿһ֡�����ܽ��в���
        else if(effect is Frozen)
        {
            Frozen frozen = effect as Frozen;
            if (frozen.Stun.State == EffectState.Processing)
                UpdateEffect(frozen.Stun);
        }
    }
    /// <summary>
    ///�Զ���ֹͣ����Ч��
    /// </summary>
    /// <param name="effect">Ч��</param>
    private void DisableEffect(IEffect effect)
    {
        if (effect is SpeedEffect)
            (effect as SpeedEffect).DisableEffect(ref speed);
        else if (effect is Frozen)
        {
            DisableEffect((effect as Frozen).Speed);
        }
    }
    public void Action()
    {
        CheckAllEffect();
        if (canAction)//û���յ�Ч�������Լ�������
            RealAction();
        else
            canAction = true;//���µ�������һ֡���ж�
    }
}
