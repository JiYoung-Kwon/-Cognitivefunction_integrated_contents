using Newtonsoft.Json;
using System;
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

    // Start is called before the first frame update
    void Start() //시작 시 초기화
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Stage text 설정
        if (GameManager.Game.stage - 1 < 10)
            Stage.text = "0" + (GameManager.Game.stage - 1);
        else
            Stage.text = "" + (GameManager.Game.stage - 1);

        //Score text 설정
        Score.text = "" + (GameManager.Game.stage - 2) * 10;

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
        ResultStage.text = Stage.text;
        ResultTime.text = _timerText.text;
        ResultScore.text = Score.text;
        StartCoroutine(SendScore());
    }
    IEnumerator SendScore() //점수 정보 전송
    {
        //data 값
        ScoreStatisticsDbData data = new ScoreStatisticsDbData();
        data.UserID = SceneChangeManager.Patient.UserID;
        data.Date = DateTime.Now.ToString("yy/MM/dd");
        data.Game = "SimonOrder";
        data.Type = "Score";
        data.Score = int.Parse(ResultScore.text);
        data.Level = GameManager.Game.stage - 1;

        ETC etc = new ETC
        {
            PlayTime = ResultTime.text
        };

        //Object를 Json String으로 변환
        string jsonETC = JsonConvert.SerializeObject(etc);
        data.etc = jsonETC;
        string json_key = JsonConvert.SerializeObject(data);

        //string을 byte[] 배열로 변환
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json_key);

        using (UnityWebRequest www = new UnityWebRequest(SaveScoreURL, UnityWebRequest.kHttpVerbPOST)) //HTTP POST 요청
        {
            UploadHandlerRaw uH = new UploadHandlerRaw(bytes); //HTTP 요청 중 본문 데이터의 버퍼링 및 전송 관리
            DownloadHandlerBuffer dH = new DownloadHandlerBuffer(); //원격 서버로부터 수신 된 바디 데이터를 관리

            www.uploadHandler = uH;
            www.downloadHandler = dH;
            www.SetRequestHeader("Content-Type", "application/json"); //HTTP 요청 헤더 값 설정
            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError) //에러 발생 시
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
