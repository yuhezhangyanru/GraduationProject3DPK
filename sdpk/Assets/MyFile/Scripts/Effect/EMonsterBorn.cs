using UnityEngine;
using System.Collections;

public class EMonsterBorn : MonoBehaviour {

    private static GameObject animationObj = null;//脚本和animation同级绑定的那个对象
    private static GameObject monsterObj = null;  //怪物对象

    void Awake()
    {
        animationObj = this.transform.FindChild("MonsterBorn").gameObject;
    }


    /// <summary>
    /// 指定动画播放完毕后要出现的怪物
    /// </summary>
    /// <param name="go"></param>
    public static void SetMonster(GameObject go)
    {
        animationObj.SetActive(true);
        monsterObj = go;
    }


    /// <summary>
    /// 怪物出生动画中火光结束之后的回调函数
    /// </summary>
    private void LightEndCallbback()
    {
        Debug.Log(Time.time + "light end callback!");
        animationObj.SetActive(false);
        if (monsterObj)
            monsterObj.SetActive(true); 
    }
}
