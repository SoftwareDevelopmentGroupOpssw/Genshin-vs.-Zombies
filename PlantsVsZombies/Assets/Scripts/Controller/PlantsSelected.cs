using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 已选择的植物
/// </summary>
public class PlantsSelected
{
    private IPlantData data;
    private float timeLeft;//剩余时间

    public PlantsSelected(IPlantData data)
    {
        this.data = data;
        timeLeft = 0;
    }

    /// <summary>
    /// 植物数据
    /// </summary>
    public IPlantData Data => data;

    /// <summary>
    /// 这个植物冷却时间进度的百分比
    /// 为0时刚开始冷却，为1时冷却完成
    /// </summary>
    public float IsReady => timeLeft / data.CoolTime;

    /// <summary>
    /// 开始倒计时，计算冷却时间
    /// </summary>
    public void StartCoolTime()
    {
        void Update()//公用mono模块帧更新函数
        {
            timeLeft -= Time.unscaledDeltaTime;
            if(timeLeft <= 0)
            {
                timeLeft = 0;
                MonoManager.Instance.RemoveUpdateListener(Update);
            }
        }
        timeLeft = data.CoolTime;//设定好开始时间
        MonoManager.Instance.AddUpdateListener(Update);//将Update送入公用mono模块
    }
}
