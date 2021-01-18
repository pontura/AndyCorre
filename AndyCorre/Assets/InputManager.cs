using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Game game;
    public float rotationFactor = 0.1f;

    int lastFoot = 0;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z) && lastFoot != 1)
        {
            lastFoot = 1;
            game.character.Step();
        }
        else if (Input.GetKeyDown(KeyCode.X) && lastFoot != 2)
        {
            lastFoot = 2;
            game.character.Step();
        }
        Vector2 mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        mousePos *= rotationFactor;
        mousePos.y *= -1;
        game.character.RotateHead(mousePos);
    }
}
