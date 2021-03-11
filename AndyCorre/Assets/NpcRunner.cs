using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRunner : MonoBehaviour
{

    public int stateToshow = 1; // viene de como te fue en la experiencia anterior 1,2 o 3:

    public CharacterAnimationController animationController;

    public SimpleNPCDialogueSignal simpleNPCDialogueSignal;
    public states state;
    public enum states
    {
        IDLE,
        RUNNING,
        MULTIPLECHOICE,
        EXIT
    }

    public int disparadorID = 0;
    public int signalID;

    Settings.SignalData actualSignal = null;

    public Settings.SignalData ActualSignal {
        get { return actualSignal; }
    }
    public void Init()
    {
        signalID = 0;
        Idle();
    }
    public void Idle()
    {
        state = states.IDLE;
    }
    public void Run()
    {
        state = states.RUNNING;
    }
    List<Settings.SignalData> disparatorSignalsData;

    public void Talk()
    {       
        if (state != states.RUNNING) return;
        disparatorSignalsData = Data.Instance.settings.GetSignalsByDisparadorNpc(disparadorID);

        actualSignal = GetSignalByID(signalID);

        if (actualSignal == null)
        {
            NextDisparador();
            return;
        }
        if (actualSignal.multiplechoice != null && actualSignal.multiplechoice.Length > 0)
            state = states.MULTIPLECHOICE;
        else
        {
            float timer = 2 + ((float)actualSignal.text.Length * 0.08f);
            Invoke("Talk", timer);
            signalID++;

          //  print("timer : " + timer + " actualSignal.text.Length: " + actualSignal.text.Length);
        }

       // print("Talk state : " + state + " signalID: " + signalID + " disparadorID " + disparadorID + " --->> Talk actualSignal: " + actualSignal.text);

        simpleNPCDialogueSignal.Init(this, actualSignal);
        simpleNPCDialogueSignal.SetOn(signalID, Data.Instance.settings.GetTotalLinesInDisparadorNpc(disparadorID));
        
    }
    public void MultiplechoiceSelected(Settings.SignalDataMultipleContent content)
    {
        state = states.RUNNING;
        Debug.Log("MultiplechoiceSelected: " + content.text);
        signalID = content.goto_id;
        Talk();
    }
    Settings.SignalData GetSignalByID(int id)
    {
        
        foreach (Settings.SignalData sd in disparatorSignalsData)
        {
            if (sd.id == id)
                return sd;
        }
        return null;
    }
    void NextDisparador()
    {
        Debug.Log("_________NextDisparador: " + disparadorID);
        signalID = 0;
        disparadorID = Data.Instance.settings.GetNextDisparadorIDNPC(disparadorID, stateToshow);
        Debug.Log("Nuevo disparador: " + disparadorID);

        if (disparadorID == -1 || disparadorID >= Data.Instance.settings.allDataNpc.all.Count)
        {
            state = states.EXIT;
        }
        else
        {
            disparatorSignalsData = Data.Instance.settings.GetSignalsByDisparadorNpc(disparadorID);
            Talk();
        }
    }
}
