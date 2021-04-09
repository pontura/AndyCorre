using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public Animation cam_anim;
    public Text field;
    public Animation anim;
    public GameObject panel;
    public string[] texts;
    int id;
    float timeStart;
    void Start()
    {
        Events.OnKeyPressed += OnKeyPressed;
    }
    void OnDestroy()
    {
        Events.OnKeyPressed -= OnKeyPressed;
    }
    public void Init()
    {
        Events.PlaySound("park", "park", true);
        Events.FadeVolumeFromTo("park", 0, 0.7f, 30);
        Invoke("NextText", 2);
        cam_anim.Stop();
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
