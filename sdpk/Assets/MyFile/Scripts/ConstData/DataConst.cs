///author:Yanru Zhang
///date:2014-2-16
///function:data const in project

using UnityEngine;
using System.Collections;

public class DataConst : MonoBehaviour {
    //public const Color ONSLOPE = new Color(Color.yellow); //这个颜色用于爬坡的时候标注使用
    //public const Color COLLITION = new Color(Color.blue); //用于游戏角色和其他物体的碰撞检测颜色
    //数组中暂时只存了22条road的名字，后续还要增加
    public static readonly string[] RoadNames = new string[]{"Road1", "Road2", "Road3", "Road4", "Road5", 
        "Road6", "Road7", "Road8", "Road9", "Road10", "Road11", "Road12", "Road13", "Road14", 
        "Road15", "Road16", "Road17", "Road18", "Road19", "Road20", "Road21", "Road22", "Road23", "Road24", "Road25", "Road26"};

    //是否会产生怪物
    public static readonly int[] Monster = new int[] {0,0,0,0,1,0,1,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,1,1};//26个

    //是否产生金币了
    public static bool[] isBorn = new bool[]{false, false, false, false, false, false, false, false, false, false, false, false, 
        false, false, false, false, false, false, false, false, false, false, false, false, false, false};
    //是否产生怪物了
    public static bool[] isMonsBorn = new bool[]{false, false, false, false, false, false, false, false, false, false, false, false, 
        false, false, false, false, false, false, false, false, false, false, false, false, false, false};
    //是否产生了宝箱
    public static bool[] isBoxBorn = new bool[]{false, false, false, false, false, false, false, false, false, false, false, false, 
        false, false, false, false, false, false, false, false, false, false, false, false, false, false};
  
    //是否产生了陈设
    public static bool[] isProBorn = new bool[]{false, false, false, false, false, false, false, false, false, false, false, false, 
        false, false, false, false, false, false, false, false, false, false, false, false, false, false};


    //各个游戏元素的分值
    public static readonly int SCORE_GOLD = 100;               //金币
    public static readonly int SCORE_STONE = 200;              //石头
    public static readonly int SCORE_TRAP = 200;               //陷阱的分数
    public static readonly int SCORE_LINE = 300;               //在绳子上的分数
    public static readonly int SCORE_MONSTER = 300;            //怪物
    public static readonly int SCORE_BOX = 500;                //装有金币的箱子
    public static readonly int SCORE_SPIDER = 800;             //蜘蛛怪物
    public static readonly int SCORE_DRAGON = 1000;            //飞龙怪物
    
    //特殊变量的定义
    public static int SCORE_TOTAL = 0;      //标示人物的得分, 存在要保存的问题
    public static int BLOCK_COUNT = 0;      //当前遇到的障碍物数目
    public static string NAME_BLOCK = "";   //当前障碍物的名字


    //prefab预加载的资源对象, 
    public static GameObject GoldPrefab = null;               //金币
    public static GameObject MonsterPrefab = null;            //怪物，暂时是个熊
    public static GameObject BlocksPrefab = null;             //障碍物，暂时只有岩石
    public static GameObject WallPrefab = null;               //墙壁预设
    public static GameObject RoadPrefab = null;               //道路预设
    public static GameObject BoxPrefab = null;                //箱子的预设
    public static GameObject PropertyPrefab = null;           //环境陈设的预设
    public static GameObject SpiderPrefab = null;             //蜘蛛怪物
    public static GameObject DragonPrefab = null;             //飞龙的预设
    public static GameObject HousePrefab = null;              //房屋的预设,
    public static GameObject MeDisplayPrefab = null;          //主角的预设
    public static GameObject [] P1 = new GameObject[26];      //每个道路的陈设资源，为对象, array
    public static GameObject SwallFast1Pre = null;            //挥剑时候的特效， 为对象,single
    public static GameObject MonsterBornPre = null;           //怪物出生的特效，为对象,single
    public static GameObject MonsterDiePre = null;            //怪物死亡的紫色特效，为对象，single
    public static GameObject UIPre = null;                    //Ui资源的预设，single

    public static GameObject MeDisplay = null;                //主角的对象，为对象, single
    public static GameObject RoadObject = null;


    //一些标签信息
    public static string sBianhua = "";  //打印的是不断变化的信息

    //障碍物名称,暂时只有这些
    public static string[] BlockName = { "gold", "Rock", "Box", "mSpider", "Trap", "area", "dragon"};

    //一些表的名字：
    public static string TB_RECORD = "recordtb";                //游戏记录的表名

    public static readonly string PRE_GOLD = "Gold";            //金币的预设名称
    public static readonly string PRE_MONSTER = "Monster";      //怪物的预设名称
    public static readonly string PRE_BLOCK = "Blocks";         //障碍物的预设名称
    public static readonly string PRE_BOX = "Box";              //箱子的预设名称
    public static readonly string PRE_SPIDER = "mSpider";       //蜘蛛的预设名称
    public static readonly string PRE_DRAGON = "dragon";        //飞龙的预设名称
}
