using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerajp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ganyu,obj;
    bool flag = true;
    int tot = 0,f=-300;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tot++;
        if (tot<340) this.transform.position += this.transform.right * 5 * Time.deltaTime;
        //开局画面向右平移


        Vector3 pos = obj.transform.position;
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;
        Vector3 pos1= new Vector3(x-7,y+3, z);
        x -= 7;y += 3;
        if (tot==400)
        {
           // Instantiate(ganyu, pos1, this.transform.rotation);
            for (int i=0;i<12;i+=2)
            {
                for (int j=0;j< 12;j+=3)
                {
                    Instantiate(ganyu, new Vector3(x+i,y-j,z), this.transform.rotation);
                }
            }
        }
        //生成角色选择栏
   
        if (Input.GetMouseButtonDown(0)&&flag&&tot>400)
        {
            Instantiate(obj, pos, this.transform.rotation);
            f = tot;
            flag = false;
        }
        if (tot<f+330)
        { 
            this.transform.position -= this.transform.right * 5 * Time.deltaTime;
        }
        //选择完毕后屏幕向左平移
    }
}
