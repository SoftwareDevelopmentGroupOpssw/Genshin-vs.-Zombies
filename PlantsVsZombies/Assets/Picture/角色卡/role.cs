using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class role : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool flag = false;
    public int fun = 0;
    
    Vector3 pos1 = new Vector3(-4,6,0);

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
          fun = sttk.fun;
        if (fun == 1) DestroyRole();
    }

    private void FlyAnimation(Vector3 pos1)
    {
        StartCoroutine(DoFly(pos1));
    }
    //����fly����

    private IEnumerator DoFly(Vector3 pos1)
    {
        Vector3 direction = (pos1 - transform.position).normalized;
        while(Vector3.Distance(pos1,transform.position)>1f)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(direction);
        }
    }
    //��ɫѡ���ɵ���ɫ��

    private void OnMouseDown()
    {
        FlyAnimation(pos1);
        flag = true;
       // DestroyRole();
    }
    //���õ�collide2D������󴥷�����Ч������ʧЧ��

    private void DestroyRole()
    {
        Destroy(gameObject);
    }
}
