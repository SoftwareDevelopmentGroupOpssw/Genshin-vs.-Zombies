using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button1 : MonoBehaviour
{
    // Start is called before the first frame update
    public void See()
    {
        SceneManager.LoadScene("GameScene");
    }
}
