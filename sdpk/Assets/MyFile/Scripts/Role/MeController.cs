///author:Yanru Zhang
///date:2014-2-5
///function:binded to MeDisplay, to control role

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using myfiles.scripts.tools;
using myfiles.scripts.ui.mainview;

public class MeController : MonoBehaviour {

    public GameObject mainCam = null; //主摄像机
    public Animation MeAnimation = null;   //主角的animation
    public static int nRate = 10;  //默认的运动的速率
    private Vector3 vDirection = Vector3.forward;    //运动方向默认向前
    private GameObject roadParent = null;
    private GameObject road = null;
    public static List<GameObject> roads = new List<GameObject>();  //新建一个List，用于存放所有的道路
    public static List<GameObject> lines = new List<GameObject>();
    private Collider otherCollider = null;
    
	private RaycastHit hit = new RaycastHit();    //射线命中的点，主要用于上坡和变换道路的检测
    private bool JumpDetection = false;   //是否需要检测陷阱，true时进入检测区
    private BoxCollider MeDisBoxcollider = null;
    private Vector3 lineStart = Vector3.zero;   //碰撞检测的起点
    private Vector3 lineEnd = Vector3.zero;   //碰撞检测的终点

    //定义矩形框数据
    private Rectangle[] rectangles = {new Rectangle(54,59,15.2f,20f), new Rectangle(67.1f,72f,14f,18.8f), new Rectangle(67.1f,72f,41.5f,46f), 
    new Rectangle(42f,46.5f,41.5f,46f), new Rectangle(42.5f,46.7f,8f,13f), new Rectangle(8f,13f,8f,13f), new Rectangle(8f,13.2f,55f,58.7f), 
    new Rectangle(91.2f,96f,53.3f,59f), new Rectangle(91.5f,96f,81.2f,85f), new Rectangle(49.5f,54.4f,80f,85f)};
         
    
    /// <summary>
    /// Start用于对象的实例化和变量的初始化工作
    /// </summary>
	public void Start () {
        //Debug.Log(this.name +"对象：控制角色的脚本"); 
        DataConst.MeDisplay = GameObject.Find("MeDisplay(Clone)"); //.GetComponent<Transform>().gameObject;
        mainCam = GameObject.Find("MainCamera");
        roadParent = DataConst.RoadObject;
        MeAnimation = DataConst.MeDisplay.GetComponent<Animation>();
        MeDisBoxcollider = DataConst.MeDisplay.GetComponent<BoxCollider>();

        //先向后
        //默认播放跑步的动作！！！！！
        Transform[] trans = roadParent.GetComponentsInChildren<Transform>();
        foreach(Transform tran in trans)
        {
            if (("roads(Clone)" != tran.name) && (!tran.name.Contains("Line")) && (!tran.name.Contains("pillar"))) //不是绳子和自己    
                roads.Add(tran.gameObject);
            else if(tran.name.Contains("Line"))
            {
                lines.Add(tran.gameObject);
            }
        }
        road = roads[0];   //默认的当前道路为第一条路，因为就是从第一条路开始走的 
	}

    

    void FixedUpdate()
    {
        Vector3 roleMid = DataConst.MeDisplay.transform.position + new Vector3(0.8f, 1, 0.8f);
        Vector3 roleEnd = DataConst.MeDisplay.transform.position + new Vector3(0.8f, -20, 0.8f);  //足够长的线，如果快到路面也能检测出来
        if (Physics.Linecast(roleMid, roleEnd, out hit, 1))   //这个是必然会执行的，用于检测当前角色碰到了什么东西
        {
            //这条线是一定会画出来的，用于判断是不是角色换了路
            Debug.DrawLine(roleMid, roleEnd, Color.blue);
            if (RoadInList(hit.collider.gameObject))
            {
                road = hit.collider.gameObject;
            }
        }
        //爬坡检测
        //下面的线
        if (otherCollider)
        {
            if ("Road8" == otherCollider.gameObject.name || "Road16" == otherCollider.gameObject.name) //在坡度的路上
            {
                MeDisBoxcollider.enabled = false; //禁用触发器
                UpdatePosOnRoad();  //更新角色位置
            }
            else            //在水平路面上，前方横放置一条射线，用于检测是否靠近障碍物
            {
            }
        }
    }

    ///<summary>用于更新角色的运动状态</summary>
	// Update is called once per frame
    void Update()    //用于监听对人物的控制事件
    {
        UpdateAction();  //更新角色动作
        
        //!problem这个问题暂时搁浅
        ///身体倾斜控制水平位移
        ///f键：向左偏移
        ///g键：向右偏移
        if (Input.GetKeyDown("f") && State.OFFSET_CURRENT != State.OFFSET_LEFT)  //向左偏移,当POS_CURRENT == POS_LEFT的时候表示已经到了最左边了
        {
            transform.localPosition -= 0.3f * ToolsFunction.OffsetPosiion(DataConst.MeDisplay, 0);
        }
        if(Input.GetKeyDown("g") && State.OFFSET_CURRENT != State.OFFSET_RIGHT) //向右偏移
        {
            transform.localPosition += 0.3f * ToolsFunction.OffsetPosiion(DataConst.MeDisplay, 1);
        }

        if(false == State.OFFSET_IDLE && false == State.KEEP_IDLE)  //表示没有撞到墙, 并且没有撞到障碍物
        {
            transform.Translate(vDirection / nRate);   //暂时设定为100
        }


        if(Input.GetKeyDown("t"))  //用于测试的按钮
        {
            Rectangle rec = new Rectangle(1,1,1,1);
            Debug.Log(Time.time + " rectangle side: " + rec.X_Min);
            Debug.Log(Time.time + " rectangle side: " + rec.X_Max);

        }
    }





    //这个函数以后不用了！！！！！！！！！！！！！！！！！
    //public void TrapDetection()
    //{
    //    //以下检测是否在规定区域起跳了
    //    //int n = (int)MeDisplay.transform.eulerAngles.y;
    //    Vector3 v1 = new Vector3(0, 1, 6f);
    //    Vector3 v2 = new Vector3(0, 1, 4f);
    //    Vector3 trapS1 = MeDisplay.transform.position + v1;// new Vector3(0, 1, 6f);
    //    Vector3 trapS2 = MeDisplay.transform.position + v2;//new Vector3(0, 1, 4f);
    //    Vector3 trapE = new Vector3(0, 20, 0);     //向下画的射线的长度
    //    if ((Physics.Linecast(trapS1, trapS1 - trapE, out hitStart, 1)) && (Physics.Linecast(trapS2, trapS2 - trapE, out hitEnd, 1)))
    //    {
    //        Debug.DrawLine(trapS1, trapS1 - trapE, Color.green);
    //        Debug.DrawLine(trapS2, trapS2 - trapE, Color.white);
    //    }
    //    //如果在jump的检测区域：条件，hitEnd在地面，hitStart不等于当前走的路且不等于地面
    //    if(hitStart.collider && hitEnd.collider)
    //    if (false == isLock && hitStart.collider.gameObject && MeDisplay.transform.eulerAngles.y == 0)  //人物的旋转角度为0
    //        JumpKeepIdle(hitStart.collider.gameObject.name, hitEnd.collider.gameObject.name, hitStart.collider.gameObject);
    //}
  
    //这个函数已经无用。。
    /*
     * 用于判断role没有在该跳的区域起跳，就保持静止
     * */
    //private int JumpKeepIdle(string targetS, string targetE, GameObject go)
    //{
    //    Vector3 roleMid = MeDisplay.transform.position + new Vector3(0f, 1, 0.8f);
    //    Vector3 roleEnd = MeDisplay.transform.position + new Vector3(0f, -20, 0.8f);  //足够长的线，如果快到路面也能检测出来
    //    Physics.Linecast(roleMid, roleEnd, out hit, 1);   //这个是必然会执行的，用于检测当前角色碰到了什么东西
    //    if(hit.collider)
    //    if(((road.name == hit.collider.gameObject.name)&&(NextRoad(road.name)==targetE))||((hit.collider.gameObject.name==targetE)&&(road.name == targetE)))
    //    {
    //        //ShowInfo("不做任何事");
    //    }
    //    //else if(((IsaRoad(targetE)) && (!IsaRoad(targetS)) && (road.name != targetS))&&hit.collider.gameObject)
    //    else if ((("Terrain" == targetE) && ("Terrain" != targetS) && (road.name != targetS)) && hit.collider.gameObject)
    //    {
    //        ShowInfo("需要检测跑步的区域");
    //        if (false == JumpDetection)
    //            State.IS_JUMP = false;

    //        JumpDetection = true;
    //        //在此范围内如果起跳的话，
    //        Vector3 v2 = new Vector3(0, 0, -6f);
    //        //if (MeDisplay.transform.eulerAngles.y == 270)
    //        //    v2 = new Vector3(8f, 0, 0);
    //        v = go.transform.position + v2;
    //        v.y = 2.13f;  //暂时写死
    //        ShowInfo("JumpDetection = "+JumpDetection+",    isJump = "+State.IS_JUMP);
    //        if ((true == JumpDetection) && (true == State.IS_JUMP))
    //        {
    //                ShowInfo("应该起跳了");
    //                MeDisplay.transform.position = v;// go.transform.position;
    //                JumpDetection = false;   //执行了跳的动作之后就应该复位
    //                State.IS_JUMP = false;
    //        }



    /// <summary>
    /// 更新MeDisplay在路面的位置，主要是在爬坡的时候才会调用
    /// </summary>
    private void UpdatePosOnRoad()
    {
        //首先判断当前路的状态
        //注意：统一：上升的角度-20， 下降的角度， 20
        if ((road.transform.eulerAngles.x >= 18) && (road.transform.eulerAngles.x <= 21))  //角度为20，表示上坡路
        {
            //上升的射线位置
            lineStart = (DataConst.MeDisplay.transform.position + new Vector3(0.3f, 1, 0.8f));
            lineEnd = DataConst.MeDisplay.transform.position + new Vector3(0.3f, -0.5f, 0.5f);
        }
        else if ((road.transform.eulerAngles.x >= 338) && (road.transform.eulerAngles.x <= 341))   //角度为-20，表示下坡路
        {
            //下降的射线角度
            lineStart = (DataConst.MeDisplay.transform.position + new Vector3(0.6f, 1, -0.8f));
            lineEnd = DataConst.MeDisplay.transform.position + new Vector3(0.6f, -1f, -0.5f);
        }
        else if (ToolsFunction.EqualFloat(road.transform.eulerAngles.x, 0)) //((road.transform.eulerAngles.x >= -1) && (road.transform.eulerAngles.x <= 1))   //角度为0, 平坦的路
        {
            MeDisBoxcollider.enabled = true;
        }
        if (Physics.Linecast(lineStart, lineEnd, out hit, 1))
        {
            if (road.name != "Road9" && road.name != "Road17") 
            if (road.name == hit.collider.gameObject.name && State.KEEP_IDLE == false) //这个时候暂时加上标记量，避免   //碰到了某一条路
            {
                //Debug.Log(Time.time + "执行到！"+road.name);
                Debug.DrawLine(lineStart, lineEnd, Color.yellow);
                Vector3 tempV = transform.position;
                tempV.y = hit.point.y;
                transform.position = tempV;
            }
            else if ("MeDisplay(Clone)" == hit.collider.gameObject.name)  //碰到的是自己，就忽略掉
            {
            }
            else if ("test" == hit.collider.gameObject.name)     //此时碰到的是除了路之外的物体，但是包括绳子
            {
                Debug.Log(Time.time + "已经脱离了road");
            }
        }
    }

    /// <summary>
    /// 检测前方是否即将有障碍物出现，给出提示
    /// </summary>
    public void UpdateRockCheck()
    {
        if ("1" == SettingTb.Element[SettingTb.INDEX_HINT_TIP])   //表示隐藏主界面消息提示，直接不显示提示了
            return;
        lineStart = DataConst.MeDisplay.transform.position + 2 * ToolsFunction.Toward(DataConst.MeDisplay) + 2f * ToolsFunction.ToLeft(DataConst.MeDisplay)+new Vector3(0, 0, -2);
        lineEnd = DataConst.MeDisplay.transform.position + 2 * ToolsFunction.Toward(DataConst.MeDisplay) - 2f * ToolsFunction.ToLeft(DataConst.MeDisplay) + new Vector3(0, 0, -2);

        Debug.DrawLine(lineStart, lineEnd); 
        if (Physics.Linecast(lineStart, lineEnd, out hit, 1))
        {
            //判断是不是障碍物
            string blockName = hit.collider.gameObject.name;
            ToolsFunction.Log(this.gameObject, "line cast name: " + blockName);  //障碍物名称
            if(ToolsFunction.IsBlock(blockName))
            {
                MainTipControl.StartAnim(MainView.MainTip, MainView.BlockHead);
            }
        }
    }

    /*
     * 更新MeDisplay的动作
     * */
    public void UpdateAction()
    {
        ///以下主要为控制运动方向, 相对于自身的方向
        ///a：左转
        ///d：右转
        ///w：跳起
        ///s：躺倒
        if (Input.GetKeyDown("a")) //左转
        {
            ///是否在区域内
            if (InTrunArea(transform.localPosition.x, transform.localPosition.z))
            {
                transform.Rotate(new Vector3(0, -90, 0));
                State.OFFSET_IDLE = false;
                State.KEEP_IDLE = false;
            }
        }
        if (Input.GetKeyDown("d")) //右转
        {
            ///是否在区域内
            if (InTrunArea(transform.localPosition.x, transform.localPosition.z))
            {
                transform.Rotate(new Vector3(0, 90, 0));
                State.OFFSET_IDLE = false;
                State.KEEP_IDLE = false;
            }
        }
        if (Input.GetKeyDown("w")) //跳起
        {
            State.ON_JUMP = true;  ///表示在跳的状态
            State.KEEP_IDLE = false;
            if(true == JumpDetection)  //表示正在检测跑步的区域
            {
                State.IS_JUMP = true;
            }
            SetAction(MeAnimation, State.ANIM_JUMP);
        }
        if (Input.GetKeyDown("s")) //躺倒的动作
        {
            if (MeAnimation)
            {
                SetAction(MeAnimation,  State.ANIM_SLIDE);
            }
        }
        if(Input.GetKeyDown("z"))   //执行锤击宝箱的动作
        {
            MeAnimation.Play(State.ANIM_FIGHT);
            State.ON_FIGHT = true;
            State.KEEP_IDLE = false;
        }
    }


    //用于打印测试信息
    public void ShowInfo(string str)
    {
        Debug.Log(Time.time + ":  " + str);
    }


    /// <summary>
    /// MeDisplay的触发器函数，主要用于滑绳的碰撞检测产生金币
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        otherCollider = other;  // 当前碰到的物体
        ToolsFunction.CreateGold(DataConst.MeDisplay, other);
        //以下调用产生怪物的函数
        //这个函数暂时屏蔽
        //解锁偏移锁定
        ToolsFunction.UnLockOffset(otherCollider);
        //检测障碍物和当前所处的游戏阶段
        ToolsFunction.BlockJudge(other.gameObject.name);
    }


    //表示是否存在这样的道路
    public bool RoadInList(GameObject go)
    {
        foreach(string name in DataConst.RoadNames)
        {
            if(name == go.name)
            {
                return true;
            }
        }
        return false;
    }


    public string NextRoad(string presentRoadName)
    {
        for (int i = 0; i < DataConst.RoadNames.Length; i++) 
        {
            if (DataConst.RoadNames[i] == presentRoadName && i != DataConst.RoadNames.Length-1)
            {
                return DataConst.RoadNames[i + 1];
            }
        }
        return null;
    }

    /// <summary>
    /// 将go对象移动到指定点target，按速率2f
    /// </summary>
    /// <param name="go">需要移动的对象</param>
    /// <param name="target">移动的目标点</param>
    /// <param name="callBack">移动结束的回调函数</param>
    /// <param name="param">回调函数传入的参数</param>
    public static void MoveTo(GameObject go, Vector3 target, float duration)
    {
        Hashtable args = new Hashtable();
        args.Add("easeType", iTween.EaseType.easeInOutExpo);
        args.Add("speed", duration);
        args.Add("delay", 0.0001f);
        args.Add("loopType", "none");
        args.Add("position", target);  //移动的目标点
        //动画播放的回调函数
        args.Add("oncomplete", "AnimationEnd");
        args.Add("oncompleteparams", "end");
        args.Add("oncompletetarget", go);
        //最终让改对象开始移动
        iTween.MoveTo(go, args);
    }


    /// <summary>
    /// 其他动作结束，恢复到run状态
    /// </summary>
    /// <param name="msg"></param>
    public void AnimationEnd(string msg)
    {
        ShowInfo(msg + Time.time);
        State.KEEP_IDLE = false;
        MeAnimation.Play(State.ANIM_RUN);  //恢复到跑步状态
        Debug.Log(Time.time + "走完绳子，恢复触发器");
        MeDisBoxcollider.enabled = true;
        //还要恢复上一次跳跃置的标记值, 因为跳绳执行的那次操作没有动作回掉
        State.ON_JUMP = false;
        State.ON_FIGHT = false;
    }


    /// <summary>
    /// 主要用于设定除了Run之外的动作，他们执行完毕后回调函数中都会结束当前动作恢复到跑步状态
    /// </summary>
    /// <param name="action"></param>
    public static void SetAction(Animation MeAnimation, string action)
    {
        MeAnimation.Play(action);
    }


    /// <summary>
    /// 一个打击的动作结束之后的回调函数，会调用下一个动作，如果到末尾了就变成跑步的状态, 虽然暂时不做处理，也不要删除
    /// </summary>
    public void AttackEnd()
    {
        if (State.nAnimIndex <= 1)
        {
            MeAnimation.Play(State.ANIM_ATTACK[State.nAnimIndex]);
            State.nAnimIndex++;
        }
        else
        {
            State.ON_FIGHT = false;
            MeAnimation.Play(State.ANIM_RUN);
            State.nAnimIndex = 0;
        }
        //此处要隐藏特效
        DataConst.SwallFast1Pre.SetActive(false);
        State.ON_FIGHT = false;
    }

    public void ActionEndCallback()
    {
        MeAnimation.Play(State.ANIM_RUN);
        //动作状态重置
        State.ON_JUMP = false;
        State.ON_FIGHT = false;
    }

    IEnumerator ResetRun()
    {
        yield return new WaitForSeconds(1f);
        //设置为跑步
        MeAnimation.Play(State.ANIM_RUN);
    }


    /// <summary>
    /// 播放主角打斗的时候的特效动画
    /// </summary>
    public void PlayEffect()
    {
        ToolsFunction.Log(this.gameObject, "play sword effect!");
        DataConst.SwallFast1Pre.SetActive(true);
    }


    /// <summary>
    /// 判断当前的坐标是否在限定区域内
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private bool InTrunArea(float x, float z) 
    {
        for (int index = 0; index < rectangles.Length; index++)
        {
            //找到一个在区域内的
            if ((x >= rectangles[index].X_Min && x <= rectangles[index].X_Max) && (z >= rectangles[index].Z_Min && z > rectangles[index].Z_Min))
            {
                Debug.Log(Time.time + "---在！在区域内！");
                return true;
            }
        }
        Debug.Log(Time.time + " 不在区域内！不可以转弯！");
        return false;
    }

}
//注意：MeDisplay在爬坡的时候要被禁用，不然会出问题