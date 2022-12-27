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

        Debug.Log("Add electic");
        monster.Data.ReceiveDamage(new SystemDamage(0, Elements.Electric, true));

        Debug.Log("Add grass");
        monster.Data.ReceiveDamage(new SystemDamage(0, Elements.Grass, true));

        Task.Run(() =>
        {
            //Debug.Log("task start");
            Thread.Sleep(1000);
            //Debug.Log("SleepComplete");
            ////monster.Data.AddEffect(new StrengthEffect(-50, 2000, SystemObject.Instance));
            //Debug.Log("AddComplete");
            monster.Data.ReceiveDamage(new SystemDamage(10, Elements.Electric, true));
            monster.Data.ReceiveDamage(new SystemDamage(10, Elements.Grass, true));
            Thread.Sleep(8000);
            monster.Data.ReceiveDamage(new SystemDamage(10, Elements.Electric, true));
            monster.Data.ReceiveDamage(new SystemDamage(10, Elements.Grass, true));
            Debug.Log("added");
        });
    }
    // Update is called once per frame
    string str = "";
    void Update()
    {
        monster.Data.Action();
        if(monster.Data.ToString() != str)
        {
            str = monster.Data.ToString();
            Debug.Log(str);
        }
    }
}
