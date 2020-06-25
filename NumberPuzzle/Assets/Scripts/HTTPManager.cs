using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public static class GlobalValue
{
    public static string name;
    public static string birth;
}
public class DataForm
{
    public double Time { get; set; }
    public int Wrong { get; set; }
    public string PatientName { get; set; }
    public string PatientBirth { get; set; }
}
public class patientInformation
{
    public string name;
    public string birth;
    public ContentsLevel level;
}
public class ContentsLevel
{
    public int CardMatching_Level { get; set; }
    public int AscendingNum_Level { get; set; }
    public int Shopping_Level { get; set; }
    public int SimonOrder_Level { get; set; }
    public int TileMatching_Level { get; set; }
    public int TmtLine_Level { get; set; }
    public int TrafficLight_Level { get; set; }
    public int WordMatching_Level { get; set; }
    public int Mole_Level { get; set; }
}

public class HTTPManager : MonoBehaviour
{
    #region Singleton
    private static HTTPManager _instance;
    public static HTTPManager Http { get { return _instance; } }
    private void Awake()
    {
        _instance = GetComponent<HTTPManager>();
    }
    #endregion

    private const string ip = "localhost";
    private const int port = 57540;
    private string url = "http://" + ip + ":" + port + "/Contents/CC";   //여기서 Kon 은 앞에 서버코드에서 [Route() 로 정의해줬던거랑 이름을 맞춰야 합니다.]
    //private bool useNat = false;
    public int userLimit = 10;

    public void Start()
    {
        GetInfo();
    }

    public void postUpLoad(DataForm Data)
    {
        var data = JsonMapper.ToJson(Data);
        Debug.Log("data :  " + data);
        StartCoroutine(Upload(data));
    }

    private IEnumerator Upload(string str)
    {
        yield return new WaitForEndOfFrame();
        WWWForm form = new WWWForm();
        form.AddField("Data", str);
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();
    }
    public IEnumerator GetInfo()
    {
        yield return new WaitForEndOfFrame();
        UnityWebRequest uwr = UnityWebRequest.Get("localhost:57540/ServerConnect/GetPatientInfo");
        yield return uwr.SendWebRequest();
        var info = JsonMapper.ToObject<patientInformation>(uwr.downloadHandler.text);
        GlobalValue.name = info.name;
        GlobalValue.birth = info.birth;
        Debug.Log("info = " + info.birth + info.level + info.name + "  :  " + info.level.Mole_Level);
    }
}
