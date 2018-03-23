using UnityEngine;
using System.Collections;
using System.Xml;

public class XmlReadDoc : MonoBehaviour
{
    
    public static XmlDocument settingDoc = null; //设置表的文档对象
    public static XmlNode settingRoot = null;    //设置表文档的根节点
    public static XmlDocument roadDoc = null;    //道路属性表的文档对象
    public static XmlNode roadRoot = null;       //道路属性文档的根节点
    public static XmlDocument recordDoc = null;  //游戏记录表的文档对象
    public static XmlNode recordRoot = null;     //游戏记录表的根节点

    void Start()
    {

	}


    /// <summary>
    /// 读取设置表信息
    /// </summary>
    public static void ReadSetting()
    {
        SettingTb.Element.Clear();   //清空设置表的内容
        settingDoc = new XmlDocument();
        settingDoc.Load("Assets/MyFile/Xml/settingtb.xml");
        settingRoot = settingDoc.SelectSingleNode("RECORDS");
        XmlNodeList xnl = settingRoot.ChildNodes;
        Debug.Log(Time.time + "Start to read xml");
        foreach (XmlNode xnl_1 in xnl)
        {
            XmlElement xe = (XmlElement)xnl_1;
            XmlNodeList nodeList = xe.ChildNodes;
            string str1 = nodeList.Item(0).InnerText;
            Debug.Log(Time.time + " id: " + str1);
            SettingTb.Element.Add(nodeList.Item(0).InnerText);
            str1 = nodeList.Item(1).InnerText;
            Debug.Log(Time.time + " music: " + str1);
            SettingTb.Element.Add(nodeList.Item(1).InnerText);
            str1 = nodeList.Item(2).InnerText;
            Debug.Log(Time.time + " hind_tip: " + str1);
            SettingTb.Element.Add(nodeList.Item(2).InnerText);
            str1 = nodeList.Item(3).InnerText;
            Debug.Log(Time.time + " speed: " + str1);
            SettingTb.Element.Add(nodeList.Item(3).InnerText);
            str1 = nodeList.Item(4).InnerText; ;
            Debug.Log(Time.time + " quality: " + str1);
            SettingTb.Element.Add(nodeList.Item(4).InnerText);
        }
    }

    /// <summary>
    /// 保存设置修改
    /// </summary>
    public static void SaveSetting(string music, string hind, string speed, string quality)
    {
        XmlElement xe = settingDoc.DocumentElement;
        string strPath = string.Format("/RECORDS/RECORD");
        XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
        selectXe.GetElementsByTagName("music").Item(0).InnerText = (music == "True" ? "1":"0");// "music1";
        selectXe.GetElementsByTagName("hind_tip").Item(0).InnerText = (hind == "True" ? "1" : "0");// "tip1";
        selectXe.GetElementsByTagName("speed").Item(0).InnerText = speed;
        selectXe.GetElementsByTagName("quality").Item(0).InnerText = quality;
        settingDoc.Save("Assets/MyFile/Xml/settingtb.xml");
    }


    /// <summary>
    /// 读取道路属性的表
    /// </summary>
    private static void ReadRoadTb()
    {
        roadDoc = new XmlDocument();
        roadDoc.Load("Assets/MyFile/Xml/roadtb.xml");
        roadRoot = roadDoc.SelectSingleNode("RECORDS");
        XmlNodeList xnl = roadRoot.ChildNodes;
        Debug.Log(Time.time + "Start to read road xml");
        foreach (XmlNode xnl_1 in xnl)
        {
            XmlElement xe = (XmlElement)xnl_1;
            XmlNodeList nodeList = xe.ChildNodes;
            RoadTb.road.road_id = int.Parse(nodeList.Item(0).InnerText); // reader.GetInt32(0);
            RoadTb.road.road_name = nodeList.Item(1).InnerText;// reader.GetString(1);
            RoadTb.road.gold_pos.x = float.Parse(nodeList.Item(2).InnerText);// float.Parse(reader.GetString(2));
            RoadTb.road.gold_pos.y = float.Parse(nodeList.Item(3).InnerText);//reader.GetString(3));
            RoadTb.road.gold_pos.z = float.Parse(nodeList.Item(4).InnerText);//reader.GetString(4));
            RoadTb.road.is_gold = int.Parse(nodeList.Item(5).InnerText);// reader.GetInt32(5);
            RoadTb.road.monster = int.Parse(nodeList.Item(6).InnerText);// reader.GetInt32(6);
            RoadTb.road.is_monster = int.Parse(nodeList.Item(7).InnerText);// reader.GetInt32(7);
            RoadTb.road.box = int.Parse(nodeList.Item(8).InnerText);// reader.GetInt32(8);
            RoadTb.road.is_box = int.Parse(nodeList.Item(9).InnerText);// reader.GetInt32(9);
            RoadTb.road.box_pos.x = float.Parse(nodeList.Item(10).InnerText);//reader.GetString(10));
            RoadTb.road.box_pos.y = float.Parse(nodeList.Item(11).InnerText);//reader.GetString(11));
            RoadTb.road.box_pos.z = float.Parse(nodeList.Item(12).InnerText);//reader.GetString(12));
            RoadTb.road.mon_pos.x = float.Parse(nodeList.Item(13).InnerText);//reader.GetString(13));
            RoadTb.road.mon_pos.y = float.Parse(nodeList.Item(14).InnerText);//reader.GetString(14));
            RoadTb.road.mon_pos.z = float.Parse(nodeList.Item(15).InnerText);//reader.GetString(15));
            RoadTb.RoadList.Add(RoadTb.road);  //加到队列中去
        }
    }


    /// <summary>
    /// 读取游戏记录表
    /// </summary>
    private static void ReadRecordTb()
    {
        recordDoc = new XmlDocument();
        recordDoc.Load("Assets/MyFile/Xml/recordtb.xml");
        recordRoot = recordDoc.SelectSingleNode("RECORDS");
        XmlNodeList xnl = recordRoot.ChildNodes;
        foreach (XmlNode xnl_1 in xnl)
        {
            XmlElement xe = (XmlElement)xnl_1;
            XmlNodeList nodeList = xe.ChildNodes;
            RecordTb.record.score = nodeList.Item(0).InnerText;
            RecordTb.record.time = nodeList.Item(1).InnerText;
            RecordTb.record.cost = nodeList.Item(2).InnerText;
            RecordTb.record.level = nodeList.Item(3).InnerText; //存的是a,b,c,d
            RecordTb.RecList.Add(RecordTb.record);
        }
    }


    /// <summary>
    /// 保存游戏记录
    /// </summary>
    public static void SaveRecord(string [] param)
    {
        XmlElement xelKey = recordDoc.CreateElement("RECORD");
        XmlElement item = recordDoc.CreateElement("score");
        item.InnerText = param[0];
        xelKey.AppendChild(item);
        item = recordDoc.CreateElement("time");
        item.InnerText = param[1];
        xelKey.AppendChild(item);
        item = recordDoc.CreateElement("cost");
        item.InnerText = param[2];
        xelKey.AppendChild(item);
        item = recordDoc.CreateElement("level");
        item.InnerText = param[3];
        xelKey.AppendChild(item);
        recordRoot.AppendChild(xelKey);
        recordDoc.Save("Assets/MyFile/Xml/recordtb.xml");
   }


    /// <summary>
    /// 加载所有的数据表
    /// </summary>
    public static void DataLoad()
    {
        ReadSetting();  //读取设置表信息
        ReadRoadTb();   //读取道路属性表信息
        ReadRecordTb(); //读取游戏记录表
    }

    void Update()
    {
        if (Input.GetKeyDown("w"))  //增加一条记录
        {
            XmlElement xelKey = settingDoc.CreateElement("RECORD");
            XmlElement item  = settingDoc.CreateElement("id");
            item.InnerText = "12";
            xelKey.AppendChild(item);
            item = settingDoc.CreateElement("music");
            item.InnerText = "music";
            xelKey.AppendChild(item);
            item = settingDoc.CreateElement("hind_tip");
            item.InnerText = "hind_tip";
            xelKey.AppendChild(item);
            item = settingDoc.CreateElement("speed");
            item.InnerText = "speed";
            xelKey.AppendChild(item);
            item = settingDoc.CreateElement("quality");
            item.InnerText = "quality";
            xelKey.AppendChild(item);
            settingRoot.AppendChild(xelKey);
            settingDoc.Save("Assets/MyFile/Xml/settingtb.xml");
        }
        if(Input.GetKeyDown("a"))  //修改设置表的内容
        {
            XmlElement xe = settingDoc.DocumentElement;
            string strPath = string.Format("/RECORDS/RECORD");
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            selectXe.GetElementsByTagName("music").Item(0).InnerText = "music1";
            selectXe.GetElementsByTagName("hind_tip").Item(0).InnerText = "tip1";
            selectXe.GetElementsByTagName("speed").Item(0).InnerText = "speed1";
            selectXe.GetElementsByTagName("quality").Item(0).InnerText = "quality1";
            settingDoc.Save("Assets/MyFile/Xml/settingtb.xml");
        }
    }
	
}
