using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Game game;
    float rotationFactor;

    int lastFoot = 0;
    CharacterRunningManager character;
    private void Start()
    {
        character = Game.Instance.Character;
        rotationFactor = Data.Instance.settings.rotationFactor;
    }
    void Update()
    {      
        if (Input.GetKeyDown(KeyCode.Space))
            Events.OnKeyPressed("space");

        if (Game.Instance.state == Game.states.INTRO)
            return;

        Vector2 mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        mousePos *= rotationFactor;
        mousePos.y *= -1;
        character.RotateHead(mousePos);
    }
}
