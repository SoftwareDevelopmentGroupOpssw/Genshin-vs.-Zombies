using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class role : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool flag = false;
    public int fun = 0;
    Vector3 pos1;
    int sum = 0, q = -1;

    // float x = 0;
    // x=GameMa.tx;
    // float y = GameMa.ty;
    // float z = GameMa.tz;
    // Vector3 pos1 = new Vector3(tx +Gamema.num, ty , tz
   // Vector3 pos1 = new Vector3(GameMa.num-4, 6, 0);
 //   Debug.Log(GameMa.num);

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        sum++;
       fun = sttk.fun;
        pos1 = new Vector3(GameMa.num*1.8f - 4, 6, 0);
        if (fun == 1) DestroyRole();
        flag = false;
        if (q==sum)
        {
            Instantiate(gameObject, new Vector3(GameMa.num * 1.8f - 4, 6, 0), this.transform.rotation);
            DestroyRole();
        }
    }
    //fly���������λ�ô���һ��������Ҫ����
    //����������������ķ���

    private void FlyAnimation(Vector3 pos1)
    {
        StartCoroutine(DoFly(pos1));

    }
    //����fly����

    private IEnumerator DoFly(Vector3 pos1)
    {
        Vector3 direction = (pos1 - transform.position).normalized;
        while(Vector3.Distance(pos1,transform.position)>0.75f)
        {
            yield return new WaitForSeconds(0.05f);
            transform.Translate(direction);
        }
    }
    //��ɫѡ���ɵ���ɫ��

    private void OnMouseDown()
    {
        FlyAnimation(pos1);
      //  Debug.Log(GameMa.num);
        GameMa.num += 1;
        q = sum+100;
       // Instantiate(gameObject, new Vector3(GameMa.num * 1.8f - 4, 6, 0), this.transform.rotation);
      //  DestroyRole();
    }
    //���õ�collide2D������󴥷�����Ч������ʧЧ��

    private void DestroyRole()
    {
        Destroy(gameObject);
    }
    //�ݻ�����ĺ���

}
