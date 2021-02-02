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
    }
    public void Init()
    {
        parkManager = GetComponent<ParkManager>();
        runSignalsManager = GetComponent<RunSignalsManager>();
        lightsManager = GetComponent<LightsManager>();
        runSignalsManager.Init();
    }
    private void Update()
    {
        distance = character.transform.position.z;
        parkManager.OnUpdate(distance);
        runSignalsManager.OnUpdate(distance);
    }
    public void SetLightsValue(float value)
    {
        lightsManager.SetStatus(value);
    }

}
