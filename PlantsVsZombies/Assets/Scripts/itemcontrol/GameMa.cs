using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMa : MonoBehaviour
{
    // Start is called before the first frame update
    int tot = 0;

    public GameObject ganyu, obj, column, panel, lisha, ying, ningguang, sanbing, kelai, yanfei, naxida, mona, zhongli, stt;
    public int fun = 0;

    bool flag = true;

    public static bool gamestart = false;
    public static float tx = -4, ty = 6, tz = 0,num=0;
    public static float x = 4;

    int  f = -20000;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         tot++;
         if (tot<900&&tot>=500) Camera.main.transform.position += Camera.main.transform.right * 5 * Time.deltaTime;


        Vector3 pos = obj.transform.position;
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;
        Vector3 pos1 = new Vector3(x - 7, y + 3, z);
        x += 10;y -= 5;
        //��ʼ�����ȷ��

        if (tot == 1100)
        {
            Instantiate(ganyu, new Vector3(x , y, z), this.transform.rotation);
            Instantiate(lisha, new Vector3(x + 2, y, z), this.transform.rotation);
            Instantiate(ying, new Vector3(x + 4, y, z), this.transform.rotation);
            Instantiate(ningguang, new Vector3(x + 6, y, z), this.transform.rotation);
            Instantiate(sanbing, new Vector3(x + 8, y, z), this.transform.rotation);
            Instantiate(yanfei, new Vector3(x + 0, y - 3, z), this.transform.rotation);
            Instantiate(naxida, new Vector3(x + 2, y - 3, z), this.transform.rotation);
            Instantiate(mona, new Vector3(x + 4, y - 3, z), this.transform.rotation);
            Instantiate(kelai, new Vector3(x + 6, y - 3, z), this.transform.rotation);
            Instantiate(zhongli, new Vector3(x + 8, y - 3, z), this.transform.rotation);
            //���ɽ�ɫͷ��

            Instantiate(stt, new Vector3(x + 13, y , z), this.transform.rotation);
            //���ɿ�ʼ��ť


            Instantiate(column, new Vector3(x + 4, y + 3, z), this.transform.rotation);
            //����ѡ����

        }

        fun = sttk.fun;
        //���Σ�����������Ϸ�Ŀ�ʼ���

        if (fun == 1 && flag && tot > 1100)
        {
            //  Instantiate(obj, pos, this.transform.rotation);
            f = tot;
            gamestart = true;
            flag = false;
            //����������������ô��Ϸ��ʼ
        }

        if (tot < f + 360)
        {
            Camera.main.transform.position -= Camera.main.transform.right * 5 * Time.deltaTime;
        }
        //ѡ����Ϻ���Ļ����ƽ��

        if (tot == f + 500)
        {
            Instantiate(panel, new Vector3(x - 2, y + 4, z), Camera.main.transform.rotation);
        }
        //������Ʒ��
    }
}
