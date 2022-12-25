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
        GameController.Instance.StartGame();
        UIManager.Instance.ShowPanel<SelectPanel>("SelectPanel",UIManager.UILayer.Mid, (panel) => panel.SetPlotCount(8));
        for(int i = 200;i < 800; i+= 100)
        {
            EnergyMonitor.InstantiateEnergy(new Vector2Int(i, 540), EnergyType.Big);
        }
        
    }
    // Update is called once per frame
    void Update()
    {

    }
}
