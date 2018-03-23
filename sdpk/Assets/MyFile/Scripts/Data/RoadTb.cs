using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadTb : MonoBehaviour {

    public struct RoadTb_1
    {
        public int road_id;  //道路的ID，序号
        public string road_name;  //道路名字
        public Vector3 gold_pos;
        public Vector3 box_pos;
        public Vector3 mon_pos;
        public int is_gold;  //是否已经产生金币，0尚未产生
        public int monster;  //是否会有怪物，0，没有
        public int is_monster;  //是否已经产生产生怪物
        public int box; //是否有怪物
        public int is_box; 

    }
    public static RoadTb_1 road = new RoadTb_1();
    public static List<RoadTb_1> RoadList = new List<RoadTb_1>(); //路的列表
}
