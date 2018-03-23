///author: Yanru Zhang
///date: 2014-2-21
///function: binded to area, to ensure here in jump detection state

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;

public class areaScript : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        if ("MeDisplay(Clone)" == other.collider.gameObject.name)
        {
            State.KEEP_IDLE = true;
            if (true == State.ON_JUMP)  //在跳的过程中
            {
                Vector3 v = (ToolsFunction.EqualFloat(other.transform.eulerAngles.y, 90) == true ? new Vector3(73, 2.3f, 57) : new Vector3(25, 2.3f, 11));
                MeController.MoveTo(other.gameObject, v, 2f);
                other.gameObject.GetComponent<Animation>().Play(State.ANIM_HANDS_UP);
                ToolsFunction.AddScore(DataConst.SCORE_LINE);//加分
            }
            else
            {
                other.transform.position -= 0.1f * ToolsFunction.Toward(other.gameObject);  //现在人就是往后退了，然后应该会重新检测吧！！！！
                ToolsFunction.AddScore(-DataConst.SCORE_LINE / 2);//减分
            }
        }
    }
}
