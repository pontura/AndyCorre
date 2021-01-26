using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RunSignalsManager : MonoBehaviour
{
    [SerializeField] private RunSignal runSignal_to_add;
    [SerializeField] private Transform container;
    
    float last_z = 0;
    float viewDistance = 50;

    List<Settings.SignalData> disparatoresData;
    public List<Settings.SignalData> disparatorSignalsData;
    Settings.SignalData actualSignal;
    public List<RunSignal> all;
    Transform camTransform;

    public int disparadorID;
    public int signalID;

    public states state;
    public enum states
    {
        NONE,
        DISPARADOR,
        WAITING
    }

    float nextDistance;
    float distance;

    public void Init()
    {
        
        camTransform = Game.Instance.Character.cam.transform;
        viewDistance = Data.Instance.settings.viewDistance;
        nextDistance = viewDistance;
        disparatoresData = Data.Instance.settings.disparatoresData;
        all = new List<RunSignal>();
        SetNextSignal();
    }

    float posY;

    Settings.SignalData GetSignalDataByID(int id)
    {
        foreach(Settings.SignalData d in disparatorSignalsData)
        {
            if (d.id == id)
                return d;
        }
        return null;
    }
    void SetDarkness()
    {
        float dark = 1 - ((float)all.Count / 6);
        Game.Instance.SetLightsValue(dark);
    }
    void CheckAllSignalsState()
    {

        float cam_rotation = camTransform.rotation.y;

        RunSignal newDisparador = null;
        RunSignal signalOff = null;
        foreach (RunSignal rSignal in all)
        {
            if (camTransform.position.z + 4 > rSignal.transform.position.z)
            {
                if (!rSignal.data.isDisparador && state == states.NONE)
                    signalOff = rSignal;
                else if (Mathf.Abs(cam_rotation) < Data.Instance.settings.rotationToActive || (rSignal.state == RunSignal.states.IDLE && Mathf.Sign(cam_rotation) != Mathf.Sign(rSignal.data.pos.x)))
                    signalOff = rSignal;
                else if (state == states.NONE)
                    newDisparador = rSignal;
            }
            rSignal.OnUpdate();
        }
        if(signalOff != null)
            signalOff.SetOff();
        if (newDisparador != null)
            SetActiveDisparador(newDisparador);

        if (Mathf.Abs(cam_rotation) < Data.Instance.settings.rotationToActive)
        {
            state = states.NONE;
            if (actualSignal == null)
            {                
                SetNextSignal();
                nextDistance += viewDistance;
            }
        }
    }
    
    public void OnUpdate(float distance)
    {
        this.distance = distance;
        SetDarkness();
        CheckAllSignalsState();

        if (actualSignal == null)
            return;

        if (state == states.WAITING)
            return;

        Vector3 pos;
        

        if (state == states.DISPARADOR && actualSignal.isDisparador)
            return;
        
        if (distance + viewDistance > nextDistance)
        {            
            //last_z += actualSignal.distance;
            RunSignal signal = Add();
            pos = signal.transform.position;
            pos.x = actualSignal.pos.x;
            pos.z = nextDistance;
            signal.transform.position = pos;

            if (!actualSignal.isDisparador)
                posY++;

            print(signalID + " Pos Y: " + posY + " text: " + actualSignal.text);

            actualSignal.pos.y = 1.5f - Data.Instance.settings.signalsSeparationY * posY;

            signal.Init(this, actualSignal);

            signalID++;

            if (actualSignal.multiplechoice.Length > 0)
                state = states.WAITING;
            else
                SetNextSignal();
            
        }
    }
    void SetNextSignal()
    {
        if (state == states.NONE)
            AddDisparador();
        else if (state == states.DISPARADOR)
            AddSignal();
        if (actualSignal != null)
        {
            nextDistance = distance + actualSignal.distance + viewDistance;
        }
        print("SetNextSignal - nextDistance: " + nextDistance + " disparadorID: " + disparadorID + "    SignalID: " + signalID);
    }
    void SetActiveDisparador(RunSignal rs)
    {        
        state = states.DISPARADOR;        
        disparadorID = rs.data.id;
        ResetAll(rs);
        SetNewSignals();        
    }
    void SetNewSignals()
    {
        disparatorSignalsData = Data.Instance.settings.GetSignalsByDisparador(disparadorID);
        if(disparatorSignalsData.Count >  0)
            actualSignal = disparatorSignalsData[0];
        signalID = 0;
    }
    void AddSignal()
    {
        actualSignal = GetNewSignal();
    }
    void AddDisparador()
    {
        Reset();

        actualSignal = GetNewDisparador();
        disparadorID = actualSignal.id;        
    }
    public void RemoveSignal(RunSignal rs)
    {
        all.Remove(rs);
    }
    RunSignal Add()
    {
        RunSignal rs = Instantiate(runSignal_to_add);
        all.Add(rs);
        rs.transform.SetParent(container);
        rs.transform.localScale = Vector3.one;
        rs.transform.localPosition = Vector3.zero;        
        return rs;
    }
    void Reset()
    {
        posY = 0;
    }
    Settings.SignalData GetNewDisparador()
    {
        print("GetNewDisparador" + disparadorID);
        signalID = 0;
        int id = 0;
        foreach (Settings.SignalData sd in disparatoresData)
        {
            id++;
            if (sd.id == disparadorID && id < disparatoresData.Count)
            {
                return disparatoresData[id];
            }               
        }
        return disparatoresData[0];
    }
    Settings.SignalData GetNewSignal()
    {
       
        return GetSignalDataByID(signalID);       
    }
    void ResetAll(RunSignal signalDontDestroyable = null)
    {
        List<RunSignal> _all = new List<RunSignal>();
        foreach (RunSignal rs in all)
        {
            if (rs != signalDontDestroyable)
                _all.Add(rs);
        }
        foreach (RunSignal rs in _all)
        {
            rs.SetOff();
            //Destroy(rs.gameObject);
        }
    }
    public void MultiplechoiceSelected(Settings.SignalDataMultipleContent content)
    {
        state = states.DISPARADOR;
        print("multiplechoice text: " + content.text + " content.goto + " + content.goto_id);
        ResetAll(all[0]);
        posY = 0;
        signalID = content.goto_id;
        AddSignal();
    }
}
