using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public ActivableButton buttonZ;
    public ActivableButton buttonX;

    void Start()
    {
        Events.OnKeyPressed += OnKeyPressed;
    }

    void OnKeyPressed(string key)
    {
        switch(key)
        {
            case "Z": buttonZ.OnActive();  break;
            case "X": buttonX.OnActive(); break;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
