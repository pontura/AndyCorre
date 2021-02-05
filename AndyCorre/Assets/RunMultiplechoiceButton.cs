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
    float speed = 2;
    bool isOver;
    bool isOff;
    bool isDone;
    Color bgColor;
    Color fieldColor;
    float alphaValue;

    public void Init(RunSignal runSignal, Settings.SignalDataMultipleContent content)
    {
        bgColor = bg.color;
        fieldColor = Color.white;
        speed = Data.Instance.settings.multiplechoiceSpeed;
        this.runSignal = runSignal;
        this.content = content;
        field.text = content.text;

        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }
    public void RollOut()
    {
        isOff = false;
        bgColor.a = 1;
        bg.color = bgColor;
        field.color = Color.white;
    }
    public void SetOff()
    {
        isOff = true;
    }
    void UpdateAlpha()
    {
        if (isOff)
        {
            alphaValue -= speed * Time.deltaTime;
        }
        else
            alphaValue += speed * Time.deltaTime / 2;
        if (alphaValue < 0)
            alphaValue = 0;
        else if (alphaValue > 1)
            alphaValue = 1;

        bgColor.a = alphaValue;
        fieldColor.a = alphaValue;

        bg.color = bgColor;
        field.color = fieldColor;
    }
    private void Update()
    {
        if (isDone)
            return;

        UpdateAlpha();

        if (isOver)
        {            
            value += speed * Time.deltaTime;
            if (value > 1)
            {
                value = 1;
                Clicked();
            }
            SetBar();
        }
    }
    void SetBar()
    {
        bar.fillAmount = value;
    }
    public void PointerEnter()
    {
        runSignal.OnOverMultiplechoice(this);
        alphaValue = 1;
        isOff = false;
        isOver = true;
        value = 0;
        SetBar();
    }
    public void Pointerxit()
    {
        isOff = false;
        runSignal.OnOverMultiplechoice(null);
        isOver = false;
        value = 0;
        SetBar();
    }
    public void Clicked()
    {
        isDone = true;
        runSignal.Clicked(content);
    }
}
