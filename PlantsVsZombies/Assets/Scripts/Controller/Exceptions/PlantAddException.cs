using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ֲ�����ʱ���ֵ��쳣
/// </summary>
public abstract class PlantAddException : System.Exception
{
    public PlantAddException(string msg) : base(msg) { }
    
    /// <summary>
    /// ����ֲ��ʱ��ȴʱ�䲻���������쳣
    /// </summary>
    public class NotCooledDownYet : PlantAddException
    {
        public NotCooledDownYet(string msg) : base(msg) { }
    }
    /// <summary>
    /// ����ֲ��ʱ���������������쳣
    /// </summary>
    public class NotEnoughEnergy : PlantAddException
    {
        public NotEnoughEnergy(string msg) : base(msg) { }
    }
    /// <summary>
    /// ����ֲ��ʱ����Ѿ���ռ�õ��쳣
    /// </summary>
    public class SpaceOccupied : PlantAddException
    {
        public SpaceOccupied(string msg) : base(msg) { }
    }
    /// <summary>
    /// ����ֲ��������ڵ�ͼ֮����쳣
    /// </summary>
    public class OutOfBorder : PlantAddException
    {
        public OutOfBorder(string msg) : base(msg) { }
    }
}
