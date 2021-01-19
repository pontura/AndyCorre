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
        if(Input.GetKeyDown(KeyCode.Z) && lastFoot != 1)
        {
            lastFoot = 1;
            character.Step();
        }
        else if (Input.GetKeyDown(KeyCode.X) && lastFoot != 2)
        {
            lastFoot = 2;
            character.Step();
        }
        Vector2 mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        mousePos *= rotationFactor;
        mousePos.y *= -1;
        character.RotateHead(mousePos);
    }
}
