using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 게임 매니저 스크립트
 */

public class GameManager : MonoBehaviour
{
    #region 변수
    public GameObject[] Button = new GameObject[4];
    public GameObject[] Life = new GameObject[3];
    public Color[] ButtonColor = new Color[4];
    public GameObject TouchOff;
    public Color OriginColor;
    public Color HighColor;

    public List<string> ColorText = new List<string>(); //색깔
    public int stage;
    public int life;
    public int randomNum;
    public Text CircleText;

    bool firstresult = true;
    #endregion

    #region Singleton
    private static GameManager _gameManager;
    public static GameManager Game
    {
        get { return _gameManager; }
    }

    void Awake()
    {
        _gameManager = GetComponent<GameManager>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {      
        //시작 시 초기화
        life = 2;   
        stage = 2;       
        Init();        
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.UI._time <= 0 && firstresult) //타임오버이거나 firstresult == true일 경우
        {
            firstresult = false;
            GameOver(); //게임 종료
        }
    }

    public void Init() //초기화함수
    {
        randomNum = Random.Range(0, 4); //4개중 하나
        CircleText.color = ButtonColor[Random.Range(0, 4)];
        CircleText.text = ColorText[randomNum];
    }

    public void ButtonClick(int n) //버튼 클릭 함수
    {
        if (n == randomNum) //맞은 경우
        {
            Debug.Log("맞음");
            SoundManager.instance.Success(); //성공 사운드
            Init(); //초기화
            stage ++;
        }
        else //틀린 경우
        {
            Debug.Log("틀림");
            SoundManager.instance.Failure(); //실패 사운드
            Init(); //초기화

            //라이프 감소
            Life[life].SetActive(false);
            life--;

            if (life < 0) //3번 틀릴 경우
            {
                GameOver(); //게임 종료
            }
        }        
    }

    public void GameOver() //게임 종료 함수
    {
        Time.timeScale = 0f;
        GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true);
        UIManager.UI.ShowResult(); //결과
    }

}
