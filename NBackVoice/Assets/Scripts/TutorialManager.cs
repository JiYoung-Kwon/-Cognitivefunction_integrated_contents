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
    public AudioClip[] Sound;
    AudioSource Audio;

    public GameObject[] Tuto = new GameObject[6];

    public int ChangeNum;
    public int tutoNum;

    public List<int> order = new List<int>();

    public GameObject TouchOff;

    public int correct;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        Time.timeScale = 1f;
    }

    public void OkayButton()
    {
        SoundManager.instance.Btn_Click();

        //게임진행
        //0-> 1-> 2 -> 반짝임3번 -> 4 -> 틀림/맞음 -> 4

        if (tutoNum < 2) //0,1
        {
            Tuto[tutoNum].SetActive(false); //0,1내리기 
            tutoNum++;
            Tuto[tutoNum].SetActive(true); //1,2올리기
        }
        else if (tutoNum == 2) //2
        {
            //패널 내리고 문구 내리고
            Tuto[tutoNum].SetActive(false); //2 내리기
            GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(false);
            tutoNum++;
            Tuto[tutoNum].SetActive(true); //3올리기
            //게임 진행하고

            StartCoroutine("InitSound");
        }
        else if (tutoNum == 3) //3
        {
            Tuto[tutoNum].SetActive(false); //내리기
            tutoNum++; //tutoNum ==4
            GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(false);
            //패널내리기, 클릭후 값 비교
        }
        else
        {
            GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(false);
            StartCoroutine("InitSound");
            tutoNum = 3;
        }
    }

    IEnumerator InitSound() //불빛 초기화
    {
        for (int firstChange = 0; firstChange < 3; firstChange++)
        {
            int a = Random.Range(0, 2);
            order.Add(a);
            ChangeNum++;

            yield return new WaitForSeconds(1f);

            Audio.PlayOneShot(Sound[a]);

            yield return new WaitForSeconds(1f);
        }
        TouchOff.SetActive(false);
        GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true);
        Tuto[4].SetActive(false);
        Tuto[5].SetActive(false);
        Tuto[3].SetActive(true);
    }

    public void ClickRight() //O를 클릭했을 경우
    {
        if (order[ChangeNum - 1] == order[ChangeNum - 2]) //맞음
        {
            correct++;
            SoundManager.instance.Success();
            Debug.Log("맞음");

            if (correct == 2)
            {
                ShowResult();
            }
            else
            {
                //맞은 문구 4
                Tuto[4].SetActive(true);
            }
        }
        else //틀림
        {
            
            SoundManager.instance.Failure();
            Debug.Log("틀림");
            Tuto[4].SetActive(false);
            Tuto[5].SetActive(true);

        }
        TouchOff.SetActive(true);
        //패널 올리기
        GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true);
    }

    public void ClickWrong() //X를 클릭했을 경우
    {
        if (order[ChangeNum - 1] != order[ChangeNum - 2]) //틀림
        {
            correct++;
            Debug.Log("맞음");
            SoundManager.instance.Success();
            if (correct == 2)
            {
                ShowResult();
            }
            else
            {
                //맞은 문구 4
                Tuto[4].SetActive(true);
            }
        }
        else //틀림
        {
            SoundManager.instance.Failure();
            Debug.Log("틀림");
            Tuto[4].SetActive(false);
            Tuto[5].SetActive(true);

        }
        TouchOff.SetActive(true);
        GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true);
    }

    public void ShowResult() //결과 패널
    {
        GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true);
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

    public void Restart() //다시하기
    {
        SoundManager.instance.Btn_Click();
        SceneChangeManager.SCENE.Tutorial();
    }
}
