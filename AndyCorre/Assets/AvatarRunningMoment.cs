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
        Vector3 pos = Game.Instance.Character.transform.position;
        pos.z += 45;
        npc.transform.position = pos;
        npc.gameObject.SetActive(true);
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
        npc.animationController.SetSpeed(speed);
        if (npc.state == NpcRunner.states.IDLE)
        {
            npc.animationController.SetSpeed(speed);
            if (npc.transform.position.z < _distance + distanceToRunInit)
                InitRun();
            if (speed > 0)
                speed -= Time.deltaTime;
        }
        else
        {
            if (speed < speedMax)
                speed += Time.deltaTime;
            if (_x > max_x)
                _x -= Time.deltaTime/2;
           
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
