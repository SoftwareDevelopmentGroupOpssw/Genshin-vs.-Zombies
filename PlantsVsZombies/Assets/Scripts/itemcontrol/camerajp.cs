using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerajp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ganyu,obj,column,panel,lisha,ying,ningguang,sanbing,kelai,yanfei,naxida,mona,zhongli,stt;
    public int fun = 0;
  //  public GameObject a[30];
    bool flag = true;
    public static bool gamestart = false;
    public static float x = 4;
    int tot = 0,f=-20000;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tot++;
        if (tot<800&&tot>=500) this.transform.position += this.transform.right * 5 * Time.deltaTime;
        //开局画面向右平移


        Vector3 pos = obj.transform.position;
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;
        Vector3 pos1= new Vector3(x-7,y+3, z);
        x -= 6;y += 4;
     
        
        if (tot==1100)
        {
            // Instantiate(ganyu, pos1, this.transform.rotation);

            /*for (int i=-2;i<12;i+=2)
            {
                for (int j=0;j< 12;j+=3)
                {
                    Instantiate(ganyu, new Vector3(x+i,y-j,z), this.transform.rotation);
                }
            }*/
            //这里本来是循环生成角色的，后改成单独


            Instantiate(ganyu, new Vector3(x, y , z), this.transform.rotation);
            Instantiate(lisha, new Vector3(x+2, y , z), this.transform.rotation);
            Instantiate(ying, new Vector3(x +4, y , z), this.transform.rotation);
            Instantiate(ningguang, new Vector3(x+6 , y , z), this.transform.rotation);
            Instantiate(sanbing, new Vector3(x+8 , y , z), this.transform.rotation);
            Instantiate(yanfei, new Vector3(x + 0, y-3, z), this.transform.rotation);
            Instantiate(naxida, new Vector3(x + 2, y-3, z), this.transform.rotation);
            Instantiate(mona, new Vector3(x + 4, y-3, z), this.transform.rotation);
            Instantiate(kelai, new Vector3(x + 6, y-3, z), this.transform.rotation);
            Instantiate(zhongli, new Vector3(x + 8, y - 3, z), this.transform.rotation);

            Instantiate(stt, new Vector3(x + 13, y - 2, z), this.transform.rotation);
            //生成开始按钮



            Instantiate(column, new Vector3(x +4, y +3, z), this.transform.rotation);
        }
        //生成角色选择栏

        fun = sttk.fun;

        if (fun==1&&flag&&tot>1100)
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
        //选择完毕后屏幕向左平移
        if (tot==f+500)
        {
            Instantiate(panel, new Vector3(x-2,y+4,z), this.transform.rotation);
        }
        //生成物品栏
    }
}
