///author:Yanru Zhang
///date2014-2-22
///function: some common useful function 

using UnityEngine;
using System.Collections;
using System;
using myfiles.scripts.ui.mainview;
using System.Collections.Generic;

namespace myfiles.scripts.tools
{
    public class ToolsFunction : MonoBehaviour
    {
        public delegate void EventHandler(int code); //委托
        public static event EventHandler ScoreUpdate; //事件
        private static string preBlockName = "";
        private static int index = 0;   //用于产生障碍物时的道路下标
        
        /// <summary>
        ///数据更新的调用函数
        /// </summary>
        /// <param name="code">需要更新的业务代码</param>
       private static void DataUpdateCode(int code)
        {
            if (ScoreUpdate != null)
            {
                ScoreUpdate(code);
            }
        }


        public int nPreScore = 0;  //之前的分数

        ///<summary>判断两个浮点数是不是相等</>  
        ///低精度判断，所以活动范围在2个单位
        ///<param name="fSrc">源数据变量</param>
        ///<param name="fDst">目的数据常量</param>
        public static bool EqualFloat(float fSrc, float fDst)
        {
            return ((fSrc >= fSrc - 1.0f) && (fSrc <= fDst + 1.0f));
        }

        /// <summary>
        /// 高精度判断，活动范围在0.1之间
        /// </summary>
        /// <param name="fSrc">源数据变量</param>
        /// <param name="fDst">目的数据变量</param>
        /// <returns></returns>
        public static bool EqualHighFloat(float fSrc, float fDst)
        {
            return ((fSrc >= fSrc - 0.1f) && (fSrc <= fDst + 0.1f));
        }


        /// <summary>
        /// 判断num1的值是否介于min和max之间
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool BetweenTwo(int num1, int min, int max)
        {
            if (num1 >= min && num1 <= max)
                return true;
            else
                return false;
        }

        ///<summary>
        ///用于更新角色的水平偏移位置，默认为水平向右，
        ///</summary>
        ///<param name="go">要偏移的对象</param>
        ///<param name="nToFoward">表示对象想要的朝向，=0， 向左； =1，向右, 只有滑绳子的时候用到</param>
        public static Vector3 OffsetPosiion(GameObject go, int nToFoward)
        {
            //首先应当判断是不是在绳子上，绳子上用的是旋转，然后再进行普通状态的判断
            //暂时取消绳子上偏移的功能！
            if (EqualFloat(go.transform.eulerAngles.y, 0))   //0度
            {
                return Vector3.right;
            }
            else if (EqualFloat(go.transform.eulerAngles.y, 90))   //90度
            {
                return Vector3.back;
            }
            else if (EqualFloat(go.transform.eulerAngles.y, 180))   //180度
            {
                return Vector3.left;
            }
            else if (EqualFloat(go.transform.eulerAngles.y, 270))   //270度
            {
                return Vector3.forward;
            }
            else return Vector3.zero;   //否则不变动，这种情况应该不会变动
        }


        /// <summary>
        ///得到两个对象在水平面，即y轴的夹角
        /// </summary>
        /// <param name="go1">第一个对象，默认将MeDisplay放在第一位</param>
        /// <param name="go2">第二个对象</param>
        /// <returns></returns>
        public static int IntersectionAngle(GameObject go1, GameObject go2)
        {
            return (int)Math.Abs(((go1.transform.eulerAngles.y) - (go2.transform.eulerAngles.y)));  //返回绝对值
        }


        /// <summary>
        /// 用于方便打印的函数
        /// </summary>
        /// <param name="str"></param>
        public static void ShowInfo(string str)
        {
            Debug.Log(Time.time + str);
        }


        /// <summary>
        /// 产生金币的函数，当该路有金币的时候有效调用
        /// </summary>
        /// <param name="other"></param>
        public static void CreateGold(GameObject me, Collider other)
        {
            index = Array.IndexOf(DataConst.RoadNames, other.gameObject.name);
            if (index == 15)  //已经第一阶段的结束点
            {
                PassOne();
                return; //这是一个斜坡，这条路上不会有障碍物，直接return
            }
            if (-1 != index && index != 15 && index != 17)  //表示当前碰到的确实是一条路
            {
                if (false == DataConst.isProBorn[index])
                {
                    LoadPropery(index);   //加载道路陈设
                    DataConst.isProBorn[index] = true;
                }
                //产生金币
                Quaternion rotation = Quaternion.Euler(Toward(DataConst.MeDisplay));   //获取当前人的反方向
                Vector3 foward = Toward(DataConst.MeDisplay);
                if (Vector3.zero != RoadTb.RoadList[index].gold_pos)//DataConst.GoldBornPos[index])  //表示这条路可以产生金币
                {
                    if (false == DataConst.isBorn[index])
                    {
                        //在产生新的金币之前，也要销毁之前没有打光的金币
                        DestroyOldGold();
                        //GameObject go1 = ObjectManager.ObjInList(DataConst.PRE_GOLD);
                        //go1.transform.localPosition = RoadTb.RoadList[index].gold_pos;
                        //go1.transform.rotation = rotation;
                        GameObject go1 = Instantiate(DataConst.GoldPrefab, RoadTb.RoadList[index].gold_pos, rotation) as GameObject;
                        go1.transform.eulerAngles = DataConst.MeDisplay.transform.eulerAngles;
                        DataConst.isBorn[index] = true;  //只产生一次金币
                    }
                }
                //产生怪物
                if (Vector3.zero != RoadTb.RoadList[index].mon_pos)//DataConst.GoldBornPos[index])  //表示这条路可以产生金币
                {
                    if (false == DataConst.isMonsBorn[index])
                    {
                        if (WillBornDragon(index))   //表示为产生飞龙的路段
                        {
                            DataConst.MonsterBornPre.SetActive(true); //开始播放出生的烟雾特效
                            //GameObject go1 = ObjectManager.ObjInList(DataConst.PRE_DRAGON);
                            //go1.transform.localPosition = RoadTb.RoadList[index].mon_pos;
                            //go1.transform.rotation = rotation;
                            GameObject go1 = Instantiate(DataConst.DragonPrefab, RoadTb.RoadList[index].mon_pos, rotation) as GameObject;
                            go1.transform.Rotate(DataConst.MeDisplay.transform.eulerAngles + new Vector3(0, 180, 0));
                            go1.SetActive(false);
                            DataConst.MonsterBornPre.transform.localPosition = go1.transform.localPosition;
                            EMonsterBorn.SetMonster(go1);
                            DataConst.isMonsBorn[index] = true;
                        }
                        else   //产生蜘蛛
                        {
                            DataConst.MonsterBornPre.SetActive(true); //开始播放出生的烟雾特效
                            //GameObject go1 = ObjectManager.ObjInList(DataConst.PRE_SPIDER);
                            //go1.transform.localPosition = RoadTb.RoadList[index].mon_pos;
                            //go1.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

                            GameObject go1 = Instantiate(DataConst.SpiderPrefab, RoadTb.RoadList[index].mon_pos, Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

                            go1.transform.eulerAngles = DataConst.MeDisplay.transform.eulerAngles;
                            go1.SetActive(false);
                            EMonsterBorn.SetMonster(go1);
                            DataConst.isMonsBorn[index] = true;
                        }
                    }
                }
                //产生宝箱
                if (Vector3.zero != RoadTb.RoadList[index].box_pos)//DataConst.GoldBornPos[index])  //表示这条路可以产生金币
                {
                    Debug.Log(Time.time + "应该产生宝箱了！");
                    if (false == DataConst.isBoxBorn[index])
                    {
                        //GameObject go1 = ObjectManager.ObjInList(DataConst.PRE_BOX);
                        //go1.transform.localPosition = RoadTb.RoadList[index].box_pos;
                        //go1.transform.rotation = rotation;
                        GameObject go1 = Instantiate(DataConst.BoxPrefab, RoadTb.RoadList[index].box_pos, rotation) as GameObject;
                        go1.transform.eulerAngles = DataConst.MeDisplay.transform.eulerAngles;
                        DataConst.isBoxBorn[index] = true;  //只产生一次金币
                    }
                }
            }
        }


        /// <summary>
        /// 通过了第一阶段的跑步，提示一个的动画，并且让人物静止，当动画播放完就继续跑步
        /// </summary>
        private static void PassOne()
        {
            Debug.Log(Time.time + " 恭喜你已经通过第一阶段了！");
            Debug.Log(Time.time + " 恭喜你已经通过第一阶段了！1");
            Debug.Log(Time.time + " 恭喜你已经通过第一阶段了2！");
            Debug.Log(Time.time + " 恭喜你已经通过第一阶段了！3");
            Debug.Log(Time.time + " 恭喜你已经通过第一阶段了！4");
            Debug.Log(Time.time + " 恭喜你已经通过第一阶段了！5");
            //MainView.ResetCountAnimation();
        }


        /// <summary>
        /// 返回当前路段是否要产生一个飞龙，true为是，else产生蜘蛛
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool WillBornDragon(int index)
        {
            List<int> n = new List<int>(){ 0, 2, 12, 19};
            return n.Contains(index) ? true : false;
        }



        /// <summary>
        /// 加载道路周围的陈设, 产生新的陈设也销毁过去的两段路的陈设
        /// </summary>
        /// <param name="roadIndex">道路下表</param>
        private static void LoadPropery(int roadIndex)
        {
            //加载陈设
            GameObject propertyObj1 = Instantiate(DataConst.P1[roadIndex], Vector3.zero, DataConst.P1[roadIndex].transform.rotation) as GameObject;
            //销毁陈设
            if(roadIndex-2 >= 0)   //防止出界
            {
                string str = "P" + (roadIndex - 1).ToString()+"(Clone)";
                Debug.Log(Time.time + "当前要销毁的对象：" + str);
                Destroy(GameObject.Find(str));
            }
        }

        /// <summary>
        /// 销毁金币
        /// </summary>
        public static void DestroyOldGold()
        {
            GameObject go = GameObject.Find("Gold(Clone)");
            Destroy(go);
        }

        /// <summary>
        /// 用于得到当前路的序列号
        /// </summary>
        /// <param name="roadName">路的名字</param>
        /// <returns></returns>
        public static int GetRoadNum(string roadName)
        {
            return Array.IndexOf(DataConst.RoadNames, roadName);
        }


        /// <summary>add
        ///执行了跳的动作， 判断当前的是什么障碍物，来决定怎么加分，绕过障碍物,完了过后同时恢复keep_idle = false
        /// </summary>
        /// <param name="me"> 我自己的对象</param>
        /// <param name="other">碰到的障碍物，根据障碍物的类型判断和决定给多少加分</param>
        //public static void JudgeBlock(GameObject me, Collider other)
        //{
        //    //跳过障碍物了
        //    //ShowInfo("跳过障碍物了！");
        //    me.transform.position += 2.3f * Toward(me.gameObject);

        //    State.IS_JUMP = false;  //起跳动作解锁
        //    State.KEEP_IDLE = false;  //动作解锁
        //    //State.KEEP_ACTIVE = false;  //触发归位

        //    //要判断当前的方向决定在哪个方向加，
        //    //还没有判断是什么障碍物！给加分！
        //    DataConst.SCORE_TOTAL += AddScore(other.gameObject.name);
        //    ShowInfo("当前的得分：" + DataConst.SCORE_TOTAL);
        //}


        /// <summary>
        /// 返回当前对象的前方，主要用于绕过障碍物的时候
        /// </summary>
        /// <param name="go">需要获取朝向的对象</param>
        public static Vector3 Toward(GameObject go)
        {
            if (EqualFloat(go.transform.eulerAngles.y, 0))   //0度
            {
                return Vector3.forward;
            }
            else if (EqualFloat(go.transform.eulerAngles.y, 90))   //90度
            {
                return Vector3.right;
            }
            else if (EqualFloat(go.transform.eulerAngles.y, 180))   //180度
            {
                return Vector3.back;
            }
            else if (EqualFloat(go.transform.eulerAngles.y, 270))   //270度
            {
                return Vector3.left;
            }
            else return Vector3.zero;   //否则不变动，这种情况应该不会变动
        }

        /// <summary>
        /// 获取当前游戏对象的左方
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static Vector3 ToLeft(GameObject go)
        {   
            if (EqualFloat(go.transform.eulerAngles.y, 0))   //0度
            {
                return Vector3.left;
            }
            else if (EqualFloat(go.transform.eulerAngles.y, 90))   //90度
            {
                return Vector3.forward;
            }
            else if (EqualFloat(go.transform.eulerAngles.y, 180))   //180度
            {
                return Vector3.right;
            }
            else if (EqualFloat(go.transform.eulerAngles.y, 270))   //270度
            {
                return Vector3.back;
            }
            else return Vector3.zero;   //否则不变动，这种情况应该不会变动
        }

        /// <summary>
        /// 产生怪物的函数， 暂时测试的时候怪物的后面也会产生宝箱的！！！！！
        /// </summary>
        /// <param name="go">道路对象</param>
        public static void CreateMonster(GameObject go)
        {
            int index = Array.IndexOf(DataConst.RoadNames, go.name);  //返回路的下表
            //应该是判断当前数字钟改下标元素是否为1,暂时用if代替
            //if (1 == DataConst.Monster[index] && false == DataConst.isMonsBorn[index])  //这条路可以有没有怪物且没有产生怪物
            //if ( && false == DataConst.isMonsBorn[index] && index != 15)
            if (-1 != index)
            {
                Debug.Log("要产生怪物");
                if (Vector3.zero != RoadTb.RoadList[index].mon_pos)
                {
                    Vector3 v = go.transform.eulerAngles + new Vector3(0, 180, 0);
                    //GameObject go1 = ObjectManager.ObjInList(DataConst.PRE_MONSTER);
                    //go1.transform.localPosition = go.transform.position + new Vector3(0, 1.3f, 0);
                    //go1.transform.rotation = Quaternion.Euler(v);
                    GameObject go1 = Instantiate(DataConst.MonsterPrefab, go.transform.position + new Vector3(0, 1.3f, 0),
                        Quaternion.Euler(v)) as GameObject;
                    DataConst.isMonsBorn[index] = true;

                    //暂时也跟着加一个宝箱！！！
                    //GameObject go2 = ObjectManager.ObjInList(DataConst.PRE_BOX);
                    //go2.transform.localPosition = go1.transform.position + 2f * ToolsFunction.Toward(go1) + new Vector3(0, 0.5f, 0);
                    //go2.transform.rotation = Quaternion.Euler(v);
                    GameObject go2 = Instantiate(DataConst.BoxPrefab, go1.transform.position + 2f * ToolsFunction.Toward(go1) + new Vector3(0, 0.5f, 0),
                        Quaternion.Euler(v)) as GameObject;
                }
            }
        }


        /// <summary>
        /// 离开墙壁之后要解锁Offset
        /// </summary>
        /// <param name="other"></param>
        public static void UnLockOffset(Collider other)
        {
            if ("1" != other.gameObject.name && "2" != other.gameObject.name)
            {
                State.OFFSET_CURRENT = State.OFFSET_CEN;
            }
        }


        public static void Log(GameObject go, string str)
        {
            Debug.Log(go.name + ":  " + str + "(" + Time.time + ")");
        }

        public static void Log(string str1, string str)
        {
            Debug.Log(str1 + ":  " + str + "(" + Time.time + ")");
        }


        /// <summary>
        /// 处理加分的函数
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public static int AddScore(int score)
        {
            DataConst.SCORE_TOTAL += score;
            if (DataConst.SCORE_TOTAL <= 0)
                DataConst.SCORE_TOTAL = 0;

            DataUpdateCode(CODE.UPDATE_SCORE);    //更新分数

            //向view发送消息
            return DataConst.SCORE_TOTAL;
        }

        /// <summary>
        /// 用于判断当前路所处阶段，将百分比更新
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static void BlockJudge(string block)
        {
            if (IsBlock(block) && DifferentBlock(block))   //所有碰到的障碍物只给提示一次，多了不管
            {
                DataConst.BLOCK_COUNT += 1;
                DataConst.NAME_BLOCK = block;    //把障碍物名字复制给当前障碍物
                DataUpdateCode(CODE.UPDATE_BLOCK);

            }
            else if(IsRoad(block))   //是路
            {
                Debug.Log(Time.time + "是一个道路!" + GetRoadNum(block) + 1);
                State.PROCESS_PERCENT = GetRoadNum(block) + 1;  //因为下标从0开始，这里要加1
                DataUpdateCode(CODE.UPDATE_PROCESS);
            }
        }

        /// <summary>
        /// 判断当前障碍物是不是刚刚已经撞到的，如果是，返回false，不再提示；新障碍物，返回true，提示
        /// </summary>
        /// <param name="blockNow"></param>
        /// <returns></returns>
        private static bool DifferentBlock(string blockNow)
        {
            if (blockNow == preBlockName)
            {
                return false;
            }
            else
            {
                preBlockName = blockNow;
                return true;
            }
        }

        /// <summary>
        /// 判断名为name的对象是不是障碍物
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsBlock(string name)
        {
            foreach (string str in DataConst.BlockName)
            {
                if(name.Contains(str))   //用包含，不用==，因为创建的对象中包含（Clone）
                {
                    return true;
                }
            }
            return false;
        }

        //是否存在这样的路名
        public static bool IsRoad(string sName)
        {
            foreach (string name in DataConst.RoadNames)
            {
                if (name == sName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 用于返回当前对象指定名字的子对象
        /// </summary>
        /// <param name="pRoot">根对象</param>
        /// <param name="pName">子对象名称</param>
        /// <returns></returns>
        public static GameObject FindChild(GameObject pRoot, string pName)
        {
            if (pRoot)
            {
                return pRoot.transform.Find(pName).gameObject;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 将秒数转为00:00时间格式
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static string TimeFormat(int second)
        {
            Debug.Log(Time.time + " second: " + second.ToString() + " tos: " + AddScore(second / 60));
            int min = second / 60;
            string sMin = min < 10 ? "0" + min.ToString() : min.ToString();
            int sec = second % 60;
            string sSec = sec < 10 ? "0" + sec.ToString() : sec.ToString();
            return (sMin + ":" + sSec);
        }


        /// <summary>
        /// 根据分数计算游戏结果的等级
        /// </summary>
        /// <param name="nScore">分数</param>
        /// <param name="nTime">游戏所用的时间</param>
        public static string GetLevel(int nScore, int nBlock, int nTime)
        {
            float level = (float)((nScore*nBlock)/nTime);
            Debug.Log(Time.time + " 游戏系数：" + level.ToString());
            if (level > 200)
            {
                return "1";
            }
            else if (level > 150)
            {
                return "2";
            }
            else if (level > 120)
            {
                return "3";
            }
            else return "4";
        }
    }
}