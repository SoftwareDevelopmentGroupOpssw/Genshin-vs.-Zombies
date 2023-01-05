using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// �ؿ�2
/// </summary>
public class Level2 : GridLevel
{
    private int row = 5;
    private int col = 10;
    private Sprite sprite;
    private float secondsBetweenFallingEnergy = 5f;//�����ϵ�������ļ��ʱ��

    private const float commonPossiblity = 0.4f;//��ͨ��ʬ����
    private const float roadConePossiblity = 0.3f;//·�Ͻ�ʬ����
    private const float bucketPossiblity = 0.3f;//��Ͱ��ʬ����

    private const int commonWeight = 1000;//��ͨ��ʬȨֵ
    private const int roadWeight = 2000;//·�Ͻ�ʬȨֵ
    private const int bucketWeight = 5000;//��Ͱ��ʬȨֵ

    private const int waveCount = 4; // ��ʬ����
    private static int nowWave = 0; //�Ѿ����ɵĲ���

    public Level2(Sprite sprite)
    {
        this.sprite = sprite;
    }
    public override int Row => row;

    public override int Col => col;

    public override Sprite Sprite => sprite;

    IEnumerator GenerateEnergy()
    {

        int offsetX = Screen.width / 10;
        int offsetY = Screen.height / 5;
        System.Random r = new System.Random();
        do //��Ϸ������ʱһֱ���������
        {
            yield return new WaitForSecondsRealtime(secondsBetweenFallingEnergy);

            Vector2 endPoint = new Vector2(r.Next(offsetX, Screen.width - offsetX), r.Next(offsetY, Screen.height - offsetY));
            Vector2 startPoint = new Vector2(endPoint.x, Screen.height);
            GameObject obj = EnergyMonitor.CreateEnergy(new Vector2Int((int)startPoint.x, (int)startPoint.y), EnergyType.Big); //����Ļ֮������
            GameController.Instance.StartCoroutine(obj.GetComponent<Energy>().FallingCoroutine(obj, startPoint, endPoint));
        } while (GameController.Instance.IsGameStarted);
    }

    /// <summary>
    /// �ؿ�1�����ֲ��ԡ���һ��ʱ�������ˢһ����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator GenerateEnumerator()
    {
        nowWave = 0;
        GameController.Instance.StartCoroutine(GenerateEnergy());
        //��һֻ���������60�봦
        yield return new WaitForSecondsRealtime(45f);
        AudioManager.Instance.PlayEffectAudio("awooga");

        int monsterTotalCount = 80;//��������
        float geneateSpaceTime = 15f;//���ɼ��
        int realMinGenerateCount = 1;//��С��������
        int realMaxGenerateCount = 15;//�����������
        int fixedCount = 8; //������ֵ�����ؿ����ȴﵽ100%ʱ ����������������ô�� 
        int moreSummoned = 8; //���ģ��ʬ����ʱ�������ɵ�����

        int nowGenerated = 0;
        int generateAmount = 0;

        System.Random r = new System.Random();
        while (nowWave < waveCount)
        {
            int minGenerateCount = realMinGenerateCount + nowGenerated * fixedCount / monsterTotalCount; //���ݹؿ��������������С��������
            int maxGenerateCount = realMaxGenerateCount + nowGenerated * fixedCount / monsterTotalCount; //���ݹؿ�����������������������
            int totalWeight = nowGenerated * 2500 + 1000; //������������������ Ȩֵ��Ҳ��Խ��Խ��
            bool isBigWave = false;
            Debug.Log(nowGenerated + "generated. now wave" + nowWave);
            if (waveCount * nowGenerated / monsterTotalCount != nowWave) // �����ɵĹ���������������ȴﵽ1/waveCountʱ��������һ�δ��ģ��ʬ��Ȩֵ����ߣ�������������
            {
                isBigWave = true;
                totalWeight += totalWeight / 2; //Ȩֵ��Ϊ1.5��
                do
                {
                    generateAmount = r.Next(minGenerateCount + moreSummoned, maxGenerateCount + moreSummoned + 1);
                } while (generateAmount * commonWeight > totalWeight);//���ȫ������Ȩֵ��С����ͨ��ʬ�Ի�������Ȩ�أ���˴����ɵ�generateAmout���Ϸ�
                //���ģ��ʬ������������
                nowWave++;
            }
            else//���Ǵ��ģ ��������
            {
                do
                {
                    generateAmount = r.Next(minGenerateCount, maxGenerateCount + 1);
                } while (generateAmount * commonWeight > totalWeight);//���ȫ������Ȩֵ��С����ͨ��ʬ�Ի�������Ȩ�أ���˴����ɵ�generateAmout���Ϸ�
                generateAmount = Mathf.Min(generateAmount, monsterTotalCount - nowGenerated);
            }

            var task = Task.Run<List<IMonsterData>>(() => CalculateFunction(totalWeight, generateAmount));
            while (!task.IsCompleted)
                yield return 1;
            if (isBigWave)
            {
                AudioManager.Instance.PlayEffectAudio("hugewave"); //��ʾ���ģ��
                yield return new WaitForSecondsRealtime(5);//��ʾ�����ʼ����
                if (nowWave == waveCount) //���һ��
                {
                    AudioManager.Instance.PlayEffectAudio("finalwave");
                }
                AudioManager.Instance.PlayEffectAudio("siren");
                nowGenerated -= generateAmount; // ���ģ���ֵĲ����ڹؿ�������
                yield return MonsterPrefabSerializer.Instance.GetMonsterData("FlagZombie");//����ҡ�콩ʬ
            }
            foreach (IMonsterData tried in task.Result)
                yield return tried;
            nowGenerated += generateAmount;
            yield return new WaitForSecondsRealtime(geneateSpaceTime);
        }
    }
    /// <summary>
    /// ����Ȩֵ������������б�
    /// </summary>
    /// <param name="totalWeight"></param>
    /// <returns></returns>
    private List<IMonsterData> CalculateFunction(int totalWeight, int generateAmount)
    {
        int i = 0;
        List<IMonsterData> triedGenerated = new List<IMonsterData>();
        int nowWeight = 0;
        System.Random r = new System.Random();

        Debug.Log("total" + totalWeight + "amount " + generateAmount);
        do
        {
            if (totalWeight - nowWeight < bucketWeight && totalWeight - nowWeight < roadWeight)
            {
                for (; i < generateAmount; i++)
                {
                    triedGenerated.Add(MonsterPrefabSerializer.Instance.GetMonsterData("CommonZombie"));
                    nowWeight += commonWeight; //ʣ������Ȩֵ��������ͨ��ʬ
                }
                return triedGenerated;
            }
            double random = r.NextDouble();
            if (random < commonPossiblity)
            {
                triedGenerated.Add(MonsterPrefabSerializer.Instance.GetMonsterData("CommonZombie"));
                nowWeight += commonWeight;
            }
            else if (random < commonPossiblity + roadConePossiblity && totalWeight - nowWeight > roadWeight)
            {
                triedGenerated.Add(MonsterPrefabSerializer.Instance.GetMonsterData("RoadConeZombie"));
                nowWeight += roadWeight;
            }
            else if (random < commonPossiblity + roadConePossiblity + bucketPossiblity && totalWeight - nowWeight > bucketWeight)
            {
                triedGenerated.Add(MonsterPrefabSerializer.Instance.GetMonsterData("BucketHeadZombie"));
                nowWeight += bucketWeight;
            }
            i++;

        } while (i < generateAmount);
        return triedGenerated;
    }

    protected override Vector2Int GridsLeftTopCornorPos => new Vector2Int(128, 128);

    protected override int GridWidth => 137;

    protected override int GridHeight => 134;

    private List<string> monsterNames = new List<string>()
    {
        "CommonZombie","CommonZombie","RoadConeZombie","BucketHeadZombie"
    };

    public override IMonsterData[] MonsterTypes
    {
        get
        {
            int index = 0;
            IMonsterData[] array = new IMonsterData[monsterNames.Count];
            foreach (string monsterName in monsterNames)
            {
                array[index++] = MonsterPrefabSerializer.Instance.GetMonsterData(monsterName);
            }
            return array;
        }
    }
}
