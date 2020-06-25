using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 스크린 매니저 스크립트
 */

public class ScreenManager : MonoBehaviour {
	// Use this for initialization
	void Start () {
        Screen.SetResolution(Screen.width, Screen.width * 1080 / 1920, true); //유니티 해상도 설정

        #region 주석
        /*Screen.sleepTimeout = SleepTimeout.NeverSleep;
        float targetWidthAspect = 16f;
        float targetHeightAspect = 9f;

        Camera mainCamera=Camera.main;;
        mainCamera.aspect = targetWidthAspect / targetHeightAspect;

        float widthRatio = (float)Screen.width / targetWidthAspect;
        float heightRatio = (float)Screen.height / targetHeightAspect;

        float heightadd = ((widthRatio / (heightRatio / 100)) - 100) / 200;
        float widthadd=((heightRatio/(widthRatio/100))-100)/200;

        if (heightRatio > widthRatio)
            widthadd = 0f;
        else
            heightadd = 0f;
        mainCamera.rect = new Rect(
            mainCamera.rect.x + Math.Abs(widthadd),
            mainCamera.rect.y + Math.Abs(heightadd),
            mainCamera.rect.width + (widthadd * 2),
            mainCamera.rect.height + (heightadd * 2));*/
        #endregion
    }
}
