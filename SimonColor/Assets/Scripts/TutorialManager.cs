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
    public GameObject[] Tuto = new GameObject[5];
    public Color[] ButtonColor = new Color[4];
    public List<string> ColorText = new List<string>(); //색깔
    public int tutoNum;
    public Text CircleText;
    public int randomNum;
    public int correct;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void OkayButton() //확인 버튼 클릭
    {
        SoundManager.instance.Btn_Click(); //버튼음

        //총 2번 맞출 때까지 진행
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

            Tuto[tutoNum].SetActive(false); //3내리기
            tutoNum++;

        }
        else if (tutoNum == 3) //4
        {

            GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(false);

            Tuto[tutoNum].SetActive(false); //4내리기
            tutoNum++; //tutoNum == 4

        }
        else
        {
            GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(false);
        }


    }

    public void Init() //초기화함수
    {
        randomNum = Random.Range(0, 4);
        CircleText.color = ButtonColor[Random.Range(0, 4)];
        CircleText.text = ColorText[randomNum];
    }

    public void ButtonClick(int n)
    {
        //값 비교
        if(n == randomNum) //맞추면
        {
            SoundManager.instance.Success(); //성공 효과음
            correct++;

            if (correct == 2) //2번 맞출 경우
            {
                ShowResult(); //결과 화면
            }
            else
            {    
                Tuto[4].SetActive(true);
                GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true);
                Init();
            }

        }       
        else //틀리면
        {
            SoundManager.instance.Failure(); //틀림 효과음
            Tuto[4].SetActive(false);
            Tuto[3].SetActive(true); //5올리기
            GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true);
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
