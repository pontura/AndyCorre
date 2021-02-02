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

    public states state;
    public enum states
    {
        IDLE,
        RUN
    }

    void Start()
    {
        maxSpeed = Data.Instance.settings.maxSpeed;
        desaceleration = Data.Instance.settings.desaceleration;
        anim = GetComponent<Animator>();
    }

    public void UpdateSpeed(float _speed)
    {
        speed = _speed * 10;
        if (speed > maxSpeed)
            speed = maxSpeed;
    }
    private void Update()
    {
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
    public void RotateHead(Vector2 orientation)
    {
        Vector3 rot = cam.transform.localEulerAngles;
        rot.y = orientation.x;
        rot.x = orientation.y;
        if (rot.x > 27) rot.x = 27;
        else if (rot.x < -27) rot.x = -27;
        cam.transform.localEulerAngles = rot;

    }
}
