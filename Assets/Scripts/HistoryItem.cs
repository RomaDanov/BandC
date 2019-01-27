using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryItem : MonoBehaviour 
{
    [SerializeField]
    private Image[] historyValues = new Image[0];
    [SerializeField]
    private Text historyText = null;

    public void InitHistoryItem(Image[] usersValues, int bullCount, int cowCount)
    {
        for (int i = 0; i < historyValues.Length; i++)
        {
            historyValues [i].color = usersValues [i].color;
        }
        historyText.text = string.Format ("{0}B{1}C", bullCount, cowCount);
    }
}
