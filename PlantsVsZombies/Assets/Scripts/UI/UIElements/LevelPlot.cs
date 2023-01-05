using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPlot : MonoBehaviour
{
    [SerializeField]
    private Text text;
    public void SetLevelName(string name)
    {
        text.text = name;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
