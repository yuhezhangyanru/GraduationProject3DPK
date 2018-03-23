///author:Yanru Zhang
///date：2014-3-5
///function: binded to trap trig, to test if need idle on trap

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;

public class TrapControl : MonoBehaviour {


    /// <summary>
    /// 绑定在陷阱中的触发器，如果执行跳就
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if ("MeDisplay(Clone)" == other.gameObject.name)
        {
            Debug.Log(Time.time+"碰到了人啊");
            if (true == State.ON_JUMP)
            {
                Debug.Log(Time.time + "该起跳了");
                //向前移动并且禁用本触发器
                other.transform.position += 2 * ToolsFunction.Toward(other.gameObject);
                ToolsFunction.AddScore(DataConst.SCORE_TRAP);//加分
            }

            else
            {
                State.KEEP_IDLE = true;
                other.gameObject.GetComponent<Animation>().Play(State.ANIM_DEMAGE); //播放受打击的动作
                other.transform.position -= 0.1f * ToolsFunction.Toward(other.gameObject);
                ToolsFunction.AddScore(-DataConst.SCORE_TRAP / 2);//减分
            }
        }
    }
}
