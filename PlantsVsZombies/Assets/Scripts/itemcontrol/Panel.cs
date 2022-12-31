using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    // Start is called before the first frame update
    int fun = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fun = sttk.fun;
        if (fun == 1) Destroy(gameObject);
    }
}
