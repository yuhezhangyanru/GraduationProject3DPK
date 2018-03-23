///author:Yanru Zhang
///date:2014-4-4 22:36:50
///function:show When game over, show award!

using UnityEngine;
using System.Collections;
using myfiles.scripts.ui;
using myfiles.scripts.ui.mainview;

public class AwardView : BaseView
{
    private Button btnClose = null;         //关闭结束界面的按钮
    private UILabel cost_time = null;      //花费的时间00：00
    private UILabel block_num = null;      //障碍物数目
    private UISprite level = null;         //游戏等级level_
    private UILabel score_num = null;      //游戏分数 POINT

	// Use this for initialization
    void Start()
    {
        btnClose = FindInChild<Button>("btnClose");
        cost_time = FindInChild<UILabel>("Center/result/label/cost_time");
        block_num = FindInChild<UILabel>("Center/result/label/block_num");
        score_num = FindInChild<UILabel>("Center/result/label/score_num");
        level = FindInChild<UISprite>("Center/result/level");

        btnClose.onClick = OnbtnClose;
	}


    /// <summary>
    /// 打开界面后的处理数据，显示结果
    /// </summary>
    public override void HandleAfteerOpenView()
    {
        base.HandleAfteerOpenView();
        ShowResult();  //显示游戏结果
    }


    /// <summary>
    /// 
    /// </summary>
    private void ShowResult()
    {
        cost_time.text = MainView.time_lab.text;
        block_num.text = DataConst.BLOCK_COUNT.ToString();
        level.spriteName = "level_" + "3";
        score_num.text = DataConst.SCORE_TOTAL.ToString() + " POINT";
    }
    //点击了关闭结束的按钮
    private void OnbtnClose(GameObject go)
    {
        CloseView();
        OpenView("StartView");
    }
}
