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
    float value;
    float speed = 2;
    bool isOver;
    bool isDone;

    public void Init(RunSignal runSignal, Settings.SignalDataMultipleContent content)
    {
        speed = Data.Instance.settings.multiplechoiceSpeed;
        this.runSignal = runSignal;
        this.content = content;
        field.text = content.text;

        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }
    private void Update()
    {
        if (isDone)
            return;
        if (isOver)
            value += speed * Time.deltaTime;


        if (value > 1)
        {
            value = 1;
            Clicked();
        }
        SetBar();
    }
    void SetBar()
    {
        bar.fillAmount = value;
    }
    public void PointerEnter()
    {
        isOver = true;
        value = 0;
        SetBar();
    }
    public void Pointerxit()
    {
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
