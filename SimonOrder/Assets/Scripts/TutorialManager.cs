using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 튜토리얼 매니저 스크립트
 */

public class TutorialManager : MonoBehaviour
{
    #region 변수
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
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void OkayButton() //확인 버튼 클릭
    {
        SoundManager.instance.Btn_Click(); //버튼음

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

    IEnumerator Display() //랜덤 패널 점등 함수
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 2; i++)
        {
            order.Add(Random.Range(0, 4)); //0~3까지
            yield return new WaitForSeconds(1f);
            Button[order[i]].GetComponent<Image>().color = HighColor; //점등
            SoundManager.instance.Bell(order[i]); //점등 사운드
            yield return new WaitForSeconds(1f);
            Button[order[i]].GetComponent<Image>().color = OriginColor; //불꺼짐
        }

        yield return new WaitForSeconds(0.5f);

        TouchOff.SetActive(false); //추가
        GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true); //4 올려져있는 상태
    }

    public void ButtonClick(int n) //버튼 클릭 함수
    {
        Button[n].GetComponent<Image>().color = HighColor; //불 켜짐
        //값 비교
        if (n == order[randomNum]) //맞추면
        {
            SoundManager.instance.Success(); //성공 효과음
            IsCorrect = true;
            correct++;

            if (correct == 2) //2번 맞출 경우
            {

                ShowResult(); //결과 화면
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

    public void PauseButton() //일시정지
    {
        SoundManager.instance.Btn_Click();
        Time.timeScale = 0f;
    }

    public void KeepGoing() //계속하기
    {
        SoundManager.instance.Btn_Click();
        Time.timeScale = 1f;
    }

    public void HomeButton() //홈으로
    {
        SoundManager.instance.Btn_Click();
        SceneChangeManager.SCENE.MainMenu();
    }

    public void ShowResult() //결과 패널 띄우기
    {
        GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true);
    }

    public void Restart() //다시하기
    {
        SoundManager.instance.Btn_Click();
        SceneChangeManager.SCENE.Tutorial();
    }
}
