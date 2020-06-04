using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip btn_sound;
    public AudioClip success_sound;
    public AudioClip failure_sound;
    public AudioClip dog_sound;
    public AudioClip[] bell;
    AudioSource myAudio;
    public static SoundManager instance;

    void Awake()
    {
        if (SoundManager.instance == null)
        {
            SoundManager.instance = this;
        }
    }
    // Use this for initialization
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }
    public void Btn_Click()
    {
        myAudio.PlayOneShot(btn_sound);
    }
    public void Success()
    {
        myAudio.PlayOneShot(success_sound);

    }
    public void Failure()
    {
        myAudio.PlayOneShot(failure_sound);
    }
    public void Dog()
    {
        myAudio.PlayOneShot(dog_sound);
    }
    public void Bell(int i)
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
    // Update is called once per frame
    void Update()
    {

    }
}
