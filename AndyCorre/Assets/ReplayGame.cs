using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayGame : MonoBehaviour
{
    void Start()
    {
        Invoke("Delayed", 1);
    }
    void Delayed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
