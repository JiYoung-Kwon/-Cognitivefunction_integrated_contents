using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/*
* 씬 관리 스크립트
*/

public class SceneChangeManager : MonoBehaviour
{
    public static string PatientInfoURL = "http://127.0.0.1:57540/ServerConnect/GetPatientInfo";
    public static PatientInfo Patient;
    string jsonstring;
    public int NBack = 1;
    public int Level = 0;

    #region Singleton
    private static SceneChangeManager _sceneManager;
    public static SceneChangeManager SCENE
    {
        get { return _sceneManager; }
    }

    void Awake()
    {
        _sceneManager = GetComponent<SceneChangeManager>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ServerConnect());
    }

    public void MainMenu() //메인으로
    {
        SoundManager.instance.Btn_Click();
        SceneManager.LoadScene("Main");
    }

    public void GameStart() //게임시작
    {
        SoundManager.instance.Btn_Click();
        SceneManager.LoadScene("Game");
    }

    public void Tutorial() //튜토리얼
    {
        SoundManager.instance.Btn_Click();
        SceneManager.LoadScene("Tutorial");
    }

    public void Exit() //나가기
    {
        SoundManager.instance.Btn_Click();
        Application.Quit();
    }

    public void Setting() //설정
    {
        SoundManager.instance.Btn_Click();
    }

    public void NBackSetting(int i) //NBack 설정(1~2)
    {
        SoundManager.instance.Btn_Click();
        NBack = i;
    }

    public void LevelSetting(int i) //색+모양인지 모양조합인지(색+모양 0 모양조합 1)
    {
        SoundManager.instance.Btn_Click();
        Level = i;
    }

    IEnumerator ServerConnect() //서버 연결 - SimonOrder 주석 참고
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
