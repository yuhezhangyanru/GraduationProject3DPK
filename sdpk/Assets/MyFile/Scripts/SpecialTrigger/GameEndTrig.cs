///author:Yanru Zhang
///date:2014-2-28
///function: binded  to "End" to test if the game is end, then MeDisplay keep idle

using UnityEngine;
using System.Collections;
using myfiles.scripts.ui.mainview;

public class GameEndTrig : MonoBehaviour {

    public delegate void EventHandler(int code); //委托
    public static event EventHandler ProcessUpdate; //事件

    /// <summary>
    /// 绑定在End上，人走到这里就意味着游戏结束，在这里调用游戏结束的相关处理！
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if(DataConst.MeDisplay  == other.gameObject)  //主角跑到这里！
        {
            Debug.Log(Time.time + " game over!");
            Debug.Log(Time.time + " game overre!");
            Debug.Log(Time.time + " game overree!");
            Debug.Log(Time.time + " game oveewr!");
            Debug.Log(Time.time + " game overew!");
            State.KEEP_IDLE = true;  //保持静止！
            DataConst.MeDisplay.transform.Rotate(new Vector3(0, -180, 0));
            DataConst.MeDisplay.GetComponent<Animation>().Play(State.ANIM_GANASTYLE);  //播放动画
            UpdateProcess();  //更新游戏阶段
        }
    }

    private void UpdateProcess()   //表示游戏阶段结束
    {
        if(ProcessUpdate != null)
        {
            ProcessUpdate(CODE.UPDATE_GAME_PROCESS);
        }
    }
}
