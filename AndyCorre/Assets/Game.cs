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
    public float time_to_get_dark;
    public float speed_to_lights;
    public float score;


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

        time_to_get_dark = Data.Instance.settings.time_to_get_dark;
        speed_to_lights = Data.Instance.settings.speed_to_lights;
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
            case states.AVATAR_TALK:
                avatarRunningMoment.Init();
                break;
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
        if (state == states.RUNNING_SIGNALS)
            runSignalsManager.OnUpdate(distance);
        else if (state == states.AVATAR_TALK)
        {
            avatarRunningMoment.OnUpdate(distance);
            if (darkValue > 0)
            {
                darkValue -= speed_to_lights * Time.deltaTime;
            }            
        }
        SetDarkness();
    }
    public void SetLightsValue(float value)
    {
        lightsManager.SetStatus(value);
    }

    public float darkValue;
    public void SetDarknessValue(float value)
    {
        darkValue = value;
    }
    void SetDarkness()
    {
        if (darkValue > 1)
            darkValue = 1;
        else if (darkValue < 0)
            darkValue = 0;
        Game.Instance.SetLightsValue(1 - darkValue);
    }
    public int totalScores;
    public void AddScore(int value)
    {
        score += value;
        totalScores++;
    }
    public int GetStateByScore() // 1 bien 2 medio 3 mal
    {
        float value = (float)score / (float)totalScores;
        int result;
        if (value > 15)
            result = 1;
        else if (value < -15)
            result = 3;
        else
            result = 2;

        print("value: " + value + " score: " + score + " result: " + result);

        return result;
    }
}
