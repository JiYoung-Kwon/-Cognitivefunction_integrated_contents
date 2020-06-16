using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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

    private static GameManager _gameManager;
    public static GameManager Game
    {
        get { return _gameManager; }
    }

    void Awake()
    {
        _gameManager = GetComponent<GameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        life = 2;   
        stage = 2;       
        Init();
        StartCoroutine("Display");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init() //초기화함수
    {
        Circle.GetComponent<Image>().color = OriginColor; //원 색 어둡게
        CircleText.text = "";

        for (int i = 0; i < 4; i++)
            Button[i].GetComponent<Image>().color = OriginColor; //버튼 색 어둡게
     
        order.Clear();
    }

    IEnumerator Display()
    {
        for (int i = 0; i < stage; i++)
        {
            order.Add(Random.Range(0, 4)); //0~3까지
            yield return new WaitForSeconds(1f);
            Button[order[i]].GetComponent<Image>().color = HighColor;
            SoundManager.instance.Bell(order[i]);
            yield return new WaitForSeconds(1f);
            Button[order[i]].GetComponent<Image>().color = OriginColor;
        }

        yield return new WaitForSeconds(1f);
        Circle.GetComponent<Image>().color = HighColor;

        //숫자는 1,2 순서를 고르는거야!!!
        randomNum = Random.Range(0, stage); //0~1까지(맨처음) -> 1~2까지
        CircleText.text =   "" + (randomNum+1);
        TouchOff.SetActive(false);
    }

    public void ButtonClick(int n)
    {    
        Button[n].GetComponent<Image>().color = HighColor;
      
        //값 비교
        if (n == order[randomNum])
        {
            SoundManager.instance.Success();
            StartCoroutine("Right");
        }
        else
        {
            SoundManager.instance.Failure();
            Wrong();
        }
    }

    IEnumerator Right()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("맞았당");

        Init();
        stage++;
        TouchOff.SetActive(true);
        StartCoroutine("Display"); 
    }

    public void Wrong()
    {
        Debug.Log("틀렸어ㅡㅡ");
        Life[life].SetActive(false);
        life--;

        if (life < 0)
        {
            GameOver();
        }

        Init();
        TouchOff.SetActive(true);
        StartCoroutine("Display");
    }

    public void GameOver()
    {
        GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true);
        Time.timeScale = 0f; //추가
        UIManager.UI.ShowResult();
    }

}
