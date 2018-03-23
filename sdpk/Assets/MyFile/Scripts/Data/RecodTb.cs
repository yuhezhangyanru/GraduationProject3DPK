///author:Yanru Zhang
///date：2014-4-8 10:43:123
///function:to store game result

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using myfiles.scripts.ui.mainview;
using myfiles.scripts.tools;

public class RecordTb : MonoBehaviour {

    public struct RecordTb_1
    {
        public string score;       //分数
        public string time;        //记录时间
        public string cost;        //花费的时间
        public string level;       //结果等级
    }

    public static RecordTb_1 record = new RecordTb_1();
    public static List<RecordTb_1> RecList = new List<RecordTb_1>();    //成绩列表

    public class ResultInfoAscent : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((RecordTb_1)x).score.CompareTo(((RecordTb_1)y).score);
        }
    }


    public static void SortRecord()
    {
        //for(int i = 0; i< )
    }

    //保存游戏记录到数据库中
    public static void SaveRecord()
    {
        string sTime = System.DateTime.Now.ToShortDateString() + ","+System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString();
        RecordTb.record.score = DataConst.SCORE_TOTAL.ToString();
        RecordTb.record.time = sTime;
        RecordTb.record.cost = ToolsFunction.TimeFormat(MainView.nCountScon);//MainView.time_lab.text;   //时间标签此时的时间
        RecordTb.record.level = ToolsFunction.GetLevel(DataConst.SCORE_TOTAL, DataConst.BLOCK_COUNT,MainView.nCountScon);  //根据总分数获得游戏等级
        RecordTb.RecList.Add(RecordTb.record);  //把当前数据存到结构中
        string[] param = { DataConst.SCORE_TOTAL.ToString(), sTime, RecordTb.record.cost, RecordTb.record.level.ToString() };   //写入的参数
        //CMySql.WriteDB(DataConst.TB_RECORD, param);
        XmlReadDoc.SaveRecord(param); //保存游戏记录
    }
}

