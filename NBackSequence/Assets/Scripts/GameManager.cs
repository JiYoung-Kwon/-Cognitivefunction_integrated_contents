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
    public GameObject[] Light = new GameObject[8];

    public int ChangeNum;
    public int stage;

    public Color GreenColor;
    public Color WhiteColor;

    public List<int> order = new List<int>();

    public GameObject[] Life = new GameObject[3];
    public GameObject TouchOff;
    public int life;
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
            int a = Random.Range(0, 8);//0~7 랜덤
            order.Add(a);//빛나는 순서 리스트에 추가
            ChangeNum++;

            //불 깜빡임 (1초)
            //8개의 패널 중 랜덤 1개 점등
            yield return new WaitForSeconds(1f);

            Light[a].GetComponent<Image>().color = GreenColor;

            yield return new WaitForSeconds(1f);

            Light[a].GetComponent<Image>().color = WhiteColor;
        }

        TouchOff.SetActive(false);
    }

    IEnumerator RandomLight() //LightOn 함수(두번째부터)
    {
        int a = Random.Range(0, 8);
        order.Add(a);
        ChangeNum++;

        Light[a].GetComponent<Image>().color = GreenColor;

        yield return new WaitForSeconds(1f);

        Light[a].GetComponent<Image>().color = WhiteColor;

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
        if (order[ChangeNum - 1] != order[ChangeNum - 1 - SceneChangeManager.SCENE.Level]) //틀림
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
