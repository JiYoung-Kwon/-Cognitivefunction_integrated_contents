using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 랜덤숫자 관련 스크립트
 */

public class RandomNumber : MonoBehaviour
{
    public Text[] texts;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        RandomNum();
    }

    public void RandomNum() //랜덤숫자 함수
    {
        int[] randArray = new int[15];
        bool isSame;

        for (int i = 0; i < 15; i++)
        {
            while (true) //중복방지
            {
                randArray[i] = Random.Range(0, 15);
                isSame = false;

                for (int j = 0; j < i; j++)
                {
                    if (randArray[j] == randArray[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }

        for (index = 0; index < 15; index++) //text에 넣어주기
        {
            texts[index].text = (randArray[index] + 1).ToString();
        }
    }
}
