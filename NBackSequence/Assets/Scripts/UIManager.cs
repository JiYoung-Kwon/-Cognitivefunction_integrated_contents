using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
            Stage.text = "0" + (GameManager.Game.stage);
        else
            Stage.text = "" + (GameManager.Game.stage);

        Score.text = "" + (GameManager.Game.stage - 1) * 10;

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
        ResultStage.text = Stage.text + " 단계 성공!!";
        ResultTime.text = "시간 - " + _timerText.text;
        ResultScore.text = "점수 - " + Score.text;
    }
}
