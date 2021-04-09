using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRunningManager : MonoBehaviour
{
    float maxSpeed;
    public float speed;
    float desaceleration;
    Animator anim;
    public Camera cam;
    public EffectsManager effectsManager;

    public states state;
    public enum states
    {
        IDLE,
        RUN,
        STOPPED
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }
    public void Init()
    {
        anim.enabled = true;
        maxSpeed = Data.Instance.settings.maxSpeed;
        desaceleration = Data.Instance.settings.desaceleration;      
        
        Events.PlaySound("park", "park", true);
        Events.RunningState += RunningState;
    }
    private void OnDestroy()
    {
        Events.RunningState -= RunningState;
    }
    void RunningState(bool isOn)
    {
        if(isOn)
        {
            state = states.RUN;
        }
        else if(Game.Instance.state != Game.states.READY)
        {
            aberrationValue = 2;
            state = states.STOPPED;
            Invoke("Restart", 5);
        }
    }
    void Restart()
    {
        Events.RunningState(true);
    }
    float aberrationValue;
    public void UpdateSpeed(float _speed)
    {       
        if (state == states.RUN)
        {
            aberrationValue = _speed - 0.5f;
            if (aberrationValue < 0)
                aberrationValue = 0;
            else aberrationValue *= 2;
        } else
        {
            aberrationValue -= 0.05f * Time.deltaTime;
        }
        
        effectsManager.ChangeAberration(aberrationValue);
        speed = _speed * 10;
        if (speed > maxSpeed)
            speed = maxSpeed;
    }
    private void Update()
    {
        if (Game.Instance.state == Game.states.INTRO) return;

        if (speed <= 0.25f)
        {
            anim.SetFloat("speed", 0);
            return;
        }

        anim.SetFloat("speed", speed / maxSpeed);

        speed -= desaceleration * Time.deltaTime;

        Vector3 pos = transform.position;
        pos.z += speed * Time.deltaTime;
        transform.position = pos;
    }
    float timer;
    public void RotateHead(Vector2 orientation)
    {
        timer += Time.deltaTime/60;

        Vector3 rot = cam.transform.localEulerAngles;

        rot.y = orientation.x;
        rot.x = orientation.y;

        if (rot.x > 27) rot.x = 27;
        else if (rot.x < -27) rot.x = -27;
        if (rot.y > 70) rot.y = 70;
        else if (rot.y < -70) rot.y = -70;
        rot.z = 0;

        if (timer < 1)
        {
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(rot), timer);
            Vector3 r = cam.transform.localEulerAngles;
            r.z = 0;
            cam.transform.localEulerAngles = r;
        }
        else
            cam.transform.localEulerAngles = rot;

    }
}
