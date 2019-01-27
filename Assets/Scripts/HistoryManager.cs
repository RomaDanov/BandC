using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryManager : MonoBehaviour 
{
    public static HistoryManager instance;
    [SerializeField]
    private HistoryItem historyItem = null;
    [SerializeField]
    private Transform historyParent = null;
    [SerializeField]
    private ScrollRect scroll = null;

    void Awake()
    {
        instance = this;
    }

    public void AddHistoryItem(Image[] usersValues, int bullCount, int cowCount)
    {
        StartCoroutine (InitHistoryItemWithDelay(usersValues, bullCount, cowCount));
    }

    IEnumerator InitHistoryItemWithDelay(Image[] usersValues, int bullCount, int cowCount)
    {
        HistoryItem item = Instantiate (historyItem, historyParent);
        item.InitHistoryItem (usersValues, bullCount, cowCount);
        yield return new WaitForEndOfFrame ();
        yield return new WaitForEndOfFrame ();
        scroll.verticalScrollbar.value = 0;
    }

    public void ClearHistory()
    {
        for (int i = 0; i < historyParent.childCount; i++)
        {
            Destroy (historyParent.transform.GetChild (i).gameObject);
        }
        scroll.content.localPosition = Vector3.zero;
    }
}
