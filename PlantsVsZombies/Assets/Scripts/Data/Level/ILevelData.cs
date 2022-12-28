using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������ת��Ϊ��������ʱ��ʹ�õĲο���λ��
/// </summary>
public enum GridPosition
{
    /// <summary>
    /// ��������м������λ��
    /// </summary>
    Left,
    /// <summary>
    /// ���������ĵ�����λ��
    /// </summary>
    Middle,
    /// <summary>
    /// �����ұ��м������λ��
    /// </summary>
    Right,
}
public interface ILevelData
{
    /// <summary>
    /// ����
    /// </summary>
    public int Row { get; }
    /// <summary>
    /// ����
    /// </summary>
    public int Col { get; }
    /// <summary>
    /// ��ͼ��Sprite
    /// </summary>
    public Sprite Sprite { get; }
    /// <summary>
    /// ��ͼ�ĳ�������
    /// </summary>
    public Queue<IMonsterData> MonsterList { get; }
    /// <summary>
    /// ��һ����������ת��Ϊ��ͼ�ĸ�������
    /// �������������Ͻ�Ϊԭ�㣬����Ϊx�ᣬ����Ϊy��
    /// </summary>
    /// <param name="worldPos">��������</param>
    /// <param name="levelPos">���ڹؿ�����������λ��</param>
    /// <returns>�������꣬����ڸ���֮���򷵻�(-1,-1)</returns>
    public Vector2Int WorldToGrid(Vector3 worldPos, Vector3 levelPos);
    /// <summary>
    /// ��һ����������ת��Ϊ��������
    /// �������������Ͻ�Ϊԭ�㣬����Ϊx�ᣬ����Ϊy��
    /// </summary>
    /// <param name="gridPos">��������</param>
    /// <param name="pos">ת��Ϊ��������ʱ��ƫ��ö��</param>
    /// <param name="levelPos">���ڹؿ�����������λ��</param>
    /// <returns>��Ӧƫ�ƴ�����������</returns>
    public Vector3 GridToWorld(Vector2Int gridPos, GridPosition pos,Vector3 levelPos);
}
