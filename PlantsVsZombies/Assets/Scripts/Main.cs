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


        //GameController.Instance.StartGame();

        //GameObject test = new GameObject("Monster");
        //monster = test.AddComponent<TestMonster>();
        //monster.Data = new TestMonsterData();

        //Debug.Log("Add electic");
        //monster.Data.ReceiveDamage(new SystemDamage(0, Elements.Electric, true));

        //Debug.Log("Add grass");
        //monster.Data.ReceiveDamage(new SystemDamage(0, Elements.Grass, true));


    }
    // Update is called once per frame
    void Update()
    {

    }
}
