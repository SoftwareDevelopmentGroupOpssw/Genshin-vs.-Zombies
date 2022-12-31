using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sttk : MonoBehaviour
{
    // Start is called before the first frame update
    public static int fun = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        fun = 1;
        Destroystt();
    }
    //如果按钮被触发，那么销毁

    private void Destroystt()
    {
        Destroy(gameObject);
    }
}
