using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject arrow;
    public bool gamestart;
    public GameObject MainCamera;
    //GameObject arrowPrefab = Resources.Load(“Prefabs / arrowPrefab”);
    int tot = 0;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        tot++;
        //  gamestart = MainCamera.GetComponent<camerajp>().gamestart;
        gamestart = camerajp.gamestart;
        if (tot % 500 == 0&&gamestart)
        {
           Instantiate(arrow, this.transform.position, this.transform.rotation);
        }
        //每隔一段时间射出一支箭
        //起点是自身的坐标

        //Destroy(this.gameObject);
        if (gamestart)
        {
            Destroy(this.gameObject);
        }
        //摧毁自己
    }
}
