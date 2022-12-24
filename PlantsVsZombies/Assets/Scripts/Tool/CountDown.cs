using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CountDown
{
    private int miliseconds;
    private bool available;
    public bool Available => available;
    public CountDown(int milisecondsCountDown)
    {
        miliseconds = milisecondsCountDown;
        available = true;
    }
    private Task task;
    public void StartCountDown()
    {
        if(task == null)
        {
            task = Task.Run(() =>
            {
                available = false;
                Thread.Sleep(miliseconds);
                available = true;
                task = null;
            });
        }
    }
}
