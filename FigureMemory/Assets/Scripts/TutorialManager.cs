using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    #region 변수
    public GameObject LCard;
    public GameObject RCard;
    public GameObject Target;
    public GameObject TouchOff;
    public GameObject[] Tuto = new GameObject[6];

    public Sprite[] BackImage;
    public Sprite OriginImage;

    Vector3 CardOriginPosition = new Vector3();

    public List<int> order = new List<int>();

    public int ChangeNum;
    public int tutoNum;

    public int LCardNum;
    public int RCardNum;

    public int correct;
    public int TwoBack;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CardOriginPosition = LCard.transform.position;
        Time.timeScale = 1f;
    }

    public void OkayButton() //확인 버튼
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

            StartCoroutine("InitCard");
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
            StartCoroutine("CardMove");
            tutoNum = 3;
        }
    }

    public IEnumerator CardMove() //카드 이동함수
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
        LeanTween.move(LCard, Target.transform.position, 1.0f).setEase(LeanTweenType.easeOutQuint);
        yield return new WaitForSeconds(1.0f);

        if (LCard.transform.eulerAngles.y == 180f || LCard.transform.eulerAngles.y == -180f) //카드 회전 설정
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

        RCardNum = Random.Range(0, 12);

        order.Add(RCardNum);

        LCard.GetComponent<Image>().sprite = OriginImage;  //왼쪽 뒤집히고 모양
        RCard.GetComponent<Image>().sprite = BackImage[RCardNum]; //오른쪽 새로운 모양

        TouchOff.SetActive(false);

        if (correct == 1 && TwoBack == 0)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("CardMove");
            TwoBack++;
        }
        else
        {
            GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true);
            Tuto[4].SetActive(false);
            Tuto[5].SetActive(false);
            Tuto[3].SetActive(true);
        }
    }

    public IEnumerator InitCard() //처음 카드 보낼때
    {
        TouchOff.SetActive(true);

        ChangeNum++;

        //랜덤 이미지 왼쪽

        LCardNum = Random.Range(0, 12);
        order.Add(LCardNum);
        LCard.GetComponent<Image>().sprite = BackImage[LCardNum];

        //랜덤 이미지2 (오른쪽)
        RCardNum = Random.Range(0, 12);
        order.Add(RCardNum);


        //이동
        SoundManager.instance.CardPlace();
        LeanTween.move(LCard, Target.transform.position, 1.0f).setEase(LeanTweenType.easeOutQuint);
        yield return new WaitForSeconds(1.0f);

        LeanTween.rotateY(LCard, 180f, 0.2f).setEase(LeanTweenType.linear);
        LeanTween.rotateY(RCard, 180f, 0.2f).setEase(LeanTweenType.linear);
        yield return new WaitForSeconds(0.1f);
        LCard.GetComponent<Image>().sprite = OriginImage;  //왼쪽 뒤집히고 모양
        RCard.GetComponent<Image>().sprite = BackImage[RCardNum]; //오른쪽 새로운 모양

        TouchOff.SetActive(false);

        if (correct == 1)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("CardMove");

        }
        else
        {
            GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true);
            Tuto[4].SetActive(false);
            Tuto[5].SetActive(false);
            Tuto[3].SetActive(true);
        }
    }


    public void ClickRight() //O 클릭
    {
        if (order[ChangeNum] == order[ChangeNum - SceneChangeManager.SCENE.NBack]) //맞음
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
            TwoBack = 0;

        }
        TouchOff.SetActive(true);
        //패널 올리기
        GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true);
    }

    public void ClickWrong() //X 클릭
    {
        if (order[ChangeNum] != order[ChangeNum - SceneChangeManager.SCENE.NBack]) //틀림
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
            TwoBack = 0;
        }
        TouchOff.SetActive(true);
        GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(true);
    }

    public void ShowResult() //결과 화면
    {
        GameObject.Find("Canvas").transform.Find("TutorialPanel").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true);
    }

    public void PauseButton() //일시정지
    {
        SoundManager.instance.Btn_Click();
        Time.timeScale = 0f;
    }

    public void KeepGoing() //계속하기
    {
        SoundManager.instance.Btn_Click();
        Time.timeScale = 1f;
    }

    public void HomeButton() //홈으로
    {
        SoundManager.instance.Btn_Click();
        SceneChangeManager.SCENE.MainMenu();
    }

    public void Restart() //다시시작
    {
        SoundManager.instance.Btn_Click();
        SceneChangeManager.SCENE.Tutorial();
    }
}
