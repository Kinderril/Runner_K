using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Block : MonoBehaviour 
{
    public float width = 4;
    public float heightL = 1;
    public float heightR = 1;
    public string date;
    public Vector3 PreRotatePos;
    private bool isRotate = false;

    public bool IsRotate
    {
        get { return isRotate; }
        set { isRotate = value; }
    }

    void Awake()
    {
        date = DateTime.Now.ToFileTimeUtc().ToString();
    }

    public void SetCurrent()
    {
        SetColor(Color.green);
    }
    public void SetLast()
    {
        SetColor(Color.cyan);
    }
    private void SetColor(Color c)
    {
        var images = GetComponentsInChildren<SpriteRenderer>();
        foreach (var image in images)
        {
            image.color = c;
        }
    }
}
