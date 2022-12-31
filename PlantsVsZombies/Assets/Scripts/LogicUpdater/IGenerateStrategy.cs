using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成怪物的策略函数
/// </summary>
public interface IGenerateStrategy
{
    /// <summary>
    /// 生成的迭代器
    /// 每次迭代会处理相应逻辑
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateEnumerator();
}
