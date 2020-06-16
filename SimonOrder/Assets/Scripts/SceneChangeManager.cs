using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static string PatientInfoURL = "http://127.0.0.1:57540/ServerConnect/GetPatientInfo?game=SimonOrder";
    string jsonstring;
    public static PatientInfo Patient;

    private static SceneChangeManager _sceneManager;
    public static SceneChangeManager SCENE
    {
        get { return _sceneManager; }
    }

    void Awake()
    {
        _sceneManager = GetComponent<SceneChangeManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ServerConnect());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu()
    {
        SoundManager.instance.Btn_Click();
        SceneManager.LoadScene("Main");
    }

    public void GameStart()
    {
        SoundManager.instance.Btn_Click();
        SceneManager.LoadScene("Game");
    }

    public void Tutorial()
    {
        SoundManager.instance.Btn_Click();
        SceneManager.LoadScene("Tutorial");
    }

    public void Exit()
    {
        SoundManager.instance.Btn_Click();
        Application.Quit();
    }
    IEnumerator ServerConnect()
    {
        using (UnityWebRequest www = new UnityWebRequest(PatientInfoURL, UnityWebRequest.kHttpVerbGET))
        {
            DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

            www.downloadHandler = dH;
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                jsonstring = www.downloadHandler.text;

                if (!jsonstring.Equals("[]") && !jsonstring.Equals(""))
                {
                    var playerData = JsonConvert.DeserializeObject<PatientInfo>(jsonstring);

                    if (playerData != null)
                    {
                        Patient = new PatientInfo
                        {
                            UserID = playerData.UserID,
                            Name = playerData.Name,
                            Level = playerData.Level
                        };
                    }
                }
            }
        }
    }
}
