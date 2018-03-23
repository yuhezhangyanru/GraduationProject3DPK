using UnityEngine;
using System.Collections;

public class State : MonoBehaviour {
    //以下的状态表示人的animation状态
    //public static readonly int ANIM_JUMP = Animator.StringToHash("Base Layer.jump_1");  //跳起
    //public static readonly int ANIM_SLIDE = Animator.StringToHash("Base Layer.slide_1");   //身体滑到
    //public static readonly int ANIM_HANDS_UP = Animator.StringToHash("Base Layer.hands_up_1");   //双手抓绳子
    //public static readonly int ANIM_FIGHT = Animator.StringToHash("Base Layer.fight_1");  //爆发时候打击怪物的动作


    public static readonly string ANIM_RUN = "Run00";
    public static readonly string ANIM_JUMP = "Jump_NoBlade";//Animator.StringToHash("Base Layer.jump_1");  //跳起
    public static readonly string ANIM_SLIDE = "Idle";// 这个动作最终会被替换成别的动作！！！！！Animator.StringToHash("Base Layer.slide_1");   //身体滑到
    public static readonly string ANIM_HANDS_UP = "Idle";  //暂时用静止代替 // Animator.StringToHash("Base Layer.hands_up_1");   //双手抓绳子
    public static readonly string ANIM_FIGHT ="Attack";  //Animator.StringToHash("Base Layer.fight_1");  //爆发时候打击怪物的动作
    public static readonly string ANIM_PICK_UP = "Pickup";  //开宝箱时候的动作
    public static readonly string ANIM_DEMAGE = "Damage";  //在碰到障碍物或受击的动作
    public static readonly string ANIM_GANASTYLE = "GanamStyle";
    public static readonly string[] ANIM_ATTACK = new string[] {"Attack", "Attack00", "Attack01"};

    public static int nAnimIndex = 0;

    //表示人在跑道中的偏移位置
    public static int OFFSET_CURRENT = 3;   //这个状态是可以被修改的, 默认为0的时候表示在中间，要考虑修改后什么时候复位！！！    
    public static readonly int OFFSET_LEFT = 1;  //左
    public static readonly int OFFSET_RIGHT = 2;  //右
    public static readonly int OFFSET_CEN = 3;  //表示在路当中
    public static bool OFFSET_IDLE = false; //表示人垂直撞到了墙上，就不应该运动,当执行转向操作的时候才能解锁

    //是否已经转弯了
    public static bool TURN_CLREADY = false; //为true的时候表示不需要再转弯了


    //角色执行的动作状态
    public static bool IS_JUMP = false;   //执行了跳的动作
    public static bool ON_JUMP = false;  //在调的过程中。跳动作完毕在回调函数中重置
    public static bool ON_FIGHT = false;  //正在打击状态


    //以下4个状态用于标识人在滑绳索的时候
    //public static int JUMP_STATION = 0;  //表示人跑步的状态， 可被修改
    //public static readonly int JUMP_NO_ACTION = 0;
    //public static readonly int JUMP_IS_UP = 1;
    //public static readonly int JUMP_IN_AREA = 2;


    //当这个状态为true的时候保持静止，否则会继续运动
    public static bool KEEP_IDLE = true;
    public static bool KEEP_ACTIVE = false;

    //人物在遇到怪物的时候， 为true表示执行了打击的动作
    public static bool ACT_FIGHT = false;

    //人当前所处的游戏的阶段
    public static int PROCESS_PERCENT = 0;

    //人物在遇到宝箱的时候，为true表示执行了”锤击的动作“
    //public static bool ACT_BOX = false;

    //在石头前的检测成功了，就禁用石头的boxcollider
    //public static bool UNABLE_ROCK = false;
}
