using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class role : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool flag = false;
    public int fun = 0;
    Vector3 pos1;

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
       fun = sttk.fun;
        pos1 = new Vector3(GameMa.num*1.5f - 4, 6, 0);
        if (fun == 1) DestroyRole();
        flag = false;
    }

    private void FlyAnimation(Vector3 pos1)
    {
        StartCoroutine(DoFly(pos1));

    }
    //调用fly函数

    private IEnumerator DoFly(Vector3 pos1)
    {
        Vector3 direction = (pos1 - transform.position).normalized;
        while(Vector3.Distance(pos1,transform.position)>0.5f)
        {
            yield return new WaitForSeconds(0.00001f);
            transform.Translate(direction);
        }
    }
    //角色选择后飞到角色栏

    private void OnMouseDown()
    {
        FlyAnimation(pos1);
      //  Debug.Log(GameMa.num);
        flag = true;
        GameMa.num += 1;
       // DestroyRole();
    }
    //利用的collide2D，点击后触发飞行效果和消失效果

    private void DestroyRole()
    {
        Destroy(gameObject);
    }
}
