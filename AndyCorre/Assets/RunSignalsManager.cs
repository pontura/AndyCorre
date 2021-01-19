using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RunSignalsManager : MonoBehaviour
{
    [SerializeField] private RunSignal runSignal_to_add;
    [SerializeField] private Transform container;
    int id;
    float last_z = 0;
    float viewDistance = 50;

    Settings.SignalData[] allSignalsData;
    Settings.SignalData actualSignal;
    List<RunSignal> all;

  
    public void Init()
    {
        viewDistance = Data.Instance.settings.viewDistance;
        allSignalsData = Data.Instance.settings.allSignalsData;
        all = new List<RunSignal>();
        SetNewSignal();
        lastSignalID = actualSignal.id;
    }
    int lastSignalID = 0;
    float posY;
    public void OnUpdate(float distance)
    {
        if (all.Count == 0 && lastSignalID != actualSignal.id)
        {
            Reset();
            last_z = distance + viewDistance;
            lastSignalID = actualSignal.id;
        } else if (lastSignalID < actualSignal.id && all.Count > 0)
            return;

        float dark = 1 - ((float)all.Count / 10);
        Game.Instance.SetLightsValue(dark);

        Vector3 pos;
        if (distance > last_z+actualSignal.distance - viewDistance)
        {
           
            last_z += actualSignal.distance;
            RunSignal signal = Add();
            pos = signal.transform.position;
            pos.x = actualSignal.pos.x;
            pos.z = last_z;
            signal.transform.position = pos;
            id++;

            if (lastSignalID == actualSignal.id)
                posY++;
            else
                posY = 0;
            actualSignal.pos.y = 1.5f - Data.Instance.settings.signalsSeparationY * posY;

            signal.Init(this, actualSignal);
            lastSignalID = actualSignal.id;

            SetNewSignal();
            
        }
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
    private void Reset()
    {
        posY = 0;
    }
    void SetNewSignal()
    {
        if (id >= allSignalsData.Length)
        {
            Reset();
            id = 0;
        }
        actualSignal = allSignalsData[id];
    }
}
