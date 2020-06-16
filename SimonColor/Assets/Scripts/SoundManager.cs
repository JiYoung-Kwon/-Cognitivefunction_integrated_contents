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
    public AudioClip[] bell;
    AudioSource myAudio;

    public AudioSource BGMSource;
    public AudioClip BGMClip;
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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Main")) //main씬이면
        {
            //BGM재생
            BGMSource.clip = BGMClip;
            BGMSource.loop = true;
            BGMSource.Play();
        }
        myAudio = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }
    public void Btn_Click() //버튼음
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
    public void Bell(int i) //벨소리(패널 점등 소리)
    {
        switch (i)
        {
            case 0:
                myAudio.PlayOneShot(bell[0]);
                break;
            case 1:
                myAudio.PlayOneShot(bell[1]);
                break;
            case 2:
                myAudio.PlayOneShot(bell[2]);
                break;
            case 3:
                myAudio.PlayOneShot(bell[3]);
                break;
            default:
                break;
        }
    }
}
