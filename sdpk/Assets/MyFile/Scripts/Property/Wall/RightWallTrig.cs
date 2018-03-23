///author:Yanru Zhang
///date:2014-2-22
///function:binded to right wall, once collide MeDisplay, offset_current = offset_right

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;

public class RightWallTrig : MonoBehaviour {
    

    /// <summary>
    /// 右边的墙壁碰到了人，不能继续右移
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if ("MeDisplay(Clone)" == other.gameObject.name)   //碰到了玩家
        {
            //要判断当前两个对象的夹角
            Debug.Log(Time.time + "right 人和墙壁的夹角：" + ToolsFunction.IntersectionAngle(this.gameObject, other.gameObject));
            int angle = ToolsFunction.IntersectionAngle(this.gameObject, other.gameObject);
            if (ToolsFunction.BetweenTwo(angle, -5, 5) || ToolsFunction.BetweenTwo(angle, 175, 185))   //表示MeDisplay不是偏移过去的
            {
                State.OFFSET_CURRENT = State.OFFSET_RIGHT;
            }
            else   //表示跑步撞到墙上的，
            {
                //向后偏移
                State.OFFSET_IDLE = true;
                other.transform.position -= 0.1f * ToolsFunction.Toward(other.gameObject);
            }
        }
    }
}
