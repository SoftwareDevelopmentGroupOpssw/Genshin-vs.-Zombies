using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level1:Ĭ�Ϲؿ�
/// </summary>
public class Level1 : GridLevel
{
    private int row = 5;
    private int col = 10;
    private Sprite sprite;

    public Level1(Sprite sprite)
    {
        this.sprite = sprite;
    }
    public override int Row => row;

    public override int Col => col;

    public override Sprite Sprite => sprite;

    /// <summary>
    /// �ؿ�1�����ֲ��ԡ���һ��ʱ�������ˢһ����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator GenerateEnumerator()
    {
        int monsterTotalCount = 20;//��������
        float geneateSpaceTime = 5f;//���ɼ��
        int minGenerateCount = 2;//��С��������
        int maxGenerateCount = 8;//�����������

        int nowGenerated = 0;
        int generateAmount = 0;
        System.Random r = new System.Random();
        for(; nowGenerated < monsterTotalCount; nowGenerated += generateAmount)
        {
            generateAmount = r.Next(minGenerateCount, maxGenerateCount);
            for(int i = 0; i < Mathf.Min(generateAmount, monsterTotalCount - nowGenerated); i++)
            {
                yield return MonsterPrefabSerializer.Instance.GetMonsterData("CommonZombie");
            }
            yield return new WaitForSecondsRealtime(geneateSpaceTime);
        }
    }

    protected override Vector2Int GridsLeftTopCornorPos => new Vector2Int(128, 128);

    protected override int GridWidth => 137;

    protected override int GridHeight => 134;


}
