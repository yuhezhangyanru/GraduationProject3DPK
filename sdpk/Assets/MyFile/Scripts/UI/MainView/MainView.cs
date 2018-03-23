///author:Yanru Zhang
///date:2014-3-10 21:22:14
///function:UI, binded to MainView, information for game on play

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;
using System;
using myfiles.scripts.ui.startview;

namespace myfiles.scripts.ui.mainview
{
    public class MainView : BaseView
    {
        public override ViewLayer layerType
        {
            get
            {
                return ViewLayer.MiddleLayer;
            }
        }
        private UILabel lScoreTotal = null;     //玩家当前分数
        private UILabel lBlockNum = null;     //当前通过的障碍个数
        private UISprite SettingBg = null;      //设置按钮的背景图片
        private UISprite MusicBg = null;        //声音按钮的背景图片
        private static UISprite daoshu = null;          //开始游戏前的倒数动画
        private Camera mapCamera = null;        //地图摄像机
 

        private UISlider sRunProcess = null;   //玩家当前跑步的进度
        private Button btnSetting = null;  //测试的按钮
        private Button btnGoOn = null;
        private Button btnMusic = null;
        private Button btnQuit = null;
        private GameObject Setting_Otherbtn = null;  //除了设置按钮之外的其他按钮
        private string[] sCountStr = { "num_2", "num_1", "num_0"}; 
        private int nCountStrIndex= 0;
        private int nTotalSecond = 0;  //倒计时的总时间
        private bool bPauseCount = false;

        public static UISprite MainTip = null;         //出现障碍物的提示动画图片
        public static UISprite BlockHead = null;     //障碍物的头像
        public static UILabel time_lab = null;       //显示剩余时间
        public static int nPresentScon = 0;  //当前呈现秒数，用于倒计时
        public static int nCountScon = 0;  //当前总共秒数，用于记录成绩

        // Use this for initialization
        void Awake()
        {
            lScoreTotal = FindInChild<UILabel>("topLeft/score/score_total");
            lBlockNum = FindInChild<UILabel>("topLeft/block/block_num"); //障碍个数
            time_lab = FindInChild<UILabel>("topRight/time_lab");
            sRunProcess = FindInChild<UISlider>("topLeft/run_process");
            btnSetting = FindInChild<Button>("bottomRight/btnSetting");
            btnGoOn = FindInChild<Button>("bottomRight/otherbtn/btnGoOn");
            btnMusic = FindInChild<Button>("bottomRight/otherbtn/btnMusic");
            btnQuit = FindInChild<Button>("bottomRight/otherbtn/btnQuit");
            Setting_Otherbtn = FindInChild<Transform>("bottomRight/otherbtn").gameObject;
            SettingBg = FindInChild<UISprite>("bottomRight/btnSetting/Background");
            MusicBg = FindInChild<UISprite>("bottomRight/otherbtn/btnMusic/Background");
            daoshu = FindInChild<UISprite>("Center/daoshu");
            MainTip = FindInChild<UISprite>("topRight/tip_icon");
            BlockHead = FindInChild<UISprite>("topRight/block_head");
            mapCamera = FindInChild<Camera>("bottomLeft/MapCamera");

            btnSetting.onClick = OnBtnSetting;
            //btnSetting.onHover = OnBtnSetting;  //这个暂时用的是鼠标滑过事件
            btnGoOn.onClick = OnbtnGoOn;
            btnMusic.onClick = OnbtnMusic;
            btnQuit.onClick = OnbtnQuit;

            ToolsFunction.ScoreUpdate += UpdateData; //更新分数
            GameEndTrig.ProcessUpdate += UpdateProcess;   //更新游戏阶段，游戏结束了
            Setting_Otherbtn.SetActive(false);   //首先隐藏菜单按钮

            //main界面设置的初始化
            InitMainSetting();
        }

        private void Start()
        {
            mapCamera.GetComponent<SmoothFollow>().target = DataConst.MeDisplay.transform; //制定地图摄像机跟随人物 
        }


        /// <summary>
        /// 打开界面之后的操作
        /// </summary>
        public override void HandleAfteerOpenView()
        {
            base.HandleAfteerOpenView();
            ResetCountAnimation();
            ToolsFunction.Log(this.ToString(), "HandleAfteerOpenView执行了");
            //重新读取设置的配置信息
            XmlReadDoc.ReadSetting();
            //SettingTb.Element =CMySql. GetOneColumnTab("settingtb");
            //应用默认的设置信息
            if (this.gameObject)
            {
                InitMainSetting();
            }
            //开始播放倒数动画
            //PlayCountAnimation();

            //开启倒计时器的协程, 总时间10*60== 600s
            nTotalSecond = 600;
            StartCoroutine(TimerCount(nTotalSecond));
        }

        /// <summary>
        /// 倒计时的协程函数
        /// </summary>
        /// <param name="nTotalScond">想要计时的总的秒数</param>
        /// <returns></returns>
        IEnumerator TimerCount(int nTotalScond)
        {
            int second = 0;
            int nTotal = nTotalScond;
            if (second == nTotal)
            {
                Debug.Log(Time.time + "计时器结束了!");
                //这个时候应当直接弹出失败的界面，表示游戏结束！
                //todo
            }
            while(second < nTotal)
            {
                if (bPauseCount == false)
                {
                    Debug.Log(Time.time + " now, second：" + second.ToString());
                    nPresentScon = nTotal - second;  //剩余时间
                    nCountScon = second;
                    time_lab.text = ToolsFunction.TimeFormat(nPresentScon);
                    second++;
                    //如果没有暂定才会返回
                }
                yield return new WaitForSeconds(1f);  //每秒返回一次
            }           
        }



      
        /// <summary>
        /// 初始化主要的设置信息
        /// </summary>
        public void InitMainSetting()
        {
            if ("1" == SettingTb.Element[SettingTb.INDEX_MUSIC])  //要播放音乐
            {
                Common.MUSIC_BG.Play();
                MusicBg.spriteName = "play_music";
            }
            else 
            {
                Common.MUSIC_BG.Stop();
                MusicBg.spriteName = "no_music";
            }
        }


        /// <summary>
        /// 打开界面后重新开始播放倒数动画
        /// </summary>
        public static void ResetCountAnimation()
        {
            if (daoshu)
            {
                daoshu.GetComponent<UISprite>().spriteName = "num_3";
                daoshu.GetComponent<UISprite>().enabled = true;
                daoshu.GetComponent<Animator>().enabled = true;
            }
        }



        //播放完一个数字动画的回调函数
        private  void CountPlayCallback1()
        {
            daoshu.spriteName = "num2";
        }

        IEnumerator CountPlay()
        {
            yield return new WaitForSeconds(1f);  //等1s
            if (nCountStrIndex < 3)
            {
                //ToolsFunction.Log(this.gameObject, "播放下一个动画");
                daoshu.spriteName = sCountStr[nCountStrIndex];
                nCountStrIndex++;
            }
            else  //说明播放完了0
            {
                //ToolsFunction.Log(this.gameObject, "结束动画，开始跑步");
                daoshu.gameObject.SetActive(false);
                State.KEEP_IDLE = false;  //开始让人物跑步
            }
        }

        public void TestFunc()
        {
            Debug.Log("MainView中的测试函数被调用了");
        }


        /// <summary>
        /// 更新分数呈现
        /// </summary>
        /// <param name="code"></param>
        private void UpdateData(int code)
        {
            //ToolsFunction.Log(this.gameObject, "接受到的业务代码：" + code);
            if (code == CODE.UPDATE_SCORE)
            {
                SetLabelValue(lScoreTotal, DataConst.SCORE_TOTAL);  //更新显示分数                
            }
            if (code == CODE.UPDATE_BLOCK)
            {
                MainTipControl.StartAnim(MainTip, BlockHead);
                lBlockNum.text = DataConst.BLOCK_COUNT.ToString();
            }
            if (code == CODE.UPDATE_PROCESS)
            {
                sRunProcess.value = ((float)State.PROCESS_PERCENT / (float)24);
            }
        }


        /// <summary>
        /// 更新游戏阶段，到了游戏的终点
        /// </summary>
        /// <param name="code"></param>
        private void UpdateProcess(int code)
        {
            if(code == CODE.UPDATE_GAME_PROCESS)
            {
                OpenView("AwardView");
                CloseView();
            }
        }


        /// <summary>
        /// 遇到障碍物的消息提示控制，主要是障碍物头像和障碍物提示切换
        /// </summary>
        private void BlockTipControl()
        {
            //设置tip图标和障碍物头像

        }


        /// <summary>
        /// 按下设置按钮时候的开关处理
        /// </summary>
        private void ControlSettingBtn()
        {
            //ToolsFunction.Log(this.gameObject, "背景图片的名字："+SettingBg.spriteName);
            //Setting_Otherbtn
            if ("settting" == SettingBg.spriteName)   //收缩时候的图片
            { 
                //弹出菜单，修改图片
                SettingBg.spriteName = "setting_btn_close_icon";
                Setting_Otherbtn.SetActive(true);
                State.KEEP_IDLE = true;  //按下设置按钮，就让人物静止
                bPauseCount = true;
                //nTotalSecond = nPresentScon;
            }
            else if ("setting_btn_close_icon" == SettingBg.spriteName)   //其他按钮已经弹出时
            {
                SettingBg.spriteName = "settting";
                Setting_Otherbtn.SetActive(false);
                State.KEEP_IDLE = false;  //按下设置按钮，就让人物静止
                bPauseCount = false;
                //StartCoroutine();
                //StartCoroutine(TimerCount(nTotalSecond));
            }
        }


        /// <summary>
        /// 音乐开关的处理
        /// </summary>
        private void ControlMusicBtn()
        {
            ToolsFunction.Log(this.gameObject, "当前音乐按钮的背景图片：" + MusicBg.spriteName);
            if ("play_music" == MusicBg.spriteName)
            {
                MusicBg.spriteName = "no_music";
                Common.MUSIC_BG.Pause();  //关闭音乐,其实是音乐暂停，再次点击会继续播放
            }
            else if ("no_music" == MusicBg.spriteName)
            {
                MusicBg.spriteName = "play_music";
                Common.MUSIC_BG.Play();  //结束音乐
            }
        }



        //*******************************************************
        ///按钮函数
        /// <summary>
        ///按下测试按钮
        /// </summary>
        /// <param name="go"></param>
        private void OnBtnSetting(GameObject go)   //暂时先不使用点击，测试滑过事件
        {
            ToolsFunction.Log(this.gameObject, "按下NGUI的设置按钮");
            ControlSettingBtn();
            //CMySql.GetDataSet("delete from test");OK
            //CMySql.GetDataSet("insert into unity values(43, '43', '543534')");OK
        }

        //点击了音乐开关按钮
        private void OnbtnMusic(GameObject go)
        {
            ToolsFunction.Log(this.gameObject, "按下音乐开关按钮：当前图片名称:");
            ControlMusicBtn(); //音乐开关控制
        }

        //点击游戏继续按钮
        private void OnbtnGoOn(GameObject go)
        {
            ToolsFunction.Log(this.gameObject, "点击了游戏继续");
            ResetCountAnimation(); //倒数之后才开始继续跑步！！
        }

        //点击了退出按钮
        private void OnbtnQuit(GameObject go)
        {
            ToolsFunction.Log(this.gameObject, "点击了退出按钮");
            //这里调用保存游戏的分数和时间
            State.KEEP_IDLE = true;
            CloseView();   //关闭当前界面
            OpenView("AwardView");  //打开启动界面
            RecordTb.SaveRecord();                 //保存游戏记录
            //保存游戏当中的设置
            XmlReadDoc.SaveSetting(SettingTb.Element[SettingTb.INDEX_MUSIC], SettingTb.Element[SettingTb.INDEX_HINT_TIP], SettingTb.Element[SettingTb.INDEX_SPEED], SettingTb.Element[SettingTb.INDEX_QUALITY]);
            //CMySql.UpdateSetting(SettingTb.Element[SettingTb.INDEX_MUSIC], SettingTb.Element[SettingTb.INDEX_HINT_TIP], SettingTb.Element[SettingTb.INDEX_SPEED], SettingTb.Element[SettingTb.INDEX_QUALITY]);
        }

    }
}