using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 일시정지 패널 관련 스크립트
 */

public class StopPanel : MonoBehaviour
{

    public void StopButtonClick() //일시정지
    {
        PlaySound("Click");
        GameObject.Find("Canvas").transform.Find("StopPanel").gameObject.SetActive(true);
    }

    public void HomeButtonClick() //홈으로
    {
        PlaySound("Click");
        SceneManager.LoadScene("StartScene");
    }

    public void RestartClick() //다시시작
    {
        PlaySound("Click");
        SceneManager.LoadScene("NumberPuzzle");
    }

    public void ContinueButtonClick() //계속하기
    {
        PlaySound("Click");
        gameObject.SetActive(false);
    }

    void PlaySound(string snd) //사운드
    {
        GameObject.Find(snd).GetComponent<AudioSource>().Play();
    }
}
