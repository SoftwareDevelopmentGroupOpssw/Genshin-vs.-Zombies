using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Run();
    }
    Monster monster;
    public void Run()
    {
        //UIManager.Instance.ShowPanel<TitlePanel>("TitlePanel");
        //UIManager.Instance.ShowPanel<LevelsPanel>("LevelsPanel")

        //GameController.Instance.StartGame();
        GameObject test = new GameObject("Monster");
        monster = test.AddComponent<TestMonster>();
        monster.Data = new TestMonsterData();
        monster.Data.ReceiveDamage(new SystemDamage(0, Elements.Ice, true));
        monster.Data.ReceiveDamage(new SystemDamage(0, Elements.Water, true));
        Task.Run(() =>
        {
            Thread.Sleep(1000);
            monster.Data.ReceiveDamage(new SystemDamage(10, Elements.None, false));
        });
    }
    // Update is called once per frame
    void Update()
    {
        monster.Data.Action();
        Debug.Log(monster.Data.ToString());
    }
}
