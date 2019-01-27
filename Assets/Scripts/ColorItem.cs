using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorItem : MonoBehaviour 
{
    public int itemID = 0;
    [SerializeField]
    private Image image;

    public void Init(Color color, int id)
    {
        itemID = id;
        image.color = color;
    }
}
