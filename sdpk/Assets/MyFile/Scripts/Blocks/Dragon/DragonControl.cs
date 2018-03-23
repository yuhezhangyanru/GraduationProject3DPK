using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;

public class DragonControl : MonoBehaviour {

    private Animation dragonAnimation;
    private int hitCount = 0;
	// Use this for initialization
	void Start () {
        dragonAnimation = this.gameObject.GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
	
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
                //ToolsFunction.Log(this.gameObject.name, "人打斗了。当前龙碰到人了");
                if (hitCount == 3)   //打够了3次才会把龙打死
                {
                    dragonAnimation.Play("fly death");
                }
                else
                {        
                    State.KEEP_IDLE = true;
                    other.transform.position -= 0.1f * ToolsFunction.Toward(other.gameObject);
                }
                hitCount++;
            }
            else
            {
                State.KEEP_IDLE = true;
                other.gameObject.GetComponent<Animation>().Play(State.ANIM_DEMAGE); //播放受打击的动作
                other.transform.position -= 0.1f * ToolsFunction.Toward(other.gameObject);
                ToolsFunction.AddScore(-DataConst.SCORE_DRAGON / 2);
            }
        }
    }

    /// <summary>
    /// 死亡动画播放反比的回调函数
    /// </summary>
   private void DeathCallback()
    {
       //显示死亡的特效
       EMonsterDie.StartPlay(this.gameObject.transform.localPosition + new Vector3(0, 1.5f, 0));
       this.GetComponent<BoxCollider>().enabled = false;   //禁用触发器
       //ObjectManager.HindObj(DataConst.PRE_DRAGON);  //隐藏对象当作销毁
       State.KEEP_IDLE = false;  //静止状态复位
       ToolsFunction.AddScore(DataConst.SCORE_DRAGON);//加分
       hitCount = 0;
       Destroy(this.gameObject);  //最后销毁对象
    }
}


