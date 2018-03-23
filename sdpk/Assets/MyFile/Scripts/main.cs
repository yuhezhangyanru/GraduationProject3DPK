///author:Yanru Zhang
///date:2014-2-5
///function: binded to MainCarema 

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;
using System;

public class main : MonoBehaviour {
    
    private GameObject mainCamera = null;
    private SmoothFollow smoothFollowSpt = null;

    // Use this for initialization
	void Awake () {
        Debug.Log("main脚本，用于资源的加载");
        //设置帧数
        Application.targetFrameRate = 40;  
        mainCamera = GameObject.Find("MainCamera");
        smoothFollowSpt = mainCamera.GetComponent<SmoothFollow>();  //记得一定要初始化！
        Common.MUSIC_BG = mainCamera.GetComponent<AudioSource>();  //背景音乐的组件
        Common.MUSIC_BG.Stop();  //暂时不播放音乐

        //资源的加载
        DataLoad();           //预加载数据库资源
        ResouceLoad();    //加载预设资源
	}
	



    /// <summary>
    /// 数据装载，用于初始化需要预先加载的数据表
    /// </summary>
    private void DataLoad()
    {
        //CMySql.ConnectDB();   //装载RoadTb表
        XmlReadDoc.DataLoad();   //加载数据表
    }


    /// <summary>
    /// 加载预设资源
    /// </summary>
    private void ResouceLoad()
    {
        //下一步主要考虑的是加载次序的问题！！
        //预设资源的加载
        DataConst.GoldPrefab = Resources.Load("Gold") as GameObject;  //金币预设, 多个
        DataConst.MonsterPrefab = Resources.Load("Monster") as GameObject;  //怪物预设，多个
        DataConst.BlocksPrefab = Resources.Load("Blocks") as GameObject;    //障碍物预设，多个
        DataConst.BoxPrefab = Resources.Load("Box") as GameObject;   //箱子预设，多个
        DataConst.SpiderPrefab = Resources.Load("mSpider") as GameObject;   //蜘蛛的预设，多个
        DataConst.DragonPrefab = Resources.Load("dragon") as GameObject;    //飞龙的预设，多个
        DataConst.MeDisplayPrefab = Resources.Load("MeDisplay") as GameObject;

        DataConst.WallPrefab = Resources.Load("Wall") as GameObject;    //墙壁预设，一个
        DataConst.RoadPrefab = Resources.Load("roads") as GameObject;   //道路预设，一个
        DataConst.PropertyPrefab = Resources.Load("Property") as GameObject;   //环境陈设的预设，一个
        DataConst.HousePrefab = Resources.Load("House") as GameObject;   //房屋组的预设，一个
        GameObject UIPre = Resources.Load("UI") as GameObject;            //Ui资源的预设，一个
        GameObject MonsterBornPre = Resources.Load("Effect/MonsterBorn") as GameObject;   //怪物出生的特效
        GameObject MonsterDiePre = Resources.Load("Effect/MonsterDie2") as GameObject;   //怪物出生的特效        DataConst.MeDisplayPrefab = Resources.Load("MeDisplay") as GameObject;  //主角的预设, 一个
        
        for (int i = 0; i < 26; i++)
        {
            DataConst.P1[i] = Resources.Load("Pros/P"+(i+1).ToString()) as GameObject;   //初始化环境陈设
        }
        //GameObject go = ObjectManager.ObjInList(DataConst.PRE_BLOCK);
        //go.transform.localPosition = Vector3.zero;
        //go.transform.rotation = Quaternion.Euler(Vector3.zero);
        GameObject go = Instantiate(DataConst.BlocksPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
        ////暂时屏蔽掉这两句，但是还有用，等角色作为预设的时候放在所有加载的最后！！
        go = Instantiate(DataConst.WallPrefab, Vector3.zero + new Vector3(0, 0.3f, 0), Quaternion.Euler(Vector3.zero)) as GameObject;
        DataConst.RoadObject = Instantiate(DataConst.RoadPrefab, Vector3.zero + new Vector3(0, 0.3f, 0), Quaternion.Euler(Vector3.zero)) as GameObject;
        go = Instantiate(DataConst.PropertyPrefab, Vector3.zero + new Vector3(0, 0.3f, 0), Quaternion.Euler(Vector3.zero)) as GameObject;   //加载水火
        
        DataConst.UIPre = Instantiate(UIPre, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;   //加载UI资源
        DataConst.MonsterBornPre = Instantiate(MonsterBornPre, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;   //加载怪物出生的特效预设
        DataConst.MonsterDiePre = Instantiate(MonsterDiePre, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;   //加载怪物出生的特效预设

        //初始化主角位置
        go = Instantiate(DataConst.MeDisplayPrefab, new Vector3(57, 2.58f, -3), Quaternion.Euler(Vector3.zero)) as GameObject;
        smoothFollowSpt.target = go.transform;   //此时摄像机的跟随对象就是我的角色了
        DataConst.SwallFast1Pre = go.transform.FindChild("SwallFast1").gameObject;   //挥剑时候的紫色紫色特效
    }

}
