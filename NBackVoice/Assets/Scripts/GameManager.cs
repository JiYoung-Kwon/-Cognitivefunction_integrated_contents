using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region 변수
    public AudioClip[] Sound;
    AudioSource Audio;

    public int ChangeNum;
    public int stage;

    public List<int> order = new List<int>();

    public GameObject[] Life = new GameObject[3];
    public GameObject TouchOff;
    public int life;
    public int HardStage = 0;

    public Text LevelText;
    #endregion

    #region Singleton
    private static GameManager _gManager;
    public static GameManager Game
    {
        get { return _gManager; }
    }

    void Awake()
    {
        _gManager = GetComponent<GameManager>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();

        LevelText.text = SceneChangeManager.SCENE.Level + " Back"; //n Back 텍스트 설정

        //stage&life 초기화
        stage = 1;
        life = 2;

        if (SceneChangeManager.SCENE.Level == 3)
        {
            HardStage = 1;
        }
        Init();
    }

    public void Init() //초기화 코루틴 실행
    {
        StartCoroutine("InitSound");
    }

    IEnumerator InitSound() //Sound 초기화
    {
        for (int firstChange = 0; firstChange < 3 + HardStage; firstChange++)
        {
            int a = Random.Range(0, 12); //sound 12개 중 랜덤
            order.Add(a);
            ChangeNum++;

            yield return new WaitForSeconds(1f);

            Audio.PlayOneShot(Sound[a]);

            yield return new WaitForSeconds(1f);

        }
        TouchOff.SetActive(false);
    }

    IEnumerator RandomSound() //두번째부터
    {
        int a = Random.Range(0, 2); //0 : 동일한 음성, 1 : 랜덤 음성
        if (a == 0)
        {
            a = order[ChangeNum - SceneChangeManager.SCENE.Level]; //n번째 전 음성과 동일한 음성
        }
        else
        {
            a = Random.Range(0, 12); //랜덤 음성
        }
        order.Add(a);
        ChangeNum++;

        yield return new WaitForSeconds(0.5f);

        Audio.PlayOneShot(Sound[a]);

        yield return new WaitForSeconds(1f);

        TouchOff.SetActive(false);
    }

    //1Back이면 바로 직전 값 비교
    public void ClickRight() //O를 클릭했을 경우
    {
        if (order[ChangeNum - 1] == order[ChangeNum - 1 - SceneChangeManager.SCENE.Level]) //맞음
        {
            SoundManager.instance.Success(); //맞음사운드
            Debug.Log("맞음");
            stage++;
            StartCoroutine("RandomSound");
        }
        else //틀림
        {
            SoundManager.instance.Failure(); //틀림사운드
            Debug.Log("틀림");

            //틀린경우 라이프 감소
            Life[life].SetActive(false);
            life--;

            //life == 0이 되면 게임오버
            if (life < 0)
            {
                GameOver();
            }

            StartCoroutine("RandomSound");
        }
        TouchOff.SetActive(true);

    }

    public void ClickWrong() //X를 클릭했을 경우
    {
        if (order[ChangeNum - 1] != order[ChangeNum - 1 - SceneChangeManager.SCENE.Level]) //맞음
        {
            Debug.Log("맞음");
            SoundManager.instance.Success();
            stage++;
            StartCoroutine("RandomSound");
        }
        else //틀림
        {
            SoundManager.instance.Failure();
            Debug.Log("틀림");
            Life[life].SetActive(false);
            life--;

            if (life < 0)
            {
                GameOver();
                Time.timeScale = 0f;
            }

            StartCoroutine("RandomSound");
        }
        TouchOff.SetActive(true);
    }

    public void GameOver() //게임오버+결과화면 함수
    {
        GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true); //결과화면 띄우기
        UIManager.UI.ShowResult(); //결과값 설정
    }
}
