using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public int Level = 1;
    private static SceneChangeManager _sceneManager;
    public static SceneChangeManager SCENE
    {
        get { return _sceneManager; }
    }

    void Awake()
    {
        _sceneManager = GetComponent<SceneChangeManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MainMenu()
    {
        SoundManager.instance.Btn_Click();
        SceneManager.LoadScene("Main");
    }

    public void GameStart()
    {
        SoundManager.instance.Btn_Click();
        SceneManager.LoadScene("Game");
    }

    public void Tutorial()
    {
        SoundManager.instance.Btn_Click();
        SceneManager.LoadScene("Tutorial");
    }

    public void Exit()
    {
        SoundManager.instance.Btn_Click();
        Application.Quit();
    }

    public void Setting()
    {
        SoundManager.instance.Btn_Click();
    }

    public void LevelClick(int i)
    {
        Level = i;
    }
}
