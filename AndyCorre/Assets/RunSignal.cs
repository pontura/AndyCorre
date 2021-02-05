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

    public enum states
    {
        IDLE,
        ON,
        DONE        
    }

    public void Init(RunSignalsManager manager, Settings.SignalData data)
    {
        container = transform.parent;
        SetCanvasAlpha(0.5f);
        canvas.worldCamera = Game.Instance.Character.cam;
        this.data = data;
        multiplechoiceAll = new List<RunMultiplechoiceButton>();
        if (data.multiplechoice != null && data.multiplechoice.Length > 0)
        {
            foreach(Settings.SignalDataMultipleContent d in data.multiplechoice)
            {
                RunMultiplechoiceButton button = Instantiate(mButton);
                button.transform.SetParent(view_multiplechoice.transform);
                button.Init(this, d);
                multiplechoiceAll.Add(button);
            }                
        }

        settings = Data.Instance.settings.runSignalSettings;
        
        this.manager = manager;
        target = Game.Instance.Character.cam.transform;
        field.text = data.text;
        if (data.sprite == null)
            image.enabled = false;
        else
            image.sprite = data.sprite;       
    }
    public void OnOverMultiplechoice(RunMultiplechoiceButton buttonOver)
    {
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
    public void SetOn()
    {
        state = states.ON;
        SetCanvasAlpha(1);
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
        manager.MultiplechoiceSelected(content);
    }
}
