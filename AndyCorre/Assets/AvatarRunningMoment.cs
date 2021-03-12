using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRunningMoment : MonoBehaviour
{
    public NpcRunner npc;
    public float speed;
    public float speedMax = 10;
    float distanceToRun = 4;
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

    public void OnUpdate(float distance)
    {
        if (npc.state == NpcRunner.states.EXIT)
        {
            Events.RunningState(false);
            Events.ChangeGameState(Game.states.READY);
            npc.transform.position = new Vector3(_x, 0, npc.transform.position.z+Time.deltaTime * 4);
            return;
        }
        npc.animationController.SetSpeed(speed);
        if (npc.state == NpcRunner.states.IDLE)
        {
            npc.animationController.SetSpeed(speed);
            if (npc.transform.position.z < distance + distanceToRun)
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
            
            npc.transform.position = Vector3.Lerp(npc.transform.position, new Vector3(_x, 0, distance + distanceToRun), Time.deltaTime*4);
        }
        if (npc.ActualSignal != null)
            npc.simpleNPCDialogueSignal.OnUpdate();
    }
    void InitRun()
    {
        npc.animationController.InitRunning();
        Invoke("NpcStartTalking", 2);
        npc.Run();
    }
    void NpcStartTalking()
    {   
        npc.Talk();            
    }
}
