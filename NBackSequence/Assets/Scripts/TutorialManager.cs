using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] Light = new GameObject[8];
    public GameObject[] Tuto = new GameObject[6];

    public int ChangeNum;
    public int tutoNum;

    public Color GreenColor;
    public Color WhiteColor;

    public List<int> order = new List<int>();

    public GameObject TouchOff;

    public int correct;

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

            StartCoroutine("InitLight");
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
            StartCoroutine("InitLight");
            tutoNum = 3;
        }
    }

    IEnumerator InitLight()
    {
        for (int firstChange = 0; firstChange < 3; firstChange++)
        {
            int a = Random.Range(0, 2);
            order.Add(a);
            ChangeNum++;

            yield return new WaitForSeconds(1f);

            Light[a].GetComponent<Image>().color = GreenColor;

            yield return new WaitForSeconds(1f);

            Light[a].GetComponent<Image>().color = WhiteColor;
        }
        TouchOff.SetActive(false);
        GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true);
        Tuto[4].SetActive(false);
        Tuto[5].SetActive(false);
        Tuto[3].SetActive(true);
    }

    public void ClickRight()
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

    public void ClickWrong()
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

    public void ShowResult()
    {
        GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true);
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

    public void Restart()
    {
        SoundManager.instance.Btn_Click();
        SceneChangeManager.SCENE.Tutorial();
    }
}
