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
    float timeInRama;
    float time_to_get_dark;
    float speed_to_lights;
    public void Init()
    {
        time_to_get_dark = Data.Instance.settings.time_to_get_dark;
        speed_to_lights = Data.Instance.settings.speed_to_lights;
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
        if (darkValue > 1)
            darkValue = 1;
        else if (darkValue < 0)
            darkValue = 0;
        Game.Instance.SetLightsValue(1-darkValue);
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
                        rSignal.SetOn(rSignal.data.id + 1, Data.Instance.settings.GetTotalLinesInDisparador(disparadorID));
                    else if (rSignal.data.isDisparador && state == states.NONE)
                    {
                        if (Mathf.Abs(cam_rotation) > Data.Instance.settings.rotationToActive && Mathf.Sign(cam_rotation) == Mathf.Sign(rSignal.data.pos_x))
                        {
                            rSignal.SetOn(0, Data.Instance.settings.GetTotalLinesInDisparador(disparadorID));
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
            RemoveSignal(rs);
    }
    bool IsNear(RunSignal rSignal)
    {
        if ((camTransform.position.z + 4 > rSignal.transform.position.z) 
            && (camTransform.position.z < rSignal.transform.position.z))
            return true;
        return false;
    }
    float darkValue;
    public void OnUpdate(float distance)
    {
        this.distance = distance;
        CheckAllSignalsState();       

        if(state != states.NONE)
        {
            timeInRama += Time.deltaTime;
            darkValue = timeInRama / time_to_get_dark;                
        } else
        {
            darkValue -= Time.deltaTime * speed_to_lights;
        }
        SetDarkness();

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
        //  if (all_to_remove.Count > 0)
        //    RemoveSignal(all_to_remove[0]);
        int id = 0;
        foreach (RunSignal rs in all_to_remove)
        {
            id++;
            if (id == all_to_remove.Count)
                return;
            RemoveSignal(rs);          
        }
            

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
            print(" SetNextSignal - nextDistance: " + nextDistance + " disparadorID: " + disparadorID + "    SignalID: " + signalID + " actual distance: " + actualSignal.distance + " isDisp"  + actualSignal.isDisparador + " state: " + state + " all.Count: " + all.Count + " actual text: " + actualSignal.text );
    }
    void SetActiveDisparador(RunSignal rs)
    {
        timeInRama = 0;
        signalID = 0;
        state = states.DISPARADOR;        
        disparadorID = rs.data.id;
        Events.PlaySound("music", "theme1", true);
        Events.ChangeVolume("music", 0);
        Events.FadeVolume("music", 1, 5);
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
    void ForceFinishDisparador()
    {
        RemoveAllActiveSignals();
    }
    void AddSignal()
    {
        Settings.SignalData sData = GetNewSignal();       
        if (sData == null)
        {
            actualSignal = null;
            nextDistance = distance + viewDistance;
            Data.Instance.settings.SetDisparadorDone(disparadorID);
            Invoke("ForceFinishDisparador", 4);
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

        actualSignal = Data.Instance.settings.GetNextDisparador();
        if(actualSignal == null)
        {
            Debug.Log("GAME OVER");
            return;
        }
        nextDistance = distance + actualSignal.distance + viewDistance;
        print("sale disparadorID: " + actualSignal.id + "  nextDistance: " + nextDistance + "   distance: " + distance);
    }
    void RemoveAllActiveSignals()
    {
        CancelInvoke();
        Events.FadeVolume("music", 0, 5);
        List<RunSignal> all_to_remove = GetAllSignalsOfDisparador(disparadorID);
        foreach (RunSignal rs in all_to_remove)
        {
            RemoveSignal(rs);
        }            
    }
    List<RunSignal> GetAllSignalsOfDisparador(int _disparadorID, bool notThisDisparador = false)
    {
        print("remove others,  but: " + _disparadorID);
        List<RunSignal> arr = new List<RunSignal>();
        foreach (RunSignal rs in all)
        {
            if(notThisDisparador)
            {
                if (rs.data.disparador_id != _disparadorID)
                    arr.Add(rs);
            }
            else 
            if (rs.data.disparador_id == _disparadorID)
                arr.Add(rs);
        }
        return arr;
    }
    void RemoveSignal(RunSignal rs)
    {
        all.Remove(rs);
        rs.SetOff();
       // Destroy(rs.gameObject);
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
