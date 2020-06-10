using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
* UI 매니저 스크립트
*/

public class UIManager : MonoBehaviour
{
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

    #region 변수
    public double _time = 0;
    public Text _timerText;
    public Text Stage;
    public Text Score;

    public Text ResultTime;
    public Text ResultStage;
    public Text ResultScore;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //단계 UI
        if (GameManager.Game.stage - 1 < 10)
            Stage.text = "0" + (GameManager.Game.stage);
        else
            Stage.text = "" + (GameManager.Game.stage);

        //점수 UI
        Score.text = "" + (GameManager.Game.stage - 1) * 10;

        //시간 UI
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

    public void GameQuit() //게임 종료
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

    public void ShowResult() //결과UI
    {
        ResultStage.text = Stage.text + " 단계 성공!!";
        ResultTime.text = "시간 - " + _timerText.text;
        ResultScore.text = "점수 - " + Score.text;
    }
}
