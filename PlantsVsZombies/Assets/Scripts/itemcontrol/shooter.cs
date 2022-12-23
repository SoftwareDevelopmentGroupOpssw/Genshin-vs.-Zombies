using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject arrow;
    //GameObject arrowPrefab = Resources.Load(¡°Prefabs / arrowPrefab¡±);
    int tot = 0;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        tot++;
        if (tot%500==0)
        {
            Instantiate(arrow, this.transform.position, this.transform.rotation);
        }
    }
}
