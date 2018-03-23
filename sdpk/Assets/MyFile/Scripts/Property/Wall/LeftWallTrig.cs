///author:Yanru Zhang
///date:2014-2-22
///function: binded to left wall, once collide MeDisplay, ontrigger work

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;

public class LeftWallTrig : MonoBehaviour {
    
    /// <summary>
    ///左边的墙撞到了人，人不能继续左移
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if("MeDisplay(Clone)" == other.gameObject.name)   //碰到了玩家
        {
            //Debug.Log(Time.time + "left 人和墙壁的夹角：" + ToolsFunction.IntersectionAngle(this.gameObject, other.gameObject));
            int angle = ToolsFunction.IntersectionAngle(this.gameObject, other.gameObject);
            if (ToolsFunction.BetweenTwo(angle, -5, 5) || ToolsFunction.BetweenTwo(angle, 175, 185))   //表示MeDisplay不是偏移过去的
            {
                State.OFFSET_CURRENT = State.OFFSET_LEFT;
            }
            else
            {
                State.OFFSET_IDLE = true;  //则最终为idle，旋转的时候解锁，但是left什么时候解锁？？
                other.transform.position -= 0.1f * ToolsFunction.Toward(other.gameObject);
            }
        }
    }

}
