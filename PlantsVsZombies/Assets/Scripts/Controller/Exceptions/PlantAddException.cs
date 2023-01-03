using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 植物放置时出现的异常
/// </summary>
public abstract class PlantAddException : System.Exception
{
    public PlantAddException(string msg) : base(msg) { }
    
    /// <summary>
    /// 放置植物时冷却时间不够触发的异常
    /// </summary>
    public class NotCooledDownYet : PlantAddException
    {
        public NotCooledDownYet(string msg) : base(msg) { }
    }
    /// <summary>
    /// 放置植物时能量不够触发的异常
    /// </summary>
    public class NotEnoughEnergy : PlantAddException
    {
        public NotEnoughEnergy(string msg) : base(msg) { }
    }
    /// <summary>
    /// 放置植物时格点已经被占用的异常
    /// </summary>
    public class SpaceOccupied : PlantAddException
    {
        public SpaceOccupied(string msg) : base(msg) { }
    }
    /// <summary>
    /// 放置植物的坐标在地图之外的异常
    /// </summary>
    public class OutOfBorder : PlantAddException
    {
        public OutOfBorder(string msg) : base(msg) { }
    }
}
