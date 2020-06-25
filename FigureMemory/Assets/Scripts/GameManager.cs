using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region 변수
    public GameObject LCard;
    public GameObject RCard;
    public GameObject Target;
    public GameObject TouchOff;
    public GameObject[] Life = new GameObject[3];

    public Sprite[] BackImage;
    public Sprite[] BackImage2;
    public Sprite[] BackImage3;
    public Sprite OriginImage;

    Vector3 CardOriginPosition = new Vector3();
    public List<int> order = new List<int>();
    public List<bool> Continuity = new List<bool>();
    public int ChangeNum;
    public int life;
    public int stage;

    public int LCardNum;
    public int RCardNum;
    public int LCardNum2;
    public int RCardNum2;

    public bool IsSame;
    #endregion

    #region Singleton
    private static GameManager _gameManager;
    public static GameManager Game
    {
        get { return _gameManager; }
    }

    void Awake()
    {
        _gameManager = GetComponent<GameManager>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CardOriginPosition = LCard.transform.position; //카드 위치 저장

        //초기화
        stage = 1;
        life = 2;
    }

    public IEnumerator CardMove() //카드 이동 함수
    {
        ChangeNum++;
        TouchOff.SetActive(true);
        //초기화부분
        //다시 시작되면 RCard의 이미지가 LCard에 복사되서 왼쪽으로 가고 RCard는 새로 랜덤

        LCard.transform.position = CardOriginPosition; //처음포지션으로 가야지
        LCard.GetComponent<Image>().sprite = RCard.GetComponent<Image>().sprite; //R카드 복사
        RCard.GetComponent<Image>().sprite = OriginImage;

        //다시 왼쪽으로 가고 오른쪽 카드는 랜덤   
        SoundManager.instance.CardPlace();
        LeanTween.move(LCard, Target.transform.position, 1.0f).setEase(LeanTweenType.easeOutQuint); //카드 이동
        yield return new WaitForSeconds(1.0f);

        if (LCard.transform.eulerAngles.y == 180f || LCard.transform.eulerAngles.y == -180f) //카드 뒤집기 효과
        {
            LeanTween.rotateY(LCard, 0f, 0.2f).setEase(LeanTweenType.linear);
            LeanTween.rotateY(RCard, 0f, 0.2f).setEase(LeanTweenType.linear);
        }
        else
        {
            LeanTween.rotateY(LCard, 180f, 0.2f).setEase(LeanTweenType.linear);
            LeanTween.rotateY(RCard, 180f, 0.2f).setEase(LeanTweenType.linear);
        }
        yield return new WaitForSeconds(0.1f);

        if (SceneChangeManager.SCENE.Level == 0) //색+모양
        {
            RCardNum = Random.Range(0, 12);

            Percentage();

            order.Add(RCardNum);

            LCard.GetComponent<Image>().sprite = OriginImage;  //왼쪽 뒤집히고 모양
            RCard.GetComponent<Image>().sprite = BackImage[RCardNum]; //오른쪽 새로운 모양
        }
        else if (SceneChangeManager.SCENE.Level == 1) //모양조합
        {
            RCardNum2 = Random.Range(0, 81);

            Percentage();

            order.Add(RCardNum2);

            LCard.GetComponent<Image>().sprite = OriginImage;  //왼쪽 뒤집히고 모양
            RCard.GetComponent<Image>().sprite = BackImage2[RCardNum2]; //오른쪽 새로운 모양
        }

        TouchOff.SetActive(false);
    }

    public IEnumerator InitCard() //처음 카드 보낼때
    {
        TouchOff.SetActive(true);

        ChangeNum++;

        //랜덤 이미지 왼쪽
        if (SceneChangeManager.SCENE.Level == 0) //색+모양
        {
            LCardNum = Random.Range(0, 12);
            order.Add(LCardNum);
            LCard.GetComponent<Image>().sprite = BackImage[LCardNum];

            //랜덤 이미지2 (오른쪽)
            RCardNum = Random.Range(0, 12);
            order.Add(RCardNum);

        }
        else if (SceneChangeManager.SCENE.Level == 1)//모양 조합
        {
            LCardNum2 = Random.Range(0, 81);
            order.Add(LCardNum2);
            LCard.GetComponent<Image>().sprite = BackImage2[LCardNum2];

            //랜덤 이미지2 (오른쪽)
            RCardNum2 = Random.Range(0, 81);
            order.Add(RCardNum2);

        }

        //이동
        SoundManager.instance.CardPlace();
        LeanTween.move(LCard, Target.transform.position, 1.0f).setEase(LeanTweenType.easeOutQuint);
        yield return new WaitForSeconds(1.0f);

        LeanTween.rotateY(LCard, 180f, 0.2f).setEase(LeanTweenType.linear);
        LeanTween.rotateY(RCard, 180f, 0.2f).setEase(LeanTweenType.linear);
        yield return new WaitForSeconds(0.1f);
        LCard.GetComponent<Image>().sprite = OriginImage;  //왼쪽 뒤집히고 모양

        if (SceneChangeManager.SCENE.Level == 0) //색+모양
            RCard.GetComponent<Image>().sprite = BackImage[RCardNum]; //오른쪽 새로운 모양
        else if (SceneChangeManager.SCENE.Level == 1) //모양조합
            RCard.GetComponent<Image>().sprite = BackImage2[RCardNum2];

        if (SceneChangeManager.SCENE.NBack == 2) //nBack 난이도
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("CardMove");
        }
        else
        {
            TouchOff.SetActive(false);
        }
    }

    public void ClickRight() //O버튼
    {

        if (order[ChangeNum] == order[ChangeNum - SceneChangeManager.SCENE.NBack]) // NBack 맞을 경우
        {
            SoundManager.instance.Success();
            Debug.Log("맞음 - 같음");
            stage++;

            IsSame = true; //같음 나옴


        }
        else //틀릴 경우
        {
            SoundManager.instance.Failure();
            Debug.Log("틀림 - 다름");
            Life[life].SetActive(false);
            life--;

            if (life < 0)
            {
                GameOver();
            }

            IsSame = false; //다름 나옴

        }

        Continuity.Add(IsSame);

        StartCoroutine("CardMove");
    }

    public void ClickWrong() //X버튼
    {
        if (order[ChangeNum] != order[ChangeNum - SceneChangeManager.SCENE.NBack]) // NBack 다를 경우
        {
            SoundManager.instance.Success();
            Debug.Log("맞음 - 다름");
            stage++;

            IsSame = false; //다름 나옴
        }
        else //틀릴 경우
        {
            SoundManager.instance.Failure();
            Debug.Log("틀림 - 같음");
            Life[life].SetActive(false);
            life--;

            if (life < 0)
            {
                GameOver();
            }

            IsSame = true; //같음 나옴
        }

        Continuity.Add(IsSame);

        StartCoroutine("CardMove");
    }

    public void Percentage() //연속된 여부에 따라 같은거 나올지 다른 거 나올지 따짐 (카드 종류가 많아 다름이 너무 자주 나오는 문제 해결 방법)
    {
        if (Continuity.Count > 2)
        {
            for (int i = 2; i < Continuity.Count; i++)
            {
                if (Continuity[i] == Continuity[i - 1] && Continuity[i - 1] == Continuity[i - 2] && Continuity[i] == Continuity[i - 2])
                {
                    //같음 연속일 경우 -> 다름 나오게.. 다름 연속일 경우 -> 같음 나오게
                    if (Continuity[i] == true) //같음 연속일 경우
                    {

                    }
                    else //다름 연속일 경우
                    {
                        if (SceneChangeManager.SCENE.NBack == 1)
                        {
                            RCardNum = order[order.Count - 1]; //마지막 인덱스의 값과 동일하게
                            RCardNum2 = order[order.Count - 1];
                        }
                        else
                        {
                            RCardNum = order[order.Count - 2];
                            RCardNum2 = order[order.Count - 2];
                        }


                    }
                    Continuity.Clear(); //초기화

                }
            }
        }
    }

    public void GameOver() //게임 종료
    {
        GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true);
        UIManager.UI.ShowResult();
    }


}
