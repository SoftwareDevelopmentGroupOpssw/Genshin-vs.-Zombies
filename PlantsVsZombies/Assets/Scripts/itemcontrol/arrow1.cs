using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += this.transform.right * 5 * Time.deltaTime;
        //if (this.transform.position.x >= 200) Destroy(this.gameObject);
    }
    //���Լ����ɺ�ÿһ֡�����˶�һ���ľ���
}
