///author:Yanru Zhang
///date:2014-3-14
///function:binded to StartView, for main menu of game

using UnityEngine;
using myfiles.scripts.tools;
using myfiles.scripts.ui.mainview;
using myfiles.scripts.ui.recordview;
using myfiles.scripts.ui.settingview;

namespace myfiles.scripts.ui.startview
{
    public class StartView : BaseView
    {

        //设置界面的层次
        public override ViewLayer layerType
        {
            get
            {
                return ViewLayer.LowLayer;
            }
        }


        private Button StartBtn = null;
        private Button RecordBtn = null;
        private Button SettingBtn = null;
        private Button HelpBtn = null;
        private Button QuitBtn = null;

        // Use this for initialization
        void Start()
        {
            //ToolsFunction.Log(this.gameObject.name, "初始化了启动界面");
            StartBtn = FindInChild<Button>("Center/StartBtn");
            RecordBtn = FindInChild<Button>("Center/RecordBtn");
            SettingBtn = FindInChild<Button>("Center/SettingBtn");
            HelpBtn = FindInChild<Button>("Center/HelpBtn");
            QuitBtn = FindInChild<Button>("Center/QuitBtn");

            //添加监听事件
            StartBtn.onClick = OnStartBtn;
            RecordBtn.onClick = OnRecordBtn;
            SettingBtn.onClick = OnSettingBtn;
            HelpBtn.onClick = OnHelpBtn;
            QuitBtn.onClick = OnQuitBtn;

            //这些全部保留，当不能识别点击事件的时候，就使用鼠标划过事件
            //StartBtn.onHover = OnStartBtn;
            //RecordBtn.onHover = OnRecordBtn;
            //SettingBtn.onHover = OnSettingBtn;
            //HelpBtn.onHover = OnHelpBtn;
        }


        public override void HandleAfteerOpenView()
        {
            base.HandleAfteerOpenView();
            //ToolsFunction.Log(this.gameObject, "startview的handleafterOpenView执行了");
        }
        /// <summary>
        /// 点击了开始游戏按钮正式进入游戏
        /// </summary>
        /// <param name="go"></param>
        public void OnStartBtn(GameObject go)
        {
            ToolsFunction.Log(this.gameObject, "点击了开始游戏按钮");
            //进入就播放倒计时的动画啊

            CloseView();   //关闭当前界面
            SubViewNow = OpenView("MainView");
            SubViewNow.GetComponent<MainView>().HandleAfteerOpenView();  //打开界面后处理的操作
            //Singleton<MainView>.Instance.OpenView();   //打开游戏的主界面
        }


// Usage

        /// <summary>
        /// 点击了查看记录的按钮
        /// </summary>
        /// <param name="go"></param>
        public void OnRecordBtn(GameObject go) 
        {
            ToolsFunction.Log(this.gameObject, "点击了查看记录按钮");
            //CloseView();  //关闭当前界面
        
            SubViewNow = OpenView("RecordView");
            SubViewNow.GetComponent<RecordView>().HandleAfteerOpenView();
            //Singleton<RecordView>.Instance.OpenView();  //打开游戏记录的界面
        }

        /// <summary>
        /// 点击游戏设置按钮
        /// </summary>
        /// <param name="go"></param>
        public void OnSettingBtn(GameObject go)
        {
            ToolsFunction.Log(this.gameObject, "点击了查看设置按钮");
            SubViewNow = OpenView("SettingView");  //打开设置面板
            SubViewNow.GetComponent<SettingView>().HandleAfteerOpenView();
            //Singleton<SettingView>.Instance.OpenView();   //打开设置的面板
        }

        /// <summary>
        /// 查看游戏帮助
        /// </summary>
        /// <param name="go"></param>
        /// 

        public void OnHelpBtn(GameObject go)
        {
            //ToolsFunction.Log(this.gameObject, "点击了查看游戏帮助按钮！");
            OpenView("HelpView");
        }

        /// <summary>
        /// 点击了退出游戏
        /// </summary>
        /// <param name="go"></param>
        public void OnQuitBtn(GameObject go)
        {
            ToolsFunction.Log(this.gameObject, "点击了退出游戏按钮");

            Application.Quit();  //关闭应用程序
            //还是要做保存记录的操作!!!!!!

            //todo
            //CloseView();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}