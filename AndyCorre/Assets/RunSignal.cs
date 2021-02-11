using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunSignal : MonoBehaviour
{
    Transform container;
    Transform target;
    public Text field;
    public Image image;
    Settings.RunSignalSettings settings;
    public Settings.SignalData data;
    RunSignalsManager manager;
    public states state;
    public GameObject view_multiplechoice;
    public RunMultiplechoiceButton mButton;
    public Canvas canvas;
    List<RunMultiplechoiceButton> multiplechoiceAll;
    Color fieldColor;
    public Image bar;
    public GameObject barAsset;

    public enum states
    {
        IDLE,
        ON,
        SELECTED,
        DONE            
    }

    public void Init(RunSignalsManager manager, Settings.SignalData data)
    {
        fieldColor = Data.Instance.settings.disparadoresColors[data.disparador_id];
        bar.color = fieldColor;
        container = transform.parent;
        SetCanvasAlpha(0.5f);
        canvas.worldCamera = Game.Instance.Character.cam;
        this.data = data;
        multiplechoiceAll = new List<RunMultiplechoiceButton>();
       
        if (data.multiplechoice != null && data.multiplechoice.Length > 0)
        {
            foreach (Settings.SignalDataMultipleContent d in data.multiplechoice)
            {
                RunMultiplechoiceButton button = Instantiate(mButton);
                button.transform.SetParent(view_multiplechoice.transform);
                button.Init(this, d, fieldColor);
                multiplechoiceAll.Add(button);
            }
            SetBarOff();
        }
        settings = Data.Instance.settings.runSignalSettings;
        
        this.manager = manager;
        target = Game.Instance.Character.cam.transform;
        field.text = data.text;
        field.color = fieldColor;

        if (data.sprite == null)
            image.enabled = false;
        else
            image.sprite = data.sprite;       
    }
    void SetBarOff()
    {
        Vector3 barPos = barAsset.transform.position;
        barPos.y += 1000;
        barAsset.transform.position = barPos;
    }
    public void OnOverMultiplechoice(RunMultiplechoiceButton buttonOver)
    {
        if (state != states.ON)
            return;
        if (buttonOver == null)
        {
            foreach (RunMultiplechoiceButton m in multiplechoiceAll)
                m.RollOut();
        }
        else
        {
            foreach (RunMultiplechoiceButton m in multiplechoiceAll)
                if (m != buttonOver)
                    m.SetOff();
        }
    }
    float barTo;
    public void SetOn(int id, int total)
    {
        StopAllCoroutines();
        state = states.ON;
        SetCanvasAlpha(1);
        bar.fillAmount = 0;

        id++;
        total++;

        bar.fillAmount = (float)(id - 1) / (float)total;
        barTo = (float)id / (float)total;
        if(id > total-1)
            SetBarOff();
    }
    void Update()
    {
        if(state == states.ON && bar.fillAmount <= barTo)
            bar.fillAmount += Time.deltaTime/1;
    }
    void SetCanvasAlpha(float value)
    {
        canvas.GetComponent<CanvasGroup>().alpha = value;
    }
    bool animOff;
    public void SetAnimOff()
    {
        if (animOff) return;
        
        animOff = true;
        canvas.GetComponent<Animation>().Play();
    }
    public void SetOff()
    {
        if (state == states.DONE)
            return;
        transform.SetParent(container);
        state = states.DONE;
        SetAnimOff();
        Invoke("DestroyedDelay", 1);
    }
    void DestroyedDelay()
    {
        Destroy(this.gameObject);
    }
    public void OnUpdate()
    {
        Vector3 pos = transform.position;

        transform.LookAt(target);
        float cam_rotation = Mathf.Abs(target.transform.rotation.y);

        if (state == states.ON)
        {
              pos.z = Mathf.Lerp(pos.z, target.transform.position.z + settings.distance_z, Time.deltaTime * (cam_rotation / settings.rotationFreeze));
        }
        pos.y = target.transform.position.y -1 - (Game.Instance.Character.cam.transform.parent.transform.localPosition.y/50);

        transform.position = pos;
    }

    public void Clicked(Settings.SignalDataMultipleContent content)
    {
        state = states.SELECTED;
        manager.MultiplechoiceSelected(content);
    }
}
