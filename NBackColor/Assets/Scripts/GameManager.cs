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
    public GameObject[] Light = new GameObject[9]; //불 들어오는 패널

    public int ChangeNum;
    public int stage;

    //패널 색상 정보
    public Color GreenColor;
    public Color RedColor;
    public Color WhiteColor;
    public Color RandomColor;

    public List<int> order = new List<int>();

    public GameObject[] Life = new GameObject[3]; //life(하트 이미지)
    public GameObject TouchOff; //불깜빡이는 중 터치 방지를 위한 투명 이미지; 깜빡임 종료 시 SetActive(false)
    public int life; //life 개수
    public int HardStage = 0;

    public Text LevelText;
    #endregion

    #region Singleton
    private static GameManager _gManager;
    public static GameManager Game
    {
        get { return _gManager; }
    }

    void Awake()
    {
        _gManager = GetComponent<GameManager>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        LevelText.text = SceneChangeManager.SCENE.Level + " Back"; //n Back 텍스트 설정

        //stage&life 초기화
        stage = 1;
        life = 2;

        if (SceneChangeManager.SCENE.Level == 3)
        {
            HardStage = 1;
        }

        Init();
    }

    public void Init() //초기화 코루틴 실행
    {
        StartCoroutine("InitLight");
    }

    IEnumerator InitLight() //패널 불 초기화
    {
        //게임 난이도에 맞춰 처음 빛나는 횟수 조정
        //easy,normal : 3번, Hard : 4번
        for (int firstChange = 0; firstChange < 3 + HardStage; firstChange++) 
        {
            int a = Random.Range(0, 2); //0~1 랜덤
            order.Add(a);//빛나는 순서 리스트에 추가
            ChangeNum++;

            if (a == 0) 
                RandomColor = GreenColor; //0 : 초록불
            else
                RandomColor = RedColor; //1 : 빨간불

            
            //불 깜빡임 (1초)
            yield return new WaitForSeconds(1f);
      
            for (int i = 0; i < 9; i++) //9개의 패널 모두
            {
                Light[i].GetComponent<Image>().color = RandomColor;
            }

            yield return new WaitForSeconds(1f);

            for (int i = 0; i < 9; i++)
            {
                Light[i].GetComponent<Image>().color = WhiteColor;
            }


        }

        TouchOff.SetActive(false);
    }

    IEnumerator RandomLight() //LightOn 함수(두번째부터)
    {
        int a = Random.Range(0, 2);
        order.Add(a);
        ChangeNum++;

        if (a == 0)
            RandomColor = GreenColor;
        else
            RandomColor = RedColor;

        for (int i = 0; i < 9; i++)
        {
            Light[i].GetComponent<Image>().color = RandomColor;
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 9; i++)
        {
            Light[i].GetComponent<Image>().color = WhiteColor;
        }

        TouchOff.SetActive(false);
    }

    //1Back이면 바로 직전 값 비교
    public void ClickRight() //O를 클릭했을 경우
    {
        if (order[ChangeNum - 1] == order[ChangeNum - 1 - SceneChangeManager.SCENE.Level]) //맞음
        {
            SoundManager.instance.Success(); //맞음사운드
            Debug.Log("맞음");
            stage++;
            StartCoroutine("RandomLight");
        }
        else //틀림
        {
            SoundManager.instance.Failure(); //틀림사운드
            Debug.Log("틀림");

            //틀린경우 라이프 감소
            Life[life].SetActive(false);
            life--;

            //life == 0이 되면 게임오버
            if (life < 0)
            {
                GameOver();
            }

            StartCoroutine("RandomLight");
        }
        TouchOff.SetActive(true);

    }
 
    public void ClickWrong() //X를 클릭했을 경우
    {
        if (order[ChangeNum - 1] != order[ChangeNum - 1 - SceneChangeManager.SCENE.Level]) //맞음
        {
            Debug.Log("맞음");
            SoundManager.instance.Success();
            stage++;
            StartCoroutine("RandomLight");
        }
        else //틀림
        {
            SoundManager.instance.Failure();
            Debug.Log("틀림");
            Life[life].SetActive(false);
            life--;

            if (life < 0)
            {
                GameOver();
            }

            StartCoroutine("RandomLight");
        }
        TouchOff.SetActive(true);
    }

    public void GameOver() //게임오버+결과화면 함수
    {
        GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true); //결과화면 띄우기
        UIManager.UI.ShowResult(); //결과값 설정
    }
}
