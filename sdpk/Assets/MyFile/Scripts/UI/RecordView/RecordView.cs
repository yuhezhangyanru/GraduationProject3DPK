///author:Yanru Zhang
///date:2014-3-14
///function:binded to record view

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using myfiles.scripts.tools;

namespace myfiles.scripts.ui.recordview
{
    public class RecordView : BaseView
    {
        public override ViewLayer layerType
        {
            get
            {
                return ViewLayer.MiddleLayer;
            }
        }

        private GameObject RecordItem = null;  //记录的原始对象
        private Button btnBack = null;       //回退的按钮


        public struct Item   //记录元素的结构
        {
            public GameObject go;
            public UILabel score;
            public UILabel time;
        }
        public List<Item> recList = new List<Item>();
        // Use this for initialization
        
        void Awake()
        {
            btnBack = FindInChild<Button>("btnBack");

            //这些操作都应当在打开面板的时候去执行,暂时放在这里
            RecordItem = FindInChild<Transform>("RecordPanel/item").gameObject;
            //int num = 10;  ///这个是记录条数
            if (0 == RecordTb.RecList.Count)
            {
                return;
            }
            else
            {
                RecordItem.SetActive(true);
            } 
            Item item = new Item();
            item.go = RecordItem;
            item.score = RecordItem.transform.FindChild("score").GetComponent<UILabel>();
            item.time = RecordItem.transform.FindChild("time").GetComponent<UILabel>();
            recList.Add(item);
            Vector3 v = RecordItem.transform.localPosition;

            for (int i = 0; i < RecordTb.RecList.Count+5; i++)    //预留5条记录，用于在游戏中动态查看刚刚打完的所有成绩，游戏结束后也可以查看记录
            {
                GameObject record = GameObject.Instantiate(RecordItem, RecordItem.transform.position, RecordItem.transform.rotation) as GameObject;
                record.transform.parent = RecordItem.transform.parent;
                record.transform.localScale = Vector3.one;
                v -= new Vector3(0, 90, 0);
                record.transform.localPosition = v;
                Item item1 = new Item();
                item1.go = record;
                item1.score = record.transform.FindChild("score").GetComponent<UILabel>();
                item1.time = record.transform.FindChild("time").GetComponent<UILabel>();
                if (i >= RecordTb.RecList.Count-1)  //暂时将多余的隐藏掉
                {
                    item1.go.gameObject.SetActive(false);
                }
                recList.Add(item1);
            }

        }

        void Start()
        {
            //按钮事件
            btnBack.onClick = OnbtnBack;

            ShowRecord();
        }

        /// <summary>
        /// 打开该界面之后的一些处理操作
        /// </summary>
        public override void HandleAfteerOpenView()
        {
            base.HandleAfteerOpenView();
            //CMySql.ReadRecordTb();  //重新读取数据库的数据
            ShowRecord();  //显示分数
            //ToolsFunction.Log(this.gameObject, "HandleAfteerOpenView 这个执行了");
        }
        /// <summary>
        /// 呈现游戏记录
        /// </summary>
        public void ShowRecord()
        {
            ToolsFunction.Log(this.gameObject, "开始显示游戏记录" + RecordTb.RecList.Count);
            //首先对成绩单列表排序
            RecordTb.RecList.Sort(delegate(RecordTb.RecordTb_1 a, RecordTb.RecordTb_1 b) { return b.score.CompareTo(a.score); });
            for (int i = 0; i < RecordTb.RecList.Count; i++)
            {
                recList[i].score.text = (i + 1).ToString() + ". "+RecordTb.RecList[i].score;  // (i * 1000).ToString();
                recList[i].time.text = RecordTb.RecList[i].time;    // (i * 20).ToString();
                recList[i].go.gameObject.SetActive(true);
            }
        }



        /// <summary>
        /// 按下后退的按钮，返回到StartView界面
        /// </summary>
        /// <param name="go"></param>
        private void OnbtnBack(GameObject go)
        {
            CloseView();
            //OpenView("StartView");
        }
        
    }
}
