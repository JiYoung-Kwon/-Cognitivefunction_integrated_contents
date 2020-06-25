using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * 게임 매니저 스크립트
 */

public class GameManager : MonoBehaviour
{
    #region 변수
    public GameObject Block;
    public GameObject TextNum;
    public Text UITime;
    public Text Result;
    public Text Number;
    public double time = 0;
    public bool isPaused = true;
    public bool isPlayed = false;
    public int num;
    public int count = 1;
    public int WrongCount = 0;
    #endregion

    //void Start()
    //{

    //    isPaused = true;
    //    isPlayed = false;

    //}

    void Update()
    {
        //time 관리
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            time += Time.deltaTime;
            UITime.text = "" + time.ToString("0.0");
        }
    }

    public void PauseButton() //일시정지 사운드
    {
        if (isPaused == false)
        {
            PlaySound("Click");
            isPaused = true;
        }
    }

    public void StartButton() //시작 사운드
    {
        if (isPaused == true)
        {
            PlaySound("Click");
            isPaused = false;
        }
    }

    public void TutoButton() //튜토리얼 사운드
    {
        PlaySound("Click");
    }

    public void FadeOut(string strtag) //마우스를 가져다 대면
    {
        for (int i = 0; i < Block.transform.childCount; i++)
        {
            if (Block.transform.GetChild(i).tag.Equals(strtag)) //해당 블럭과 동일한 태그를 가진 블럭들
            {
                Block.transform.GetChild(i).GetComponent<Image>().color = new Color(188 / 255f, 188 / 255f, 188 / 255f); //색 어둡게
            }
        }
    }

    public void FadeIn(string strtag) //마우스가 빠져나오면
    {
        for (int i = 0; i < Block.transform.childCount; i++)
        {
            if (Block.transform.GetChild(i).tag.Equals(strtag)) //해당 블럭과 동일한 태그를 가진 블럭들
            {
                Block.transform.GetChild(i).GetComponent<Image>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f); //원래 색으로
            }
        }
    }

    public void Click(string strtag) //클릭 시
    {
        for (int i = 0; i < TextNum.transform.childCount; i++)
        {
            if (TextNum.transform.GetChild(i).tag.Equals(strtag)) //text와 클릭한 애 태그 같으면
            {
                num = int.Parse(TextNum.transform.GetChild(i).GetComponent<Text>().text);
            }
        }

        if (num == count) //맞음
        {
            PlaySound("Right");
            count = count + 1;
            Number.text = count.ToString();
        }
        else //틀림
        {
            WrongCount++;
            PlaySound("Wrong");
        }

        if (count == 16) //16번 시 게임 종료(결과)
        {
            GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true); //결과화면

            Result.text = "" + time.ToString("0.0"); //결과text

            //data 보내기
            DataForm data = new DataForm();
            data.Time = time;
            data.Wrong = WrongCount;
            data.PatientName = GlobalValue.name;
            data.PatientBirth = GlobalValue.birth;
            if(SceneManager.GetActiveScene().name.Equals("NumberPuzzle"))
            {
                HTTPManager.Http.postUpLoad(data);   
            }
        }
    }

    void PlaySound(string snd) //사운드
    {
        GameObject.Find(snd).GetComponent<AudioSource>().Play();
    }

}
