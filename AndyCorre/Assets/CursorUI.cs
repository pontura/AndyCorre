using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorUI : MonoBehaviour
{
    public Sprite simple;
    public Sprite read;
    public Sprite click;
    public Image image;

    public enum types
    {
        SIMPLE,
        READ,
        CLICK
    }
    void Start()
    {
        Cursor.visible = false;
        Events.ChangeCursor += ChangeCursor;
    }
    void OnDestroy()
    {
        Events.ChangeCursor -= ChangeCursor;
    }
    private void Update()
    {
        transform.position = Input.mousePosition;
    }
    types lastType;
    void ChangeCursor(types type, Color color)
    {

        color.a = 0.75f;
        image.color = color;
        if (type == lastType)
            return;
        lastType = type;
        switch (type)
        {
            case types.READ:
                image.sprite = read;
                break;
            case types.SIMPLE:
                image.sprite = simple;
                break;
            case types.CLICK:
                image.sprite = click;
                break;
        }
    }
}
