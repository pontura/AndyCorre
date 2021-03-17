using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public RunUIManager runUIManager;
    public GameObject endProtoPanel;
    public Intro intro;

    void Start()
    {
        runUIManager.gameObject.SetActive(false);
        endProtoPanel.SetActive(false);
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
    public void EndProto()
    {
        endProtoPanel.SetActive(true);
    }
}
