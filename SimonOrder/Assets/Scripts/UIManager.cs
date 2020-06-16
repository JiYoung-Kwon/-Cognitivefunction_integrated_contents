using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static string SaveScoreURL = "http://127.0.0.1:57540/Contents/SaveScore";
    private static UIManager _uiManager;
    public static UIManager UI
    {
        get { return _uiManager; }
    }

    void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }

    public double _time = 0;
    public Text _timerText;
    public Text Stage;
    public Text Score;

    public Text ResultTime;
    public Text ResultStage;
    public Text ResultScore;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Game.stage - 1 < 10)
            Stage.text = "0" + (GameManager.Game.stage - 1);
        else
            Stage.text = "" + (GameManager.Game.stage - 1);

        Score.text = "" + (GameManager.Game.stage - 2) * 10;

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

    public void GameStart()
    {
        SoundManager.instance.Btn_Click();
        Time.timeScale = 1f;
    }

    public void GameQuit()
    {
        SoundManager.instance.Btn_Click();
        SceneChangeManager.SCENE.MainMenu();
    }

    public void GamePause()
    {
        SoundManager.instance.Btn_Click();
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        SoundManager.instance.Btn_Click();
        SceneChangeManager.SCENE.GameStart();
    }

    public void ShowResult()
    {
        ResultStage.text = Stage.text;
        ResultTime.text = _timerText.text;
        ResultScore.text = Score.text;
        StartCoroutine(SendScore());
    }
    IEnumerator SendScore()
    {
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
