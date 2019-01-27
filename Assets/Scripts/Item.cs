using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour 
{
    [SerializeField]
    private Image image = null;

    private int index = 0;

    void Start()
    {
        index = transform.GetSiblingIndex ();
        ChangeColorToIndex (index);
    }

    public void OnClick()
    {
        if (index + 1 == GameManager.instance.Colors.Length)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        ChangeColorToIndex (index);
        GameManager.instance.CheckIdentityValue ();
    }

    void ChangeColorToIndex(int i)
    {
        image.color = GameManager.instance.Colors [i];
    }
}
