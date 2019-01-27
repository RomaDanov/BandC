using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    public static GameManager instance;

    private const string OWNERS_ID = "eaf1dc18eec4bb98";
    private const int COUNT = 4;

    private Color[] generatedValues = new Color[COUNT];

    [SerializeField]
    private Image[] usersValues = new Image[COUNT];

    [SerializeField]
    private Button checkButton = null;
    [SerializeField]
    private Button cheatButton = null;
    [SerializeField]
    private GameObject howToPanel = null;
    private const string HOW_TO_KEY = "firstStart";

    [SerializeField]
    private Text chanceText = null;
    private int chanceCount = 0;

    private int B = 0;
    private int C = 0;

    [SerializeField]
    private Color[] colors;
    public Color[] Colors
    {
        get
        {
            return colors;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        OnNewGame ();

#if UNITY_EDITOR
        cheatButton.gameObject.SetActive (true);
#else
        if (IsOwner ())
        {
            cheatButton.gameObject.SetActive (true);
        }
#endif

        bool firstStart = PlayerPrefs.GetInt (HOW_TO_KEY, 0) == 0 ? true : false;
        if (firstStart)
        {
            howToPanel.SetActive (true);
            PlayerPrefs.SetInt (HOW_TO_KEY, 1);
        }
    }

    bool IsOwner()
    {
        AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
        AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject> ("getContentResolver");  
        AndroidJavaClass secure = new AndroidJavaClass ("android.provider.Settings$Secure");
        string connectedID = secure.CallStatic<string> ("getString", contentResolver, "android_id");
        if (connectedID.Equals (OWNERS_ID))
        {
            return true;
        }
        return false;
    }

    public void OnNewGame()
    {
        chanceCount = 0;
        chanceText.text = "";
        StateOfUsersButtons (true);
        ClearValue ();
        ClearHistory ();
        GenerateValue ();
        StartCoroutine (CheckIdentityValueWithDelay ());
    }

    IEnumerator CheckIdentityValueWithDelay ()
    {
        yield return new WaitForEndOfFrame ();
        CheckIdentityValue ();
    }

    public void OpenClosePanel(GameObject panel)
    {
        panel.SetActive (!panel.activeSelf);
    }

    public void GenerateValue()
    {
        SetBotsValuesToDefault ();
        for (int i = 0; i < COUNT; i++)
        {
            int random = Random.Range (0, colors.Length);
            if (generatedValues [i] == Color.black)
            {
                generatedValues [i] = colors [random];
            }
        }
        CheckBotIdentityValue ();
    }

    void SetBotsValuesToDefault()
    {
        for (int l = 0; l < generatedValues.Length; l++) 
        {
            generatedValues [l] = Color.black;
        }
    }

    public void OnCheckColors()
    {
        ClearValue ();

        chanceCount++;
        for (int i = 0; i < usersValues.Length; i++)
        {
            for (int k = 0; k < generatedValues.Length; k++) 
            {
                if (usersValues [i].color == generatedValues [k])
                {
                    if (i == k)
                    {
                        B++;
                        if (B == COUNT)
                        {
                            AddedHistoryItem ();
                            string attemptText = "";
                            if (chanceCount == 1)
                            {
                                attemptText = "попытка";
                            }
                            else
                            {
                                attemptText = "попытки";
                            }
                            chanceText.text = "<color=#FDAC3FFF>" + chanceCount + "</color> " + attemptText;
                            StateOfUsersButtons (false);
                            //ShowAd ();
                            return;
                        }
                        break;
                    }
                    else
                    {
                        C++;
                        break;
                    }
                }
            }
        }
        AddedHistoryItem ();
    }

    void StateOfUsersButtons(bool state)
    {
        checkButton.interactable = state;
        for (int i = 0; i < usersValues.Length; i++) 
        {
            usersValues [i].GetComponent<Button> ().interactable = state;
        }
    }

    void AddedHistoryItem()
    {
        HistoryManager.instance.AddHistoryItem (usersValues, B, C);
    }

    void ClearHistory()
    {
        HistoryManager.instance.ClearHistory ();
    }

    void ClearValue()
    {
        B = 0;
        C = 0;
    }

    public void CheckIdentityValue()
    {
        for (int i = 0; i < usersValues.Length; i++)
        {
            Color identity = usersValues [i].color;
            for (int k = 0; k < usersValues.Length; k++) 
            {
                if (usersValues [i] != usersValues [k])
                {
                    if (identity == usersValues [k].color)
                    {
                        checkButton.interactable = false;
                        return;
                    }
                }
            }  
        }
        checkButton.interactable = true;
    }

    void CheckBotIdentityValue()
    {
        for (int i = 0; i < generatedValues.Length; i++)
        {
            Color identity = generatedValues [i];
            for (int k = 0; k < generatedValues.Length; k++) 
            {
                if (i != k)
                {
                    if (identity == generatedValues [k])
                    {
                        GenerateValue ();
                        return;
                    }
                }
            }  
        }
    }

    public void OnCheatButtonClick()
    {
        checkButton.interactable = true;
        for (int i = 0; i < usersValues.Length; i++) 
        {
            usersValues [i].color = generatedValues [i];
        }
    }

    void OnExitGameClick()
    {
        Application.Quit ();
    }

    void ShowAd()
    {
        if (Advertisement.IsReady ())
        {
            Advertisement.Show ("", new ShowOptions(){resultCallback = HandlerAdResult});
        }
    }

    void HandlerAdResult(ShowResult result)
    {
        switch (result)
        {
        case ShowResult.Finished:
            Debug.Log ("Ad is finished");
            break;
        case ShowResult.Skipped:
            Debug.Log ("Ad is skipped");
            break;
        case ShowResult.Failed:
            Debug.Log ("Ad is failed");
            break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            MessageManager.instance.ShowDialogueMessage ("Are you sure you want to exit?", OnExitGameClick);
        }
    }
}
