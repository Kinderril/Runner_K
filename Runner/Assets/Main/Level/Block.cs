using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Block : MonoBehaviour 
{
    public float width = 4;
    public float heightL = 1;
    public float heightR = 1;


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
        Debug.Log("set color: " + images.Length);
        foreach (var image in images)
        {
            image.color = c;
        }
    }
}
