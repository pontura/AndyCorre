using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunUIManager : MonoBehaviour
{
    public GameObject arrow;
    public ActivableButton space;
    public ActivableButton buttonZ;
    public ActivableButton buttonX;
    float rythmValue;
    float lastKeyPressedTime;
    float deltaTime;

    float realRythm = 0.5f;
    float rythmAcceleration = 0.05f;
    float initialRythm = 0.8f;
    public float rythm;

    float myRythm;
    public float variation = 0.025f;
    public float desaceleration = 0.002f;
    float myDesaceleration;
    public Text field;
    public Image rythmBarImage;
    CharacterRunningManager character;
    Animation anim;
    public states state;
    public enum states
    {
        IDLE,
        RUNNING,
        STOPPED
    }
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Init()
    {
        print("InitRunningInitRunningInitRunning");
        field.text = "";
        character = Game.Instance.Character;
        anim = GetComponent<Animation>();
        InitRunning();
        Events.RunningState += RunningState;
        Events.RunningState(true);
    }
    private void OnDestroy()
    {
        print("OnDestroy");
        Events.RunningState -= RunningState;
    }
    void RunningState(bool isOn)
    {
        if (Game.Instance.state == Game.states.READY)
            return;
        if (isOn)
            InitRunning();
        else
        {
            barColor = Color.red;
            Events.ChangeVolume("ui", 0f);
            anim.Play("stepRythmBarOff");
            state = states.STOPPED;
        }
    }
    int lasBreathID;
    void SetBreath(int id)
    {
        if (id == lasBreathID)
            return;
        lasBreathID = id;
        Events.PlaySound("breath", "breath" + id, true);
    }
    void InitRunning()
    {
        if (Game.Instance.state == Game.states.READY)
            return;
        anim.Play("stepRythmBarOn");
        rythm = initialRythm;
        state = states.RUNNING;
    }
    public void OnKeyPressed(string key)
    {
        if (Game.Instance.state == Game.states.READY)
            return;
        if (state == states.STOPPED)
            return;

        rythm -= rythmAcceleration/2;
        if (rythm < realRythm)
            rythm = realRythm;


        if (lastKeyPressedTime == 0)
        {
            lastKeyPressedTime = Time.time - rythm;
        }

            float newDeltaTime = Time.time - lastKeyPressedTime;
          
            float diff = Mathf.Abs(rythm - newDeltaTime);
            if (diff < 0.25f)
            {
                myRythm = Mathf.Lerp(myRythm, rythm, 0.5f);
                barColor = Color.green;
                SetBreath(1);
            }
            else
            {
                if(rythm> newDeltaTime)
                    SetBreath(2);
                barColor = Color.red;
                if (newDeltaTime > rythm)
                    myRythm -= variation;
                else myRythm += variation;
            }

            myDesaceleration = 0;

            if (myRythm < 0) myRythm = 0;
            else if (myRythm > 1) myRythm = 1;
            
            deltaTime = newDeltaTime;
            
        
        lastKeyPressedTime = Time.time;
        switch (key)
        {
            case "space":
                isLeftStep = !isLeftStep;
                if(isLeftStep)
                    Events.PlaySound("steps", "step1", false);
                else
                    Events.PlaySound("steps", "step2", false);
                break;
                break;
            case "Z": buttonZ.OnActive(); Events.PlaySound("steps", "step1", false); break;
            case "X": buttonX.OnActive(); Events.PlaySound("steps", "step2", false); break;
        }
    }
    bool isLeftStep;

    float rot_z;
    void Update()
    {
        if (Game.Instance.state == Game.states.INTRO)
            return;

        myDesaceleration += desaceleration;
        myRythm -= myDesaceleration * Time.deltaTime;
        if (myRythm < 0)
            myRythm = 0;
        character.UpdateSpeed(myRythm);

        if (state == states.RUNNING)
            UpdateBar();

        Vector3 rot = arrow.transform.localEulerAngles;
        if (rot_z != 0)
            rot_z = Mathf.Lerp(rot_z, myRythm * 180, 0.1f);
        else
            rot_z = 0.25f;
        rot.z = rot_z;
        arrow.transform.localEulerAngles = -rot;


        if (character.speed > 9.2f && state == states.RUNNING)
        {
            Events.RunningState(false);

        }
        else if (state == states.RUNNING)
        {
            
            if (character.speed >= 0.25f)
            {
                if (!isRunning)
                    Events.ChangeVolume("ui", 0.5f);
                isRunning = true;
            }
            else if (isRunning)
            {
                isRunning = false;
                Events.ChangeVolume("ui", 0.7f);
            }
        }
    }


    float timer;
    float barAlpha = 0;    
    Color barColor;
    bool isRunning;
    void UpdateBar()
    {
        timer += Time.deltaTime;
        if (timer > rythm)
        {
            if (character.speed < 4f)
            {
                rythm += rythmAcceleration;
                if (rythm > initialRythm)
                    rythm = initialRythm;
            }

            Events.PlaySound("ui", "kick", false);

            if (character.speed >= 0.25f)
                anim.Play("stepRythmBar");  
           
                
            timer = 0;
            barAlpha = 1;
            
        }
        barAlpha -= Time.deltaTime * 2;
        if (barAlpha < 0)
            barAlpha = 0;
        barColor.a = barAlpha;
        rythmBarImage.color = barColor;
    }
}
