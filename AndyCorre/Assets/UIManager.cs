using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public RunUIManager runUIManager;
    public GameObject endProtoPanel;
    public Intro intro;

    void Start()
    {
        runUIManager.gameObject.SetActive(false);
        endProtoPanel.SetActive(false);        
    }
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Replay");
    }
    void OnKeyPressed(string key)
    {
        runUIManager.OnKeyPressed(key);
    }
    public void InitRunning()
    {
        runUIManager.gameObject.SetActive(true);
        Events.OnKeyPressed += OnKeyPressed;
    }
    private void OnDestroy()
    {
        Events.OnKeyPressed -= OnKeyPressed;
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void EndProto()
    {
        endProtoPanel.SetActive(true);
    }
}
