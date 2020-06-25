using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 씬 관련 스크립트
 */

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    public void StartClick() //시작
    {
        PlaySound("Click");
        SceneManager.LoadScene("NumberPuzzle");
        Time.timeScale = 0;
    }

    public void PracticeClick() //튜토리얼
    {
        PlaySound("Click");
        SceneManager.LoadScene("Tutorial");
        Time.timeScale = 0;
    }

    public void ExitClick() //나가기
    {
        PlaySound("Click");
        Application.Quit();
    }

    void PlaySound(string snd) //사운드
    {
        GameObject.Find(snd).GetComponent<AudioSource>().Play();
    }
}
