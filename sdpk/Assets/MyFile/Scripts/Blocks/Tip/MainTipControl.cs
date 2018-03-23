///author:Yanru Zhang
///date:2014-3-18
///function:binded to main view tip, once meet block, it will work

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;

public class MainTipControl : MonoBehaviour {

    private UISprite TipIcon;
    private Animator TipAnimator;

    /// <summary>
    /// 初始化tip需要用到的组件
    /// </summary>
	void Start () {
        TipIcon = this.gameObject.GetComponent<UISprite>();
        TipAnimator = this.gameObject.GetComponent<Animator>();

        //开始的时候表示消息提示不可用
        TipIcon.enabled = false;
        TipAnimator.enabled = false;
	}

    /// <summary>
    /// tips的动画播放结束之后的回调函数
    /// </summary>
    private void TipAnimEndCallback()
    {
        ToolsFunction.Log(this.gameObject, "动画已经播放完毕了！");
        TipIcon.enabled = false;
        TipAnimator.enabled = false;
    }


    /// <summary>
    /// 开始播放动画，static，mainview中调用
    /// </summary>
    /// <param name="tip">提示图标</param>
    /// <param name="blockHead">障碍物的头像</param>
    public static void StartAnim(UISprite tip, UISprite blockHead)
    {

        foreach (string str in DataConst.BlockName)
        {
            if(DataConst.NAME_BLOCK.Contains(str))
            {
                DataConst.NAME_BLOCK = str;
                break;
            }
        }
        Debug.Log("碰到的障碍物的名称： " + DataConst.NAME_BLOCK + "   --" + Time.time);
        blockHead.spriteName = DataConst.NAME_BLOCK + "_1";
        if (DataConst.NAME_BLOCK.Contains("gold"))
        {
            tip.spriteName = "good_tip";
        }
        else if (DataConst.NAME_BLOCK.Contains("Rock") || DataConst.NAME_BLOCK.Contains("Trap"))
        {
            tip.spriteName = "jump_tip";
        }
        else if (DataConst.NAME_BLOCK.Contains("area"))
        {
            tip.spriteName = "jump_tip";
        }
        else
        {
            tip.spriteName = "block_tip";
        }
        tip.enabled = true;
        tip.GetComponent<Animator>().enabled = true;
    }
}
