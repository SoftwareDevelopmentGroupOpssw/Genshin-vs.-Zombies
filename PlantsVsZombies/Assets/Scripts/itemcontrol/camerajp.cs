using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerajp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ganyu,obj,column,panel;
    bool flag = true;
    public static bool gamestart = false;
    int tot = 0,f=-20000;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tot++;
        if (tot<800&&tot>=500) this.transform.position += this.transform.right * 5 * Time.deltaTime;
        //���ֻ�������ƽ��


        Vector3 pos = obj.transform.position;
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;
        Vector3 pos1= new Vector3(x-7,y+3, z);
        x -= 6;y += 4;
        if (tot==1100)
        {
           // Instantiate(ganyu, pos1, this.transform.rotation);
            for (int i=-2;i<12;i+=2)
            {
                for (int j=0;j< 12;j+=3)
                {
                    Instantiate(ganyu, new Vector3(x+i,y-j,z), this.transform.rotation);
                }
            }
            Instantiate(column, new Vector3(x +4, y +3, z), this.transform.rotation);
        }
        //���ɽ�ɫѡ����
   
        if (Input.GetMouseButtonDown(0)&&flag&&tot>1100)
        {
          //  Instantiate(obj, pos, this.transform.rotation);
            f = tot;
            gamestart = true;
            flag = false;
        }
        if (tot<f+360)
        { 
            this.transform.position -= this.transform.right * 5 * Time.deltaTime;
        }
        //ѡ����Ϻ���Ļ����ƽ��
        if (tot==f+500)
        {
            Instantiate(panel, new Vector3(x-2,y+4,z), this.transform.rotation);
        }
    }
}
