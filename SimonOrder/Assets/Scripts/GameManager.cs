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
    public GameObject Circle;
    public GameObject TouchOff;
    public Color OriginColor;
    public Color HighColor;

    public List<int> order = new List<int>(); //순서
    public int stage;
    public int life;
    public int randomNum;
    public Text CircleText;
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
        StartCoroutine("Display");
    }

    public void Init() //초기화함수
    {
        Circle.GetComponent<Image>().color = OriginColor; //원 색 어둡게
        CircleText.text = "";

        for (int i = 0; i < 4; i++)
            Button[i].GetComponent<Image>().color = OriginColor; //버튼 색 어둡게
     
        order.Clear();
    }

    IEnumerator Display() //랜덤 패널 점등 함수
    {
        //1스테이지 : 2번
        //n스테이지 : n+1번 랜덤 점등
        for (int i = 0; i < stage; i++)
        {
            order.Add(Random.Range(0, 4)); //0~3 랜덤
            yield return new WaitForSeconds(1f);
            Button[order[i]].GetComponent<Image>().color = HighColor; //불 켜짐
            SoundManager.instance.Bell(order[i]); //점등 사운드
            yield return new WaitForSeconds(1f);
            Button[order[i]].GetComponent<Image>().color = OriginColor; //불 꺼짐
        }

        yield return new WaitForSeconds(1f);
        Circle.GetComponent<Image>().color = HighColor; //가운데 원 점등

        //가운데 원에 있는 숫자 = 순서 중 하나
        randomNum = Random.Range(0, stage); //0부터 n까지 +1 (1부터 n+1번까지)
        CircleText.text =   "" + (randomNum+1);
        TouchOff.SetActive(false);
    }

    public void ButtonClick(int n) //버튼 클릭 함수
    {    
        Button[n].GetComponent<Image>().color = HighColor; //클릭한 패널 점등
      
        //값 비교
        if (n == order[randomNum])
        {
            SoundManager.instance.Success(); //성공 사운드
            StartCoroutine("Right");
        }
        else
        {
            SoundManager.instance.Failure(); //실패 사운드
            Wrong();
        }
    }

    IEnumerator Right() //맞았을 때
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("맞았당");

        Init();
        stage++;
        TouchOff.SetActive(true);
        StartCoroutine("Display"); 
    }

    public void Wrong() //틀렸을 때
    {
        Debug.Log("틀렸어ㅡㅡ");

        //라이프 감소
        Life[life].SetActive(false);
        life--;

        if (life < 0) //3번 틀릴 경우
        {
            GameOver(); //게임 종료
        }

        Init();
        TouchOff.SetActive(true);
        StartCoroutine("Display");
    }

    public void GameOver() //게임 종료 함수
    {
        GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true);
        Time.timeScale = 0f;
        UIManager.UI.ShowResult(); //결과
    }

}
