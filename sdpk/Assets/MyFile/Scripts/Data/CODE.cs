///author:Yanru Zhang
///date:2014-3-12 11:32:44
///function; base date code, used to update service code

using UnityEngine;
using System.Collections;

public class CODE : MonoBehaviour {
    public static readonly int UPDATE_SCORE = 0;  //分数有更新
    public static readonly int UPDATE_BLOCK = 1; //障碍物数目有更新
    public static readonly int UPDATE_PROCESS = 2;

    public static readonly int UPDATE_GAME_PROCESS = 3;  //更新游戏的进行阶段
}
