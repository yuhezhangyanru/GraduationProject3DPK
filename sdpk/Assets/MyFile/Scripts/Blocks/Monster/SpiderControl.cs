///author:Yanru Zhang
///date:2014-3-6 23:55:41
///function: binded to monster spider, mainly control animation

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;

public class SpiderControl : MonoBehaviour {

    private Animation SpiderAnimation = null;   //蜘蛛的动画组件
    private Animation RoleAnimation = null;   //主角的动画控制器
    //private bool hited = false;  //怪物是否受击
    public int nAnimIndex = 0;   //动画播放的索引
    public int nAttack = 0;

	// Use this for initialization
	void Start () {
        SpiderAnimation = this.gameObject.GetComponent<Animation>();
        //开始播放动画
        SpiderAnimation.Play(MonsterAnim.AnimNormal[0]);
        nAnimIndex = 0; //初始化    	
	}



    /// <summary>
    /// 怪物的触发器，碰到人的时候，就让人静止了，但是如果人打斗的话，就可以解锁动作
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if ("MeDisplay(Clone)" == other.gameObject.name)
        {
            ToolsFunction.Log(this.gameObject, "Spider现在撞到人了");
            nAnimIndex = 1000;  //设定一个值很大的量表示进入战斗状态, 只要碰到了人就开始战斗
            RoleAnimation = other.gameObject.GetComponent<Animation>();
            int i = 0;
           
            if (true == State.ON_FIGHT)
            {
                this.GetComponent<BoxCollider>().enabled = false;   //禁用触发器
                ToolsFunction.Log(this.gameObject, "执行次数：" + i);

                i++;
                SpiderAnimation.Play(MonsterAnim.AnimFight);
            }
            else
            {
                State.KEEP_IDLE = true;
                //我还没有攻击就撞到了蜘蛛，这个时候要播放被大的动画，毁掉函数中继续播放攻击动作
                //!!!!!!!!!!!
                other.gameObject.GetComponent<Animation>().Play(State.ANIM_DEMAGE); //播放受打击的动作
                other.transform.position -= 0.1f * ToolsFunction.Toward(other.gameObject);
                ToolsFunction.AddScore(-DataConst.SCORE_SPIDER / 2);
            }
        }
    }


    /// <summary>
    /// 怪物的普通动作完毕之后的回调函数，默认回调下一个动画
    /// </summary>
    //public void ActionEndCallback()
    //{
    //    Debug.Log("Spider的一个动画已经播放完了");
    //    //第一次被执行是idle动画完毕后
    //    if (nAnimIndex < MonsterAnim.AnimNormal.Length)
    //    {
    //        ToolsFunction.Log(this.gameObject, "要播放Spider下一个normal动作");
    //        int n = Random.Range(0, MonsterAnim.AnimNormal.Length - 1);
    //        SpiderAnimation.Play(MonsterAnim.AnimNormal[n]);
    //        //nAnimIndex++;
    //    }
    //    else  //表示撞到了，开始播放第一个攻击动作
    //    {
    //        ToolsFunction.Log(this.gameObject, "准备播放第一个受击动作");
    //        SpiderAnimation.Play(MonsterAnim.AnimAttack[0]);
    //    }
    //}


    /// <summary>
    /// 怪物攻击动作完毕之后的回调函数
    /// </summary>
    public void AttackEndCallback()
    {
        if (nAttack >= MonsterAnim.AnimAttack.Length)  //表示所有攻击动画已经播放完毕，播放死亡动画
        {
            nAttack = 0;  //置0，重新开始播放打斗序列动画
        }
        else
        {
            ToolsFunction.Log(this.gameObject, "Spider下一个打斗动作");
            SpiderAnimation.Play(MonsterAnim.AnimAttack[nAttack]);
            nAttack++;
        }
    }

    /// <summary>
    /// 蜘蛛在被人打之后执行的动作，执行这个动作后会调用蜘蛛的死亡动画
    /// </summary>
    public void FightCallback()
    {
        SpiderAnimation.Play(MonsterAnim.AnimDeath);
    }

    /// <summary>
    /// 死亡动画播放后的回调函数
    /// </summary>
    public void DeathCallback()
    {
        Debug.Log("蜘蛛要死亡了！");
        Destroy(this.gameObject);
        State.KEEP_IDLE = false;
        RoleAnimation.Play(State.ANIM_RUN);  //恢复跑步状态

        ToolsFunction.AddScore(DataConst.SCORE_SPIDER);
        //攻击动画要复位
    }

}
