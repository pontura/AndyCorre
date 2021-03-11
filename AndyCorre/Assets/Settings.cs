using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Settings : MonoBehaviour
{
    public TextAsset npcTextAsset;
    public TextAsset textAsset;
    public Color[] disparadoresColors;

    public float distanceBetweenContentText;
    public float distanceByLetters;
    public float timeToFinishLastText;

    public float speedToReadRunSignalsByLetter;
    public float speedToReadRunSignals;
    public float maxSpeed = 10;
    public float desaceleration = 5;
    public float time_to_get_dark = 10;
    public float speed_to_lights = 2;
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
        public float rotationFreeze = 0.5f;
    }
    public List<SignalData> disparatoresData;
    public List<SignalData> allSignalsData;

    public List<SignalData> disparatoresDataNpc;
    public List<SignalData> allSignalsDataNpc;

    AllData all;
    [Serializable]
    class AllData
    {
        public List<SignalData> all;
    }

    public AllDataNpc allDataNpc;
    [Serializable]
    public class AllDataNpc
    {
        public List<SignalData> all;
    }

    [Serializable]
    public class SignalData
    {
        public bool done;
        public int id;
        [HideInInspector] public bool isDisparador;
        public string audio;
        public string character;
        public int disparador_id;
        public float distance;
        public string text;
        public Sprite sprite;
        public float pos_x;
        public SignalDataMultipleContent[] multiplechoice;
        [HideInInspector] public List<SignalData> content;
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
        Load();
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
            sd.pos_x = disparador.pos_x;
            sd.distance = 20;
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
    public int GetNextDisparadorIDNPC(int disparador_id, int stateToshow) // busca si el proximo disparador es del estado en el que jugaste antes "STATE_1"..
    {
        if (disparador_id > allDataNpc.all.Count)
            return -1;

        disparador_id++;
        SignalData sd = allDataNpc.all[disparador_id];
        string[] s = sd.text.Split("_"[0]);
        if (s.Length < 2 || int.Parse(s[1]) == stateToshow)
            return disparador_id;
        else
            return GetNextDisparadorIDNPC(disparador_id, stateToshow);
    }
    public List<SignalData> GetSignalsByDisparadorNpc(int disparador_id)
    {
        List<SignalData> all = new List<SignalData>();
        foreach (SignalData sd in allSignalsDataNpc)
            if (sd.disparador_id == disparador_id)
                all.Add(sd);
        return all;
    }
    void Load()
    {
        all = JsonUtility.FromJson<AllData>(textAsset.text);
        allDataNpc = JsonUtility.FromJson<AllDataNpc>(npcTextAsset.text);
        SaveData();
        SaveDataNpc();
        Game.Instance.Init();
    }
    void SaveData()
    {
        allSignalsData.Clear();
        disparatoresData.Clear();
        int disparadorID = 0;
        foreach (SignalData sd in all.all)
        {
            sd.isDisparador = true;
            sd.id = disparadorID;
            sd.disparador_id = disparadorID;
            int id = 0;
            disparatoresData.Add(sd);
            foreach (SignalData content in sd.content)
            {
                if (content.id == 0)
                {
                    content.id = id;
                    id++;
                }
                else
                {
                    id = content.id;
                }
                content.disparador_id = sd.id;

                if (content.distance == 0)
                {
                    float d = distanceBetweenContentText;
                    if(content.id > 0 && content.text != null) 
                         d += ((float)(content.text.Length) * (distanceByLetters / 50));

                    content.distance = d;

                }
                if (content.id >= 10)
                {
                    content.distance = 5; //    multiplechoice answer:
                }
                   

                content.pos_x = sd.pos_x;
                allSignalsData.Add(content);
            }
            disparadorID++;
        }
    }



    void SaveDataNpc()
    {
        allSignalsDataNpc.Clear();
        disparatoresDataNpc.Clear();
        int disparadorID = 0;
        foreach (SignalData sd in allDataNpc.all)
        {
            sd.isDisparador = true;
            sd.id = disparadorID;
            sd.disparador_id = disparadorID;
            int id = 0;
            disparatoresDataNpc.Add(sd);
            foreach (SignalData content in sd.content)
            {
                if (content.id == 0)
                {
                    content.id = id;
                    id++;
                }
                else
                {
                    id = content.id;
                }
                content.disparador_id = sd.id;

                if (content.distance == 0)
                {
                    float d = distanceBetweenContentText;
                    if (content.id > 0 && content.text != null)
                        d += ((float)(content.text.Length) * (distanceByLetters / 50));

                    content.distance = d;

                }
                if (content.id >= 10)
                {
                    content.distance = 5; //    multiplechoice answer:
                }


                content.pos_x = sd.pos_x;
                allSignalsDataNpc.Add(content);
            }
            disparadorID++;
        }
    }


    int disparadorArrayPos = 0;
    public SignalData GetNextDisparador()
    {
        int id = 0;
        if (disparadorArrayPos >= disparatoresData.Count - 1)
            disparadorArrayPos = 0;

        foreach (SignalData sd in disparatoresData)
        {
            if (id == disparadorArrayPos && !sd.done)
            {
                disparadorArrayPos++;
                return sd;
            }
            else if (id > disparadorArrayPos)
                disparadorArrayPos++;
            id++;
        }
        id = 0;
        disparadorArrayPos = 0;
        foreach (SignalData sd in disparatoresData)
        {
            if (!sd.done)
            {
                disparadorArrayPos = id;
                return sd;
            }
            id++;
        }
        return null;
    }
    public void SetDisparadorDone(int disparadorID)
    {
        foreach(SignalData sd in disparatoresData)
        {
            if (sd.id == disparadorID)
                sd.done = true;
        }
    }
    public int GetTotalLinesInDisparador(int disparadorID)
    {
        int id = 0;
        foreach (SignalData sd in allSignalsData)
        {
            if (sd.disparador_id == disparadorID)
            {
                id++;
                if (sd.multiplechoice != null && sd.multiplechoice.Length > 0)
                    return id;
            }
        }
        return id;
    }
    public int GetTotalLinesInDisparadorNpc(int disparadorID)
    {
        int id = 0;
        foreach (SignalData sd in allSignalsDataNpc)
        {
            if (sd.disparador_id == disparadorID)
            {
                id++;
                if (sd.multiplechoice != null && sd.multiplechoice.Length > 0)
                    return id;
            }
        }
        return id;
    }
}
