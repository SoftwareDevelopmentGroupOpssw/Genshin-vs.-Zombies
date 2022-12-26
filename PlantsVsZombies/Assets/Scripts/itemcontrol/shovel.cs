using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shovel : MonoBehaviour
{
    public bool gamestart = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gamestart = camerajp.gamestart;
        if (gamestart) Destroy(this.gameObject);
    }
}
