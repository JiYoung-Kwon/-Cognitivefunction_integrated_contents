using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 환자 정보 스크립트
 */

public class PatientInfo
{
    public int UserID { get; set; }
    public string Name { get; set; }
    public ContentsLevel Level { get; set; }
    public string Tag { get; set; }
}

public class ContentsLevel
{
    public List<ContentLevel> Contents;
}
public class ContentLevel
{
    public string Game { get; set; }
    public int Level { get; set; }
}
