using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Settings : MonoBehaviour
{
    public float rotationFactor = 0.15f;
    public float signalsSeparationY = 0.5f;
    public RunSignalSettings runSignalSettings;
    public float viewDistance = 50;
    public float rotationToActive = 0.5f;
    public float multiplechoiceSpeed = 5;

    [Serializable]
    public class RunSignalSettings
    {
        public float distance_z = 6;
        public float distance_to_ignore = 6;
        public float distance_to_remove = 12;
        public float rotationFreeze = 0.5f;
    }
    public List<SignalData> disparatoresData;
    public List<SignalData> allSignalsData;

    [Serializable]
    public class SignalData
    {
        public int id;
        [HideInInspector] public bool isDisparador;
        public int disparador_id;
        public float distance = 50;
        public string text;
        public Sprite sprite;
        public Vector2 pos;
        public SignalDataMultipleContent[] multiplechoice;
    }
    [Serializable]
    public class SignalDataMultipleContent
    {
        public int goto_id;
        public string text;
    }
    private void Start()
    {
        SetIDS();
    }
    public void SetIDS()
    {
        int id = 0;
        foreach(SignalData sd in disparatoresData)
        {
            sd.id = id;
            sd.disparador_id = id;
            id++;
            sd.isDisparador = true;
        }
        id = 0;
        int lastDisparadorID = -1;
        SignalData disparador;
        Vector2 pos;
        foreach (SignalData sd in allSignalsData)
        {            
            if (sd.disparador_id == lastDisparadorID)
            {
                id++;
            }
            else
            {               
                id = 0;
                lastDisparadorID = sd.disparador_id;
            }
            disparador = GetDisparador(lastDisparadorID);
            sd.pos = disparador.pos;
           // sd.id = id;
        }
    }
    public SignalData GetDisparador(int id)
    {
        foreach (SignalData sd in disparatoresData)
            if (sd.id == id)
                return sd;
        return null;
    }
    public List<SignalData> GetSignalsByDisparador(int id)
    {
        List<SignalData> all = new List<SignalData>();
        foreach (SignalData sd in allSignalsData)
            if (sd.disparador_id == id)
                all.Add(sd);
        return all;
    }
}
