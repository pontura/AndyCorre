using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableButton : MonoBehaviour
{
    Animation anim;
    private void Awake()
    {
        anim = GetComponent<Animation>();
    }
    public void OnActive()
    {
        anim.Play();
        print("si");
    }
}
