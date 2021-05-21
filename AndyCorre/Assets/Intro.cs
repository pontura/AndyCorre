using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    bool canClick;
    public Animation cam_anim;
    public Text field;
    public Animation anim;
    public GameObject panel;
    public GameObject placa;

    public string[] texts;
    int id;
    float timeStart;

    void Start()
    {
        canClick = false;
        panel.SetActive(true);
        anim.Play("intro");
        Invoke("Delayed", 7);
        Events.ChangeCursor(CursorUI.types.NONE, Color.black);
    }
    void Delayed()
    {
        Events.ChangeCursor(CursorUI.types.SIMPLE, Color.black);
        Events.OnKeyPressed += OnKeyPressed;
        canClick = true;
    }
    void OnDestroy()
    {
        Events.OnKeyPressed -= OnKeyPressed;
    }
    public void Init()
    {
        Events.PlaySound("park", "park", true);
        Events.FadeVolumeFromTo("park", 0, 0.7f, 30);
        placa.SetActive(true);
        cam_anim.Stop();
        panel.SetActive(true);
    }
    public void StartTexts()
    {
        placa.SetActive(false);
        Invoke("NextText", 2);
    }
    void OnKeyPressed(string key)
    {
        NextText();
    }
    public void NextText()
    {
        if (!canClick) return;
        CancelInvoke();
        if (id >= texts.Length)
        {
            End();
            return;
        }
        else
            field.text = texts[id];
        id++;
        SetCosas();
        Invoke("NextText", 4);
    }
    void SetCosas()
    {
        if(id == 4)
        {
            timeStart = Time.time;
            anim.Play("introAbre");
            cam_anim.Play();
        }
    }
    bool ended;
    public void End()
    {
        if (ended) return;
        ended = true;
        CancelInvoke();        
        field.text = "";
        GotoEnd();
    }
    void GotoEnd()
    {
        panel.SetActive(false);
        Events.ChangeGameState(Game.states.RUNNING_SIGNALS);
    }
}
