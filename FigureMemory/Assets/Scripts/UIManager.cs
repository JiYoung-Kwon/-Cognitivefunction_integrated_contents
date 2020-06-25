using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/*
* UI 매니저 스크립트
*/

public class UIManager : MonoBehaviour
{
    public static string SaveScoreURL = "http://127.0.0.1:57540/Contents/SaveScore";

    #region 변수
    public double _time = 0;
    public Text _timerText;
    public Text Stage;
    public Text Score;

    public Text ResultTime;
    public Text ResultStage;
    public Text ResultScore;
    #endregion

    #region Singleton
    private static UIManager _uiManager;
    public static UIManager UI
    {
        get { return _uiManager; }
    }

    void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }
    #endregion

    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Stage text 설정
        Stage.text = "" + (GameManager.Game.stage);

        //Score text 설정
        Score.text = "" + (GameManager.Game.stage - 1) * 10;

        //시간 text 설정
        _time += Time.deltaTime;
        int minute = (int)_time / 60;
        int second = (int)_time - (minute * 60);
        int second1 = second / 10;
        int second2 = second % 10;

        if (minute / 10 != 0)
        {
            _timerText.text = (minute.ToString() + ":" + second1.ToString() + second2.ToString());
        }
        else
        {
            _timerText.text = ("0" + minute.ToString() + ":" + second1.ToString() + second2.ToString());
        }
    }

    public void GameStart() //게임시작
    {
        SoundManager.instance.Btn_Click();
        Time.timeScale = 1f;
        StartCoroutine (GameManager.Game.InitCard());
    }

    public void Continue() //계속하기
    {
        SoundManager.instance.Btn_Click();
        Time.timeScale = 1f;
    }

    public void GameQuit() //홈으로
    {
        SoundManager.instance.Btn_Click();
        SceneChangeManager.SCENE.MainMenu();
    }

    public void GamePause() //일시정지
    {
        SoundManager.instance.Btn_Click();
        Time.timeScale = 0f;
    }

    public void Restart() //다시하기
    {
        SoundManager.instance.Btn_Click();
        SceneChangeManager.SCENE.GameStart();
    }

    public void ShowResult() //결과 Text 관리
    {
        ResultStage.text = "단계 - " + Stage.text;
        ResultTime.text = "시간 - " + _timerText.text;
        ResultScore.text = "점수 - " + Score.text;

    }
    
    IEnumerator SendScore() //점수 정보 전송 - SimonOrder code 주석 참고
    {
        ScoreStatisticsDbData data = new ScoreStatisticsDbData();
        data.UserID = SceneChangeManager.Patient.UserID;
        data.Date = System.DateTime.Now.ToString("yy/MM/dd");
        data.Game = "도형기억력";
        data.Type = "점수";
        data.Score = (GameManager.Game.stage - 1) * 10;
        data.Level = SceneChangeManager.SCENE.Level;
        ETC etc = new ETC();
        if (SceneChangeManager.SCENE.Level == 0)
        {
            etc.GameType = "색+모양-" + SceneChangeManager.SCENE.NBack;
        }
        else
        {
            etc.GameType = "모양조합-" + SceneChangeManager.SCENE.NBack;
        }
        etc.PlayTime = _timerText.text;
        
        string jsonETC = JsonConvert.SerializeObject(etc);
        data.etc = jsonETC;
        string json_key = JsonConvert.SerializeObject(data);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json_key);
        using (UnityWebRequest www = new UnityWebRequest(SaveScoreURL, UnityWebRequest.kHttpVerbPOST))
        {
            UploadHandlerRaw uH = new UploadHandlerRaw(bytes);
            DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

            www.uploadHandler = uH;
            www.downloadHandler = dH;
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("결과 데이터 전송 완료");
            }
        }
    }
}
