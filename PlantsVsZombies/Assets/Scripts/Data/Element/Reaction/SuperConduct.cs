using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����
/// </summary>
public class SuperConduct : ElementsReaction, IEffect
{
    private static float radius = 0.8f;//����Ч�������İ뾶
    private static int damage = 20;//����Ч�����˺�
    private static int duration = 6000;//����Ч������ʱ��
    private static float electricalResChange = -0.35f; //��Ԫ�ؿ��Խ���
    private static float iceResChange = -0.35f; // ��Ԫ�ؿ��Խ���

    private ResistanceEffect electricEffect = new ResistanceEffect(Elements.Electric, electricalResChange, duration, system); //��Ԫ�ؿ��Խ���Ч��
    private ResistanceEffect iceEffect = new ResistanceEffect(Elements.Ice, iceResChange, duration, system);//��Ԫ�ؿ��Խ���Ч��

    public override string ReactionName => "SuperConduct";

    public string EffectName => ReactionName;

    public EffectState State
    {
        get
        {
            return electricEffect.State; //ʹ����Ԫ�ؿ���Ч����״̬��Ϊ����Ч������״̬
        }
    }

    public IGameobjectData Caster => system;




    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        //��Χ���
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        foreach (var collider in colliders)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null && damageable is Monster)
            {
                IDamageReceiver receiver = damageable.GetReceiver();
                if(receiver != null)
                {
                    //�ܵ��˺�
                    receiver.ReceiveDamage(new SystemDamage(SuperConduct.damage, Elements.Ice));
                    IEffect effect = receiver.GetEffects().Find((effect) => effect is SuperConduct);
                    if(effect != null) //����û�г���Ч���������
                    {

                        receiver.AddEffect(new SuperConduct());
                    }
                }
            }
        }
    }

    public void EnableEffect(IGameobjectData target)
    {
        electricEffect.EnableEffect(target);
        iceEffect.EnableEffect(target);
    }

    public void DisableEffect(IGameobjectData target)
    {
        electricEffect.DisableEffect(target);
        iceEffect.DisableEffect(target);
    }

    public void UpdateEffect(IGameobjectData target)
    {

    }
}
