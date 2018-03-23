///author:Yanru Zhang
///date:2014-3-16 14:16:27
///function:binded to count number, play animation callback

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;

public class CountPlay : MonoBehaviour {

    private UISprite BgNum = null;
    private Animator NumAnimator = null;   //数字的动画控制器

    public void Start()
    {
        BgNum = this.gameObject.GetComponent<UISprite>();
        NumAnimator = this.gameObject.GetComponent<Animator>();
        BgNum.spriteName = "num_3";  //重置图片，防止中途播放
        BgNum.enabled = true;
        NumAnimator.enabled = true;
    }


    private void CountPlayCallback()
    {
        //ToolsFunction.Log(this.gameObject, "当前的图片: " + BgNum.spriteName);
        switch (BgNum.spriteName)
        {
            case "num_3":
                {
                    //ToolsFunction.Log(this.gameObject, "当前的图e434片: " + BgNum.spriteName);
                BgNum.spriteName = "num_2";
                //BgNum.
            }break;
            case "num_2":
                {
                    BgNum.spriteName = "num_1";
            }break;
            case "num_1":
                {
                    BgNum.spriteName = "num_0";
            }break;
            case "num_0":{
                BgNum.enabled = false; 
                NumAnimator.enabled = false; //这样就不会继续播放动画了
                State.KEEP_IDLE = false;  //倒数完之后就开始让角色跑步
            } break;
        }
    }
}
