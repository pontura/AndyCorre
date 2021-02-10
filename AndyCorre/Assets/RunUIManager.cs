using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunUIManager : MonoBehaviour
{
    public GameObject arrow;
    public ActivableButton buttonZ;
    public ActivableButton buttonX;
    float rythmValue;
    float lastKeyPressedTime;
    float deltaTime;
    public float rythm = 0.5f;
    float myRythm;
    public float variation = 0.05f;
    public float desaceleration = 0.002f;
    float myDesaceleration;
    public Text field;
    public Image rythmBarImage;
    CharacterRunningManager character;
    Animation anim;

    void Start()
    {
        field.text = "";
        character = Game.Instance.Character;
        anim = GetComponent<Animation>();
    }
    public void OnKeyPressed(string key)
    {        
        if (lastKeyPressedTime != 0)
        {
            float newDeltaTime = Time.time - lastKeyPressedTime;
            if (deltaTime != 0)
            {
                float diff = Mathf.Abs(rythm - newDeltaTime);
                if (diff < 0.25f)
                {
                    myRythm = Mathf.Lerp(myRythm, rythm, 0.5f);
                    barColor = Color.green;
                }
                else
                {
                    barColor = Color.red;
                    if (newDeltaTime > rythm)
                        myRythm -= variation;
                    else myRythm += variation;
                }

                myDesaceleration = 0;

                if (myRythm < 0) myRythm = 0;
                else if (myRythm > 1) myRythm = 1;
            }
            deltaTime = newDeltaTime;
            
        } else
        {
            myDesaceleration = 0;
        }
        lastKeyPressedTime = Time.time;
        switch (key)
        {
            case "Z": buttonZ.OnActive(); Events.PlaySound("steps", "step1", false); break;
            case "X": buttonX.OnActive(); Events.PlaySound("steps", "step2", false); break;
        }

    }

    float rot_z;
    void Update()
    {
        UpdateBar();

        myDesaceleration += desaceleration;
        myRythm -= myDesaceleration * Time.deltaTime;
        if (myRythm < 0)
            myRythm = 0;
        //field.text = myRythm.ToString();
        Vector3 rot = arrow.transform.localEulerAngles;
        if (rot_z != 0)
            rot_z = Mathf.Lerp(rot_z, myRythm * 180, 0.1f);
        else
            rot_z = 0.25f;
        rot.z = rot_z;
        arrow.transform.localEulerAngles = -rot;
        character.UpdateSpeed(myRythm);
    }


    float timer;
    float barAlpha = 0;    
    Color barColor;
    void UpdateBar()
    {
        timer += Time.deltaTime;
        if (timer > rythm)
        {
            anim.Play("stepRythmBar");
            timer = 0;
            barAlpha = 1;
            Events.PlaySound("ui", "kick", false);
        }
        barAlpha -= Time.deltaTime * 2;
        if (barAlpha < 0)
            barAlpha = 0;
        barColor.a = barAlpha;
        rythmBarImage.color = barColor;
    }
}
