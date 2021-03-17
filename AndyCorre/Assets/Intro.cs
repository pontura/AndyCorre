using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public Text field;
    public Animation anim;
    public GameObject panel;
    public string[] texts;
    int id;

    void Start()
    {
        panel.SetActive(false);
        Events.OnKeyPressed += OnKeyPressed;
        Invoke("NextText", 2);
    }
    public void Init()
    {
        panel.SetActive(true);
    }
    void OnKeyPressed(string key)
    {
        NextText();
    }

    public void NextText()
    {
        CancelInvoke();
        if (id >= texts.Length)
            End();
        else
            field.text = texts[id];
        id++;
        SetCosas();
        Invoke("NextText", 5);
    }
    void SetCosas()
    {
        if(id == 4)
        {
            anim.Play("introAbre");
        }
    }
    public void End()
    {
        Events.ChangeGameState(Game.states.RUNNING_SIGNALS);
    }
}
