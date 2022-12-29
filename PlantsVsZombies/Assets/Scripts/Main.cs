using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        Run();
    }
    Monster monster;
    public void Run()
    {
        UIManager.Instance.ShowPanel<TitlePanel>("TitlePanel");

    }
    // Update is called once per frame
    void Update()
    {

    }
}
