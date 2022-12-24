using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏物体接口
/// </summary>
public interface IGameobjectData
{
    /// <summary>
    /// 获取/设置 当前data在场景中依附的游戏物体对象
    /// </summary>
    public GameObject GameObject { get; set; }
    /// <summary>
    /// 游戏物体的资源路径，以便使用Resouces.Load方法加载
    /// </summary>
    public string ResourcePath { get; }
    /// <summary>
    /// 当游戏物体出现时调用
    /// </summary>
    public void OnAwake();
    /// <summary>
    /// 当游戏物体死亡时调用
    /// </summary>
    public void OnDestroy();
    /// <summary>
    /// 复制一份与当前数据相同类型，所有数据为默认值的Data
    /// </summary>
    public IGameobjectData Instantiate();
}
