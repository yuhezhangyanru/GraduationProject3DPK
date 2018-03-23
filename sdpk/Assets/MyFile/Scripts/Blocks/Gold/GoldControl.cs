///author:Yanru Zhang
///date:2014-2-23 1:15:49
///function: binded to each gold coin, Collision to add score for MeDisplay

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;

public class GoldControl : MonoBehaviour {

    public Animation GoldAnimation = null;  

    //绑定在对象的脚本，如果该对象是动态生成的，一定要变量初始化！, 同时避免使用关键字，如animation作为对象的定义
    /// <summary>
    /// 用于金币的初始化，主要是animation
    /// </summary>
    public void Start()
    {
        GoldAnimation = this.gameObject.GetComponent<Animation>();
    }

    /// <summary>
    /// 绑定在金币上的函数
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if("MeDisplay(Clone)" == other.gameObject.name)
        {
            GoldAnimation.Play("gold_fly_1");  //播放飞金币的动画
        }
    }

    public void DestroyMe()
    {
        //销毁自己
        ToolsFunction.AddScore(DataConst.SCORE_GOLD);
        Destroy(this.gameObject);
        //ObjectManager.HindObj(DataConst.PRE_GOLD);   //隐藏对象当作销毁
        Debug.Log("当前的分数: " + DataConst.SCORE_TOTAL);
    }
}
