using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public Animator anim;
    actions action;
    enum actions
    {
        idle,
        walk,
        run
    }

    public void InitRunning()
    {
    }
    public void SetSpeed(float speed)
    {
        if (speed > 4.5f)
        {
            if( action != actions.run)
                anim.CrossFade("run", 0.2f);
            action = actions.run;
        }            
        else if (speed > 0.1f)
        {
            if ( action != actions.walk)
                anim.CrossFade("walk", 0.2f);
            action = actions.walk;
        }
        else if(speed <0.1f && action != actions.idle)
        {
            if (action != actions.idle)
                anim.CrossFade("idle", 0.2f);
            action = actions.idle;
        }
    }
}
