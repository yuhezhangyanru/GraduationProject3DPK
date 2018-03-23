///author:Yanru Zhang
///date:2014-3-17
///function:binded to SettingView, all setting

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;
using System.Data;
using System.Collections.Generic;

namespace myfiles.scripts.ui.settingview
{
    public class SettingView : BaseView
    {
        public override ViewLayer layerType
        {
            get
            {
                return ViewLayer.MiddleLayer;
            }
        }
        private Button btnBack = null;
        private UIToggle toggleMusic = null;   //音乐的勾选设置
        private UIToggle toggleMaintip = null;  //主界面的提示显示设置
        private UISlider sliderSpeed = null;  //速度的设置条
        private UISlider sliderQuality = null;  //设置游戏品质

        // Use this for initialization
        void Awake()
        {
            btnBack = FindInChild<Button>("btnBack");
            toggleMusic = FindInChild<UIToggle>("Center/Content/toggleMusic");
            if (toggleMusic)
            {
                ToolsFunction.Log(this.gameObject, "已经找到音乐选项了");
            }
            toggleMaintip = FindInChild<UIToggle>("Center/Content/toggleMaintip");
            sliderSpeed = FindInChild<UISlider>("Center/Content/sliderSpeed");
            sliderQuality = FindInChild<UISlider>("Center/Content/sliderQuality");

            btnBack.onClick = OnbtnBack;
            toggleMusic.onChange.Add(new EventDelegate(toggleMusicOnClick));
            toggleMaintip.onChange.Add(new EventDelegate(toggleMaintipOnClick));
            sliderSpeed.onChange.Add(new EventDelegate(sliderSpeedOnClick));
            sliderQuality.onChange.Add(new EventDelegate(sliderQualityOnClick));

            XmlReadDoc.ReadSetting();

            //初始化设置界面的组件
            InitSetting();
        }


        //打开界面之后处理的一些工作
        public override void HandleAfteerOpenView()
        {
            base.HandleAfteerOpenView();
            //重读设置界面的内容
            ToolsFunction.Log(this.gameObject.name, "执行了HandleAfterOpenView函数");
            //SettingTb.Element = CMySql.GetOneColumnTab("settingtb");
            XmlReadDoc.ReadSetting();
            if(toggleMusic)  //在组件找到的情况下
                InitSetting();
            //StartCoroutine(InitSettingFunc());
            //InitSetting();
        }

        IEnumerator InitSettingFunc()
        {
            yield return new WaitForSeconds(0.5f);
            InitSetting();
        }

        //设置基本信息
        private void InitSetting()
        {
            ToolsFunction.Log(this.gameObject, "在打开界面之后");
            ToolsFunction.Log(this.gameObject, "音乐：" + SettingTb.Element[SettingTb.INDEX_MUSIC]);
            ToolsFunction.Log(this.gameObject, "提示：" + SettingTb.Element[SettingTb.INDEX_HINT_TIP]);
            ToolsFunction.Log(this.gameObject, "速度：" + SettingTb.Element[SettingTb.INDEX_SPEED].ToString());
            ToolsFunction.Log(this.gameObject, "记录：" + SettingTb.Element[SettingTb.INDEX_QUALITY].ToString());

            toggleMusic.value = SettingTb.Element[SettingTb.INDEX_MUSIC] == "1" ? true : false ;
            toggleMaintip.value = SettingTb.Element[SettingTb.INDEX_HINT_TIP] == "1"?true:false;
            sliderSpeed.value = float.Parse(SettingTb.Element[SettingTb.INDEX_SPEED]) / ((float)2);
            sliderQuality.value = float.Parse(SettingTb.Element[SettingTb.INDEX_QUALITY]);
        }

        /// <summary>
        /// 关闭设置界面
        /// </summary>
        /// <param name="go"></param>
        private void OnbtnBack(GameObject go)
        {
            //要将设置信息写入数据库啊！！
            //ToolsFunction.Log(this.gameObject, "音乐：" + toggleMusic.value.ToString());
            //ToolsFunction.Log(this.gameObject, "提示：" + toggleMaintip.value.ToString());
            //ToolsFunction.Log(this.gameObject, "速度：" + (int)(sliderSpeed.value * 2));
            //ToolsFunction.Log(this.gameObject, "记录：" + (int)(sliderQuality.value));

            SettingTb.Element[SettingTb.INDEX_MUSIC] = toggleMusic.value.ToString();
            SettingTb.Element[SettingTb.INDEX_HINT_TIP] = toggleMaintip.value.ToString();
            SettingTb.Element[SettingTb.INDEX_SPEED] = ((int)(sliderSpeed.value * 2)).ToString();
            SettingTb.Element[SettingTb.INDEX_QUALITY] = ((int)(sliderQuality.value)).ToString();

            //保存设置信息到数据库
            XmlReadDoc.SaveSetting(SettingTb.Element[SettingTb.INDEX_MUSIC], SettingTb.Element[SettingTb.INDEX_HINT_TIP], SettingTb.Element[SettingTb.INDEX_SPEED], SettingTb.Element[SettingTb.INDEX_QUALITY]);
            //CMySql.UpdateSetting(SettingTb.Element[SettingTb.INDEX_MUSIC], SettingTb.Element[SettingTb.INDEX_HINT_TIP], SettingTb.Element[SettingTb.INDEX_SPEED], SettingTb.Element[SettingTb.INDEX_QUALITY]);
            SetRunRate();
            //关闭界面
            CloseView();
        }


        /// <summary>
        /// 设置玩家的跑步速率
        /// </summary>
        private void SetRunRate()
        {
            float fRate = (int.Parse(SettingTb.Element[SettingTb.INDEX_SPEED]) == 0) ? 0.1f : int.Parse(SettingTb.Element[SettingTb.INDEX_SPEED]);
            MeController.nRate = (int)(10 / fRate);
            Debug.Log(Time.time + "设置玩家跑步速度成功！" +" xishu："+fRate+", "+ MeController.nRate);
        }

        private void OntoggleMusic(GameObject go)
        {
            ToolsFunction.Log(this.gameObject, "被选中了");
        }


        /// <summary>
        /// 选择是否播放音乐
        /// </summary>
        private void toggleMusicOnClick()
        {
        }


        /// <summary>
        /// 选择是否提示主界面信息
        /// </summary>
        private void toggleMaintipOnClick()
        {
            //ToolsFunction.Log(this.gameObject, "zhujiemian当前的选择结果" + toggleMaintip.value);
        }


        /// <summary>
        /// 修改跑步速率
        /// </summary>
        private void sliderSpeedOnClick()
        {
        }


        /// <summary>
        /// 修改游戏品质，
        /// </summary>
        private void sliderQualityOnClick()
        {
            //ToolsFunction.Log(this.gameObject, "当前游戏品质：" + sliderQuality.value/0.5);
        }
    }
}