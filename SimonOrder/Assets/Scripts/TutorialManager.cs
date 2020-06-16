using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] Button = new GameObject[4];
    public GameObject[] Tuto = new GameObject[5];
    public int tutoNum;
    public List<int> order = new List<int>(); //순서
    public GameObject Circle;
    public GameObject TouchOff;
    public Color OriginColor;
    public Color HighColor;
    public Text CircleText;
    public int randomNum;
    public int correct;
    public bool IsCorrect = true;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OkayButton()
    {
        SoundManager.instance.Btn_Click();
        //1->2->3->게임진행->4
        //맞춤-> 6 -> 게임진행 -> 4 -> 맞추면 끝. 틀리면 다시
        //틀림-> 5 -> 게임진행 -> 4 -> 무한반복

        if (tutoNum < 2)  //1,2
        {
            Tuto[tutoNum].SetActive(false); //1내리기
            tutoNum++;
            Tuto[tutoNum].SetActive(true); //2올리기
        }
        else if (tutoNum == 2) //3
        {
            Tuto[4].SetActive(false);
            GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(false);
            Init();
            StartCoroutine("Display");
            Tuto[tutoNum].SetActive(false); //3내리기
            tutoNum++;
            Tuto[tutoNum].SetActive(true); //4올리기
        }
        else if (tutoNum == 3) //4
        {

            GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(false);

            Circle.GetComponent<Image>().color = HighColor;
            randomNum = Random.Range(0, 2); //0~1까지(맨처음) -> 1~2까지
            CircleText.text = "" + (randomNum + 1);

            TouchOff.SetActive(false);
            Tuto[tutoNum].SetActive(false); //4내리기
            tutoNum++; //tutoNum == 4
        }
        else if (tutoNum == 4) //5 틀림or 맞춤
        {
           
            if (!IsCorrect) //틀린경우에서 클릭하면
            {
                Tuto[3].SetActive(true);
                Tuto[4].SetActive(false);
            }
            if (IsCorrect)
            {
                Tuto[3].SetActive(true);
                Tuto[5].SetActive(false);
            }
        
            //게임진행
            GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(false);
            Init();
            StartCoroutine("Display");
            tutoNum = 3;
        }

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
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 2; i++)
        {
            order.Add(Random.Range(0, 4)); //0~3까지
            yield return new WaitForSeconds(1f);
            Button[order[i]].GetComponent<Image>().color = HighColor;
            SoundManager.instance.Bell(order[i]);
            yield return new WaitForSeconds(1f);
            Button[order[i]].GetComponent<Image>().color = OriginColor;
        }

        yield return new WaitForSeconds(0.5f);

        TouchOff.SetActive(false); //추가
        GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true); //4 올려져있는 상태
    }

    public void ButtonClick(int n)
    {
        Button[n].GetComponent<Image>().color = HighColor;
        //값 비교
        if (n == order[randomNum]) //맞추면
        {
            SoundManager.instance.Success();
            IsCorrect = true;
            correct++;

            if (correct == 2)
            {
                ShowResult();
            }
            else
            {
                Tuto[5].SetActive(true); //6올리기
                GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true);
                Debug.Log("맞았당");
                TouchOff.SetActive(true); //추가
            }
        }       
        else
        {
            SoundManager.instance.Failure();
            IsCorrect = false;
            Tuto[4].SetActive(true); //5올리기
            GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true);
            TouchOff.SetActive(true); //추가s
        }
    }

    public void PauseButton()
    {
        SoundManager.instance.Btn_Click();
        Time.timeScale = 0f;
    }

    public void KeepGoing()
    {
        SoundManager.instance.Btn_Click();
        Time.timeScale = 1f;
    }

    public void HomeButton()
    {
        SoundManager.instance.Btn_Click();
        SceneChangeManager.SCENE.MainMenu();
    }

    public void ShowResult()
    {
        GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true);
    }

    public void Restart()
    {
        SoundManager.instance.Btn_Click();
        SceneChangeManager.SCENE.Tutorial();
    }
}
