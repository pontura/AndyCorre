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
    Settings.SignalData data;
    RunSignalsManager manager;
    public states state;
    public enum states
    {
        IDLE,
        DONE
    }

    public void Init(RunSignalsManager manager, Settings.SignalData data)
    {
        settings = Data.Instance.settings.runSignalSettings;
        this.data = data;
        this.manager = manager;
        target = Game.Instance.Character.cam.transform;
        field.text = data.text;
        if (data.sprite == null)
            image.enabled = false;
        else
            image.sprite = data.sprite;
    }
    void Update()
    {
        Vector3 pos = transform.position;
       
        if (state == states.DONE)
        {
            if (pos.z + settings.distance_to_remove < target.transform.position.z)
                Destroy(this.gameObject);
            return;
        }
            
        transform.LookAt(target);
        float cam_rotation = Mathf.Abs(target.transform.rotation.y);
       
        pos.z = Mathf.Lerp(pos.z, target.transform.position.z+ settings.distance_z, Time.deltaTime * (cam_rotation/ settings.rotationFreeze));
        pos.y = target.transform.position.y + data.pos.y - 2;
        transform.position = pos;

        if (pos.z + settings.distance_to_ignore < target.transform.position.z)
        {
            state = states.DONE;
            manager.RemoveSignal(this);
        }
    }
}
