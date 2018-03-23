///author:Yanru Zhang
///date:2014-2-23
///function: binded to monster, if MeDisplay hit it,  then destroy

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;

public class MonsterControl : MonoBehaviour {

    private Animator monsterAnimator;
    
    //Animator MeAnimator;
    /// <summary>
    /// 初始化怪物的animator
    /// </summary>
    public void Start()
    {
        monsterAnimator = this.gameObject.GetComponent<Animator>();
    }

    
    /// <summary>
    /// 怪物的触发器，碰到人的时候，就让人静止了，但是如果人打斗的话，就可以解锁动作
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if ("MeDisplay(Clone)" == other.gameObject.name)
        {
  
            if (true == State.ON_FIGHT)
            {
                monsterAnimator.SetBool("die", true);
                //这里要播放一个捡金币的动画
            }
            else
            {
                State.KEEP_IDLE = true;
                other.transform.position -= 0.1f * ToolsFunction.Toward(other.gameObject);
                ToolsFunction.AddScore(-DataConst.SCORE_MONSTER / 2);
            }
        }
    }


    /// <summary>
    /// 怪物死亡动画播放完death动画的时候回调的函数
    /// 
    /// </summary>
    public void DeathCallback()
    {
        //用于怪物的死亡销毁对象
        Destroy(this.gameObject);  //销毁对象
        //ObjectManager.HindObj(DataConst.PRE_SPIDER);  //隐藏对象，当作销毁
        this.GetComponent<BoxCollider>().enabled = false;   //禁用触发器
        State.KEEP_IDLE = false;  //静止状态复位\
        ToolsFunction.AddScore(DataConst.SCORE_MONSTER);//加分
    }
}
