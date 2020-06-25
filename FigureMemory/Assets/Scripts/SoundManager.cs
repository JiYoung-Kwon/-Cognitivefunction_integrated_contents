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
    public AudioClip card_sound;
    AudioSource myAudio;
    public AudioSource CardAudio;
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
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(CardAudio);
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

    public void CardPlace() //카드 나눠주는 소리
    {
        CardAudio.PlayOneShot(card_sound);
    }
}
