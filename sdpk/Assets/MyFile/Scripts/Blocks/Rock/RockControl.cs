///author:Yanru Zhang
///date:2014-2-23
///function: binded to rock,  if jump, then add score to MeDisplay

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;

public class RockControl : MonoBehaviour {

    /// <summary>
    /// 绑定在石头上的脚本，如果人碰到的话，保持静止
    /// </summary>
    /// <param name="other">有用的只有MeDisplay</param>
    public void OnTriggerEnter(Collider other)
    {
        //如果人碰到的话，keey_idle
        if("MeDisplay(Clone)" == other.gameObject.name)
        {
            if (true == State.ON_JUMP)
            {
                Debug.Log(Time.time + "该跳过石头了。。");
                ToolsFunction.AddScore(DataConst.SCORE_STONE); //加分
                other.transform.position += 2 * ToolsFunction.Toward(other.gameObject);
            }

            else
            {
                State.KEEP_IDLE = true;
                other.gameObject.GetComponent<Animation>().Play(State.ANIM_DEMAGE); //播放受打击的动作
                other.transform.position -= 0.1f * ToolsFunction.Toward(other.gameObject);
                ToolsFunction.AddScore(-DataConst.SCORE_STONE / 2);//减分
            }
        }
    }
}
