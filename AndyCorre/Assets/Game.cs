using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    static Game mInstance = null;
    public float distance;
    [SerializeField] CharacterRunningManager character;
    ParkManager parkManager;
    RunSignalsManager runSignalsManager;
    LightsManager lightsManager;
    AvatarRunningMoment avatarRunningMoment;

    public states state;
    public enum states
    {
        INTRO,
        RUNNING_SIGNALS,
        AVATAR_TALK,
        READY
    }

    public static Game Instance
    {
        get
        {
            return mInstance;
        }
    }
    public CharacterRunningManager Character
    {
        get  {  return character;  }
    }
    void Awake()
    {
        if (!mInstance)
            mInstance = this;
        Events.ChangeGameState += ChangeGameState;

        parkManager = GetComponent<ParkManager>();
        runSignalsManager = GetComponent<RunSignalsManager>();
        lightsManager = GetComponent<LightsManager>();
        avatarRunningMoment = GetComponent<AvatarRunningMoment>();
    }
    private void Start()
    {
        ChangeGameState(state);
    }
    private void OnDestroy()
    {
        Events.ChangeGameState -= ChangeGameState;
    }
    void ChangeGameState(states newState)
    {
        state = newState;
        switch(state)
        {
            case states.AVATAR_TALK: avatarRunningMoment.Init(); break;
        }
    }
    public void Init()
    {        
        runSignalsManager.Init();
    }
    private void Update()
    {
        if (state == states.READY)
            return;

        distance = character.transform.position.z;
        parkManager.OnUpdate(distance);
        if(state == states.RUNNING_SIGNALS)
            runSignalsManager.OnUpdate(distance);
       else if (state == states.AVATAR_TALK)
            avatarRunningMoment.OnUpdate(distance);
    }
    public void SetLightsValue(float value)
    {
        lightsManager.SetStatus(value);
    }

}
