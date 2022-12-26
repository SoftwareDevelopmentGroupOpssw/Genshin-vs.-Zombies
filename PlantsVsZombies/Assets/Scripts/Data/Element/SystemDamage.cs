using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ϵͳ��ɵ��˺�
/// </summary>
public class SystemDamage : IElementalDamage
{
    private static IGameobjectData system;
    
    public SystemDamage(int dmgValue, Elements type, bool canAddElement = false)
    {
        AtkDmg = dmgValue;
        ElementType = type;
        CanAddElement = canAddElement;
    }

    public int AtkDmg {get; private set;}

    public Elements ElementType { get; private set; }

    public bool CanAddElement { get; private set; }
}
