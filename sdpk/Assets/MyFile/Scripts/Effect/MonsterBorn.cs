///author:Yanru Zhang
///date:2014-4-1 23:41:53
///fucntion: bined to MonsterBorn->light, when effect play end, callback monster show

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;

public class MonsterBorn : Singleton<MonsterBorn> {

    private GameObject monsterObj = null; //表示怪物的对象，特效播放完会用

    public void SetMonster(GameObject go)
    {
        monsterObj = go;
        Debug.Log(Time.time + " MonsterBorn is: " + monsterObj.name);
    }

    /// <summary>
    /// 怪物出生动画中火光结束之后的回调函数
    /// </summary>
    private void LightEndCallbback()
    {
        Debug.Log(Time.time + "light end callback!");
        //回调结束，开始产生怪物
        //ToolsFunction.MonsterBornCallBack();
        monsterObj.SetActive(true);
    }
}
