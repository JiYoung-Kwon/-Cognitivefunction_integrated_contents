using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 점수 정보 스크립트
 */

public class ScoreStatisticsData
{
    public int UserID { get; set; }
    public string Date { get; set; }
    public string Game { get; set; }
    public string Type { get; set; }
    public float Score { get; set; }
}

public class ScoreStatisticsDbData : ScoreStatisticsData
{
    public int Level { get; set; }
    public string etc { get; set; }
}

public class ETC
{
    public string RestTime { get; set; }
    public int Wrong { get; set; }
}