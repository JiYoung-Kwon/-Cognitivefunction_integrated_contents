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
    public static string PatientInfoURL = "http://127.0.0.1:57540/ServerConnect/GetPatientInfo?game=SimonOrder";
    string jsonstring;
    public static PatientInfo Patient;

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
    IEnumerator ServerConnect() //서버 연결
    {
        using (UnityWebRequest www = new UnityWebRequest(PatientInfoURL, UnityWebRequest.kHttpVerbGET)) //HTTP GET 요청
        {
            DownloadHandlerBuffer dH = new DownloadHandlerBuffer(); //수신 된 데이터를 기본 바이트 버퍼에 저장하기 위함

            www.downloadHandler = dH;
            www.SetRequestHeader("Content-Type", "application/json"); //HTTP 요청 헤더 값 설정
            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError) //에러 발생 시
            {
                Debug.Log(www.error);
            }
            else
            {
                jsonstring = www.downloadHandler.text;

                if (!jsonstring.Equals("[]") && !jsonstring.Equals(""))
                {
                    var playerData = JsonConvert.DeserializeObject<PatientInfo>(jsonstring); //Json string으로부터 Object 가져오기

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
