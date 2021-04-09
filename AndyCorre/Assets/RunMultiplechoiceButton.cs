using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunMultiplechoiceButton : MonoBehaviour
{
    public Text field;
    RunSignal runSignal;
    Settings.SignalDataMultipleContent content;
    public Image bar;
    public Image bg;
    float value;
    float speed = 1.2f;
    bool isDone;
    Color bgColor;
    Color fieldColor;
    float alphaValue;
    float initialValue = 0.015f;
    SimpleNPCDialogueSignal npcSignal;


    public void InitNpc(SimpleNPCDialogueSignal npcSignal, Settings.SignalDataMultipleContent content, Color fieldColor)
    {
        Color barColor = fieldColor;
        barColor.a = 0.3f;
        bar.color = barColor;

        bgColor = bg.color;
        this.fieldColor = fieldColor;
        speed = Data.Instance.settings.multiplechoiceSpeed;
        this.npcSignal = npcSignal;
        this.content = content;
        field.text = content.text;

        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        value = initialValue;

        alphaValue = 1;
        bgColor.a = 1;
        bg.color = bgColor;
        field.color = Color.white;
    }
    public void Init(RunSignal runSignal, Settings.SignalDataMultipleContent content, Color fieldColor)
    {
        Color barColor = fieldColor;
        barColor.a = 0.2f;
        bar.color = barColor;

        bgColor = bg.color;
        this.fieldColor = fieldColor;
        speed = Data.Instance.settings.multiplechoiceSpeed;
        this.runSignal = runSignal;
        this.content = content;
        field.text = content.text;

        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        value = initialValue;

        alphaValue = 1;
        bgColor.a = 1;
        bg.color = bgColor;
        field.color = Color.white;
    }
    public void Clicked()
    {
        if (isDone)
            return;
        isDone = true;
        if (npcSignal != null)
            npcSignal.Clicked(content);
        else
            runSignal.Clicked(content);
    }
}
