using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏物体接口
/// </summary>
public interface IGameobjectData : System.IDisposable
{
    /// <summary>
    /// 向此对象身上添加效果
    /// </summary>
    /// <param name="effect">效果</param>
    public void AddEffect(IEffect effect);
    /// <summary>
    /// 向此对象身上移除效果
    /// </summary>
    /// <param name="effect">效果</param>
    public void RemoveEffect(IEffect effect);
    /// <summary>
    /// 获得效果列表
    /// </summary>
    /// <returns>效果列表</returns>
    public List<IEffect> GetEffects();
    /// <summary>
    /// 获取/设置 当前data在场景中依附的游戏物体对象
    /// </summary>
    public GameObject GameObject { get; set; }
    /// <summary>
    /// 游戏物体的原始预制体，使用它进行实例化操作
    /// </summary>
    public GameObject OriginalReference { get; }
}
