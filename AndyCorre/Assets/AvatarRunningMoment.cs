using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRunningMoment : MonoBehaviour
{
    public NpcRunner npc;
    public float speed;
    public float speedMax = 10;
    float distanceToRunInit = 10;
    float distanceToRun = 4;
    float distance;
    float _x = 0;
    float max_x = -2;
    private void Awake()
    {
        npc.gameObject.SetActive(false);
    }
    public void Init()
    {
        npc.gameObject.transform.localEulerAngles = new Vector3(0, 172, 0);
        Invoke("DelayedMusic", 3.5f);

        Vector3 pos = Game.Instance.Character.transform.position;
        pos.z += 45;
        npc.transform.position = pos;
        npc.gameObject.SetActive(true);
    }
    void DelayedMusic()
    {
        Events.PlaySound("music", "bego", false);
        Events.FadeVolumeFromTo("music",0, 1, 0.5f);
    }
    public void OnUpdate(float _distance)
    {
        if (Game.Instance.state == Game.states.READY)
        {
            Vector3 pos = npc.transform.localPosition;
            pos.z += Time.deltaTime * 4;
            npc.transform.localPosition = pos;
            return;
        }
        if (npc.state == NpcRunner.states.EXIT)
        {
            Events.ChangeGameState(Game.states.READY);
            Events.RunningState(false);
            return;
        }
        if (npc.state == NpcRunner.states.IDLE)
        {
            npc.animationController.SetSpeed(0);
            if (npc.transform.position.z < _distance + distanceToRunInit)
                InitRun();
            //if (speed > 0)
            //    speed -= Time.deltaTime;
        }
        else
        {
            Vector3 rot = npc.gameObject.transform.localEulerAngles;
            rot.y = Mathf.Lerp(rot.y, 0, 0.05f);
            npc.gameObject.transform.localEulerAngles = rot;


            if (_x > max_x)
                _x -= Time.deltaTime/2;

                if (speed < speedMax)
                    speed += Time.deltaTime;
            print("Game.Instance.Character.speed " + Game.Instance.Character.speed);

            npc.animationController.SetSpeed(Game.Instance.Character.speed);


            if (distance < distanceToRun)
                distance = distanceToRun;
            else
                distance -= Time.deltaTime;

            npc.transform.position = Vector3.Lerp(npc.transform.position, new Vector3(_x, 0, _distance + distance), Time.deltaTime*4);
        }
        if (npc.ActualSignal != null)
            npc.simpleNPCDialogueSignal.OnUpdate();
    }
    void InitRun()
    {
        distance = distanceToRunInit;
        npc.animationController.InitRunning();
        Invoke("NpcStartTalking", 2);
        npc.Run();
    }
    void NpcStartTalking()
    {   
        npc.Talk();            
    }
}
