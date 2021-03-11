using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public Animator anim;

    public void SetSpeed(float speed)
    {
        anim.SetFloat("speed", speed);
    }
}
