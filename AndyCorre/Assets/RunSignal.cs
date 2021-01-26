using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunSignal : MonoBehaviour
{
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

    public enum states
    {
        IDLE,
        DONE
    }

    public void Init(RunSignalsManager manager, Settings.SignalData data)
    {
        canvas.worldCamera = Game.Instance.Character.cam;
        this.data = data;
        if (data.multiplechoice.Length > 0)
        {
            foreach(Settings.SignalDataMultipleContent d in data.multiplechoice)
            {
                RunMultiplechoiceButton button = Instantiate(mButton);
                button.transform.SetParent(view_multiplechoice.transform);
                button.Init(this, d);
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

    public void SetOff()
    {
        //print("SET OFF");
        if (state == states.DONE)
            return;
        state = states.DONE;
        manager.RemoveSignal(this);
    }
    public void OnUpdate()
    {
        Vector3 pos = transform.position;
       
        if (state == states.DONE)
        {
            if (pos.z + settings.distance_to_remove < target.transform.position.z)
            {
                Destroy(this.gameObject);
            }
            return;
        }
            
        transform.LookAt(target);
        float cam_rotation = Mathf.Abs(target.transform.rotation.y);
       
        pos.z = Mathf.Lerp(pos.z, target.transform.position.z+ settings.distance_z, Time.deltaTime * (cam_rotation/ settings.rotationFreeze));
        pos.y = target.transform.position.y + data.pos.y - 2;
        transform.position = pos;
    }

    public void Clicked(Settings.SignalDataMultipleContent content)
    {
        manager.MultiplechoiceSelected(content);
    }
}
