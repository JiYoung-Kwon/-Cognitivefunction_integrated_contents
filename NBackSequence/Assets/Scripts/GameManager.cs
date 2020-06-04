using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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

    private static GameManager _gManager;
    public static GameManager Game
    {
        get { return _gManager; }
    }

    void Awake()
    {
        _gManager = GetComponent<GameManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        LevelText.text = SceneChangeManager.SCENE.Level + " Back";
        stage = 1;
        life = 2;

        if (SceneChangeManager.SCENE.Level == 3)
        {
            HardStage = 1;
        }
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        StartCoroutine("InitLight");
    }

    IEnumerator InitLight()
    {
        for (int firstChange = 0; firstChange < 3 + HardStage; firstChange++)
        {           
            int a = Random.Range(0, 8);
            order.Add(a);
            ChangeNum++;

            yield return new WaitForSeconds(1f);

            Light[a].GetComponent<Image>().color = GreenColor;

            yield return new WaitForSeconds(1f);

            Light[a].GetComponent<Image>().color = WhiteColor;
        }

        TouchOff.SetActive(false);
    }

    IEnumerator RandomLight()
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
    public void ClickRight()
    {
        if (order[ChangeNum - 1] == order[ChangeNum - 1 - SceneChangeManager.SCENE.Level]) //맞음
        {
            SoundManager.instance.Success();
            Debug.Log("맞음");
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

    public void ClickWrong()
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

    public void GameOver()
    {
        GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true);
        UIManager.UI.ShowResult();
    }
}
