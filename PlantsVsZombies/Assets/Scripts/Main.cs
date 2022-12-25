using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Run();
    }
    public void Run()
    {
        UIManager.Instance.ShowPanel<TitlePanel>("TitlePanel");
        //UIManager.Instance.ShowPanel<LevelsPanel>("LevelsPanel");


    }
    // Update is called once per frame
    void Update()
    {

    }
}
