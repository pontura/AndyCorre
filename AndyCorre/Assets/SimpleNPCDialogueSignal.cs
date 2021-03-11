using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleNPCDialogueSignal : MonoBehaviour
{
    Transform container;
    Transform target;
    
    Settings.RunSignalSettings settings;
    public Settings.SignalData data;
    NpcRunner manager;
    public states state;
    public GameObject view_multiplechoice;
    public RunMultiplechoiceButton mButton;
    public Canvas canvas;
    List<RunMultiplechoiceButton> multiplechoiceAll;
    Color fieldColor;
    // public Image bar;
    //public GameObject barAsset;
    public GameObject npcFace;

    public Text field;
    public Image fillImage;

    public Text fieldMy;
    public Image fillImageMy;
    bool isMultiplechoice;

    public enum states
    {
        IDLE,
        ON
    }
    private void Start()
    {
        npcFace.SetActive(false);
    }
    public void Init(NpcRunner manager, Settings.SignalData data)
    {
        fieldColor = Color.white;
        //fieldColor = Data.Instance.settings.disparadoresColors[data.disparador_id];
       // bar.color = fieldColor;
        container = transform.parent;
        SetCanvasAlpha(0.5f);
        canvas.worldCamera = Game.Instance.Character.cam;
        this.data = data;
        multiplechoiceAll = new List<RunMultiplechoiceButton>();
        view_multiplechoice.SetActive(false);
        if (data.multiplechoice != null && data.multiplechoice.Length > 0)
        {
            Utils.RemoveAllChildsIn(view_multiplechoice.transform);
            view_multiplechoice.SetActive(true);
            isMultiplechoice = true;
            foreach (Settings.SignalDataMultipleContent d in data.multiplechoice)
            {
                RunMultiplechoiceButton button = Instantiate(mButton);
                button.transform.SetParent(view_multiplechoice.transform);
                button.InitNpc(this, d, fieldColor);
                multiplechoiceAll.Add(button);
            }
          //  SetBarOff();
        }
        else
            isMultiplechoice = false;

        settings = Data.Instance.settings.runSignalSettings;
        npcFace.SetActive(false);
        this.manager = manager;
        target = Game.Instance.Character.cam.transform;
        if (data.character == "npc")
        {
            if (data.multiplechoice == null || data.multiplechoice.Length == 0)
                npcFace.SetActive( true);
            field.text = data.text;
            fieldMy.text = "";
        }
        else
        {
            fieldMy.text = data.text;
            field.text = "";
        }

        fillImage.fillAmount = 0f;
        fillImageMy.fillAmount = 0f;
    }
   // void SetBarOff()
   // {
        //Vector3 barPos = barAsset.transform.position;
        //barPos.y += 1000;
        //barAsset.transform.position = barPos;
  //  }
    public void OnOverMultiplechoice(RunMultiplechoiceButton buttonOver)
    {
        if (state != states.ON)
            return;
        if (buttonOver == null)
        {
            foreach (RunMultiplechoiceButton m in multiplechoiceAll)
                m.RollOut();
        }
        else
        {
            foreach (RunMultiplechoiceButton m in multiplechoiceAll)
                if (m != buttonOver)
                    m.SetOff();
        }
    }
    float barTo;
    public void SetOn(int id, int total)
    {
      //  barAsset.SetActive(true);
        filled = 0;
       // Events.PlaySound("voices", data.audio + ".wav", false);
        Events.ChangeCursor(CursorUI.types.READ, fieldColor);
        Events.PlaySound("ui", "signalOn", false);
        StopAllCoroutines();
        SetCanvasAlpha(1);
      //  bar.fillAmount = 0f;
        state = states.ON;
        id++;
        total++;

        //bar.fillAmount = (float)(id - 1) / (float)total;
        //barTo = (float)id / (float)total;
        //if (id > total - 1)
        //    SetBarOff();
    }
    float filled;
    //void Update()
    //{
    //    if (state == states.ON)
    //    {
    //        if (bar.fillAmount <= barTo)
    //            bar.fillAmount += Time.deltaTime / 1;
    //    }
    //}
    void SetCanvasAlpha(float value)
    {
        canvas.GetComponent<CanvasGroup>().alpha = value;
    }
    bool animOff;
    public void SetAnimOff()
    {
        if (animOff) return;

        animOff = true;
        canvas.GetComponent<Animation>().Play();
    }
    public void OnUpdate()
    {
        Vector3 pos = transform.position;

       // transform.LookAt(target);
        float cam_rotation = Mathf.Abs(target.transform.rotation.y);

        if (state == states.ON)
        {
            if (!isMultiplechoice && !data.isDisparador)
            {
                if (filled < 1)
                {
                    float d = Data.Instance.settings.speedToReadRunSignals + ((float)(data.text.Length) * (Data.Instance.settings.speedToReadRunSignalsByLetter / 100));
                    filled += Time.deltaTime / d;
                }

                else filled = 1;
                fillImage.fillAmount = filled;
                fillImageMy.fillAmount = filled;
            }
           // pos.z = Mathf.Lerp(pos.z, target.transform.position.z + settings.distance_z, Time.deltaTime * (cam_rotation / settings.rotationFreeze));
        }
        pos.y = target.transform.position.y - 1 - (Game.Instance.Character.cam.transform.parent.transform.localPosition.y / 50);

        transform.position = pos;
    }

    public void Clicked(Settings.SignalDataMultipleContent content)
    {
        manager.MultiplechoiceSelected(content);
    }
}
