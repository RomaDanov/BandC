using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour 
{
    public static MessageManager instance;
    [SerializeField]
    private GameObject messagePanel;
    [SerializeField]
    private Text messageText;
    [SerializeField]
    private Button confirmButton;

    private void Awake()
    {
        instance = this;
    }

    public void ShowDialogueMessage(string message, UnityAction confirmAction)
    {
        messagePanel.SetActive (true);
        messageText.text = message;
        confirmButton.onClick.RemoveAllListeners ();
        confirmButton.onClick.AddListener (confirmAction);
    }
}
