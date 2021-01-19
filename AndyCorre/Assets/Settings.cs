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

    [Serializable]
    public class RunSignalSettings
    {
        public float distance_z = 6;
        public float distance_to_ignore = 6;
        public float distance_to_remove = 12;
        public float rotationFreeze = 0.5f;
    }

    public SignalData[] allSignalsData;
    [Serializable]
    public class SignalData
    {
        public int id;
        public float distance = 50;
        public string text;
        public Sprite sprite;
        public Vector2 pos;
    }

}
