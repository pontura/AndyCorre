using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRunningManager : MonoBehaviour
{
    public float maxSpeed = 10;
    public float stepSpeed = 2;
    public float speed;
    public float desaceleration = 5;
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
        anim = GetComponent<Animator>();
    }

    public void Step()
    {
        speed += stepSpeed;
        if (speed > maxSpeed)
            speed = maxSpeed;
    }
    private void Update()
    {
        if (speed <= 0)
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
        cam.transform.localEulerAngles = rot;
    }
}
