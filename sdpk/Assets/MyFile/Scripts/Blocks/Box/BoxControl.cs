///date:2014-2-27

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;

public class BoxControl : MonoBehaviour {

    public Animator BoxAnimator = null;  //箱子的控制器
    //public Animator MeAnimator = null;  //我的控制器
    public GameObject golds = null; //= new GameObject();

    public void Start()
    {
        BoxAnimator = this.GetComponent<Animator>();  //宝箱的动画控制器
        Transform[] trans = this.GetComponentsInChildren<Transform>();
        foreach(Transform tran in trans)
        {
            if("goldCoins" == tran.name)  //是一堆金币
            {
                golds = tran.gameObject;   //此时金币是禁用的
                golds.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 绑定在宝箱上的触发器，执行了动作就打开宝箱
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if("MeDisplay(Clone)" == other.gameObject.name)
        {
            State.KEEP_IDLE = true;
            if (true == State.ON_FIGHT)
            {
                BoxAnimator.SetBool("die", true);
                //这里要播放一个捡金币的动画
                this.GetComponent<BoxCollider>().enabled = false;   //禁用触发器
                MeController.SetAction(other.gameObject.GetComponent<Animation>(), State.ANIM_PICK_UP); //播放捡东西的动作
            }
            else
            {
                other.transform.position -= 0.1f * ToolsFunction.Toward(other.gameObject);
                ToolsFunction.AddScore(-DataConst.SCORE_BOX / 2); //加分
            }
        }
    }

    /// <summary>
    /// 箱子被打击，播放完死亡动作后回调的函数
    /// </summary>
    public void  DeathCallBack()
    {
        Debug.Log("Box  的回调函数被调用了");
        golds.SetActive(true);   //激活金币堆
        State.KEEP_IDLE = false;
        //分数增加
        ToolsFunction.AddScore(DataConst.SCORE_BOX);
        Debug.Log("当前的分数： " + DataConst.SCORE_TOTAL);
    }
}
