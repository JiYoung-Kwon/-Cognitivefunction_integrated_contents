using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 씬 변환 매니저 -> 각 버튼 OnClick()에서 GameManager Object를 통해 함수 실행
 */

public class SceneChangeManager : MonoBehaviour
{
    public int Level = 1;

    #region Singleton
    private static SceneChangeManager _sceneManager;
    public static SceneChangeManager SCENE
    {
        get { return _sceneManager; }
    }

    void Awake()
    {
        _sceneManager = GetComponent<SceneChangeManager>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;
    }

    public void MainMenu() //메인씬 로드 + 버튼음
    {
        SoundManager.instance.Btn_Click();
        SceneManager.LoadScene("Main");
    }

    public void GameStart() //게임씬 로드 + 버튼음
    {
        SoundManager.instance.Btn_Click();
        SceneManager.LoadScene("Game");
    }

    public void Tutorial() //튜토리얼씬 로드 + 버튼음
    {
        SoundManager.instance.Btn_Click();
        SceneManager.LoadScene("Tutorial");
    }

    public void Exit() //게임종료 + 버튼음
    {
        SoundManager.instance.Btn_Click();
        Application.Quit();
    }

    public void Setting() //세팅 버튼음
    {
        SoundManager.instance.Btn_Click();
    }

    public void LevelClick(int i)  //레벨 설정 (설정에 있는 각 버튼 OnClick()에 i 값 넣어두었음)
    {
        Level = i;
    }
}
