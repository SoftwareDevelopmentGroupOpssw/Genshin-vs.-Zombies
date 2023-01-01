using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ᾧ
/// </summary>
public class Crystallize : ElementsReaction
{
    public const int SHEILD_POWER = 20;
    public const int SHEILD_MILISECONDS_DURATION = 5000;

    private const string CRYSTAL_PATH = "ElementReaction/Crystal";
    private const string SHIELD_PATH = "ElementReaction/Shield";
    private static GameObject crystal;//�ᾧ���ɵľ�Ƭ������ʱ����һ��
    public static GameObject CrystalPrefab
    {
        get
        {
            if(crystal == null)
            {
                crystal = ResourceManager.Instance.Load<GameObject>(CRYSTAL_PATH);//ȥ���ؾ�Ƭ����
            }
            return crystal;
        }
    }
    private static GameObject shield;//�ᾧʱ���ɵĻ���
    public static GameObject ShieldPrefab
    {
        get
        {
            if(shield == null)
            {
                shield = ResourceManager.Instance.Load<GameObject>(SHIELD_PATH);
            }
            return shield;
        }
    }


    public static readonly string bufferName = "Crystalize";
    public static ObjectBuffer CrystalBuffer
    {
        get
        {
            EntitiesController controller = GameController.Instance.EntitiesController;
            if (!controller.ContainsBuffer(bufferName))
            {
                controller.AddBufferToManagement(bufferName, new ObjectBuffer(FlyersController.FlyersFatherObject.transform));
                controller.AddBehaviourManagement<Crystal>(bufferName, CrystalPrefab);
            }
            return controller[bufferName];
        }
    }
    public static GameObject AddCrystal(Vector3 location)
    {
        GameObject obj = CrystalBuffer.Get(CrystalPrefab);
        obj.transform.position = location;
        return obj;
    }
    public static void RemoveCrystal(Crystal crystal)
    {
        CrystalBuffer.Put(CrystalPrefab, crystal.gameObject);
    }

    private static int crystallizeDamage = 5;

    public override string ReactionName => "Crystallize";

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        #region ��Ƭ�ĳ�������
        IEnumerator CrystalFlyingCoroutine(Crystal crystal)
        {
            //��Ƭ�ĳ����ľ���
            float offset = 0.5f;
            float farestLocation = 0.35f;
            float nearestLocation = 0.2f;
            int sign = Random.value - offset > 0 ? 1 : -1;
            float now = Random.Range(sign * nearestLocation, sign * farestLocation);

            Vector3 startPos = crystal.transform.position;
            float flySpeed = 1;
            float polonomialArg = 15;

            for (float x = 0; Mathf.Abs(x) < Mathf.Abs(now); x += sign * flySpeed * Time.deltaTime)
            {
                float y = -polonomialArg * x * (x - now);
                crystal.transform.position = startPos + new Vector3(x, y, 0);
                yield return 1;
            }

            //�ĳ����� �Ե�һ���
            yield return new WaitForSecondsRealtime(0.5f);

            crystal.StartTracking();//��ʼ׷��
        }
        #endregion

        damage.AtkDmg += crystallizeDamage;

        //�ڹ���Ľ�������һ���ᾧ��Ƭ
        GameObject obj = AddCrystal(target.GameObject.transform.position);
        Crystal crystal = obj.GetComponent<Crystal>();
        crystal.StartCoroutine(CrystalFlyingCoroutine(crystal));

    }
}
