using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public RunUIManager runUIManager;

    void Start()
    {
        Events.OnKeyPressed += OnKeyPressed;
    }

    void OnKeyPressed(string key)
    {
        runUIManager.OnKeyPressed(key);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
