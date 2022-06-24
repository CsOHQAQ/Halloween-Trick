using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnxietyEnum
{
    NotAnxiety, //不焦虑
    MildAnxiety, //轻度焦虑
    ModerateAnxiety, //中度焦虑
    SeriousAnxiety, //重度焦虑
}

public enum BTBlockEnum
{
    NoBlock, //没有阻碍(正常情况下)
    OnChange, //中间态 用于判断是否应该更新 lastAnxiety
    OnBlock, //正在阻碍
}
