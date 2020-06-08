using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* 사운드 매니저 스크립트
*/

public class SoundManager : MonoBehaviour
{
    #region 변수
    public AudioClip btn_sound;
    public AudioClip success_sound;
    public AudioClip failure_sound;
    AudioSource myAudio;
    #endregion

    #region Singleton
    public static SoundManager instance;

    void Awake()
    {
        if (SoundManager.instance == null)
        {
            SoundManager.instance = this;
        }
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject); //씬 넘어가도 SoundManager 오브젝트 살리기
    }
    public void Btn_Click() //클릭 버튼음
    {
        myAudio.PlayOneShot(btn_sound);
    }
    public void Success() //성공 효과음
    {
        myAudio.PlayOneShot(success_sound);
    }
    public void Failure() //실패 효과음
    {
        myAudio.PlayOneShot(failure_sound);
    }
}
