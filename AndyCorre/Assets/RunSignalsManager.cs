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

        if (Mathf.Abs(cam_rotation) < Data.Instance.settings.rotationToActive)
        {
            if (state == states.WAITING)
                actualSignal = null;

            if(state != states.NONE)
                RemoveAllActiveSignals();

            state = states.NONE;
            if (actualSignal == null)
            {
                SetNextSignal();
                nextDistance += viewDistance;
            } 
           
        }
        bool newDisparador = false;
        foreach (RunSignal rSignal in all)
        {
            rSignal.OnUpdate();
            bool isNear = IsNear(rSignal);
            if (isNear)
            {
                if (rSignal.state == RunSignal.states.IDLE)
                {
                    if (!rSignal.data.isDisparador && state != states.NONE && rSignal.data.disparador_id == disparadorID)
                        rSignal.SetOn();
                    else if (rSignal.data.isDisparador && state == states.NONE)
                    {
                        if (Mathf.Abs(cam_rotation) > Data.Instance.settings.rotationToActive && Mathf.Sign(cam_rotation) == Mathf.Sign(rSignal.data.pos_x))
                        {
                            rSignal.SetOn();
                            SetActiveDisparador(rSignal);
                            newDisparador = true;
                        }
                    }
                }
            }
        }
        if(newDisparador)
            ResetSignalsFromOtherDisparadores();
    }
    void ResetSignalsFromOtherDisparadores()
    {
        List<RunSignal> all_to_remove = GetAllSignalsOfDisparador(disparadorID, true);
        foreach (RunSignal rs in all_to_remove)
        {
            RemoveSignal(rs);
        }                
    }
    bool IsNear(RunSignal rSignal)
    {
        if (camTransform.position.z + 4 > rSignal.transform.position.z)
            return true;
        return false;
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

        float distanceToAppear;
        if (state == states.NONE)
            distanceToAppear = distance + viewDistance;
        else
            distanceToAppear = distance+5;


        if (distanceToAppear > nextDistance)
        {            
            //last_z += actualSignal.distance;
            RunSignal signal = Add();
            pos = signal.transform.position;
            pos.x = actualSignal.pos_x;
            print(pos.x);
            pos.z = nextDistance;
            pos.y = 1;
            signal.transform.position = pos;
           
           // if (!actualSignal.isDisparador)
             //   posY++;


            signal.Init(this, actualSignal);

            signalID++;

            if (actualSignal.multiplechoice != null && actualSignal.multiplechoice.Length > 0)
                state = states.WAITING;

            SetNextSignal();
            
        }
    }
    void RemovePrevoiusSignal()
    {
        List<RunSignal> all_to_remove = GetAllSignalsOfDisparador(disparadorID);
        if (all_to_remove.Count > 0)
            RemoveSignal(all_to_remove[0]);
    }
    void SetNextSignal()
    {
        if (all.Count > 0 && state != states.NONE)
            RemovePrevoiusSignal();

        if (state == states.NONE)
            AddDisparador();
        else if (state == states.DISPARADOR)
            AddSignal();
        
        if(actualSignal != null)
            print(state + " SetNextSignal - nextDistance: " + nextDistance + " disparadorID: " + disparadorID + "    SignalID: " + signalID + " actual distance: " + actualSignal.distance + " isDisp"  + actualSignal.isDisparador + " actual text: " + actualSignal.text );
    }
    void SetActiveDisparador(RunSignal rs)
    {
        signalID = 0;
        state = states.DISPARADOR;        
        disparadorID = rs.data.id;
        //ResetAll(rs);
        SetNewSignals();        
    }
    void SetNewSignals()
    {
        disparatorSignalsData = Data.Instance.settings.GetSignalsByDisparador(disparadorID);
        if(disparatorSignalsData.Count >  0)
            actualSignal = disparatorSignalsData[0];
        signalID = 0;
        AddSignal();
    }
    void ForceNewDisparador()
    {
        AddDisparador();
        state = states.NONE;
    }
    void AddSignal()
    {
        Settings.SignalData sData = GetNewSignal();       
        if (sData == null)
        {
            actualSignal = null;
           // Debug.LogError("TA");
            print("no hay signal____________state " + state + " diustance: " + distance);
            //state = states.WAITING;
            //state = states.NONE;
            nextDistance = distance + viewDistance;
            //Invoke("AddDisparador", 2);
        }
        else
        {
            actualSignal = sData;
            nextDistance = distance + actualSignal.distance;
            print(signalID + "NextDistance: " + nextDistance + "   distance: " + distance + " state: " + state);
        }
    }
    void AddDisparador()
    {
        Reset();

        actualSignal = GetNewDisparador();
        disparadorID = actualSignal.id;
        nextDistance = distance + actualSignal.distance + viewDistance;
        print("D_____nextDistance: " + nextDistance + "   distance: " + distance);
    }
    void RemoveAllActiveSignals()
    {
        List<RunSignal> all_to_remove = GetAllSignalsOfDisparador(disparadorID);
        foreach (RunSignal rs in all_to_remove)
        {
            RemoveSignal(rs);
        }            
    }
    List<RunSignal> GetAllSignalsOfDisparador(int _disparadorID, bool notThisDisparador = false)
    {
        print("remove all for " + _disparadorID);
        List<RunSignal> arr = new List<RunSignal>();
        foreach (RunSignal rs in all)
        {
            if(notThisDisparador)
            {
                if (rs.data.disparador_id != _disparadorID || (rs.data.isDisparador && rs.data.id != _disparadorID))
                    arr.Add(rs);
            }
            else 
            if (rs.data.disparador_id == _disparadorID || (rs.data.isDisparador && rs.data.id == _disparadorID))
                arr.Add(rs);
        }
        return arr;
    }
    void RemoveSignal(RunSignal rs)
    {
        all.Remove(rs);
        rs.SetOff();
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
        print("GetNewDisparador" + disparadorID + " distance: " + distance);
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
    public void MultiplechoiceSelected(Settings.SignalDataMultipleContent content)
    {
        state = states.DISPARADOR;
        print("multiplechoice text: " + content.text + " content.goto + " + content.goto_id + " distance: " + distance);

        List<RunSignal> all_to_remove = GetAllSignalsOfDisparador(disparadorID);
        if(all_to_remove.Count>0)
            all_to_remove[0].SetAnimOff();

        signalID = content.goto_id;
        AddSignal();
    }
}
