///author:Yanru Zhang
///date:2014-4-8 11:57:05
///function:binded to purple effect, monster die play.

using UnityEngine;
using System.Collections;

public class EMonsterDie : MonoBehaviour {
    private static Animator monsterDieAnimator = null;
    private static GameObject monsterDieEffect = null;

    // Use this for initialization
	void Awake () {
        monsterDieAnimator = this.gameObject.GetComponent<Animator>();
        monsterDieEffect = this.transform.FindChild("Fluffy Smoke").gameObject;
        monsterDieEffect.SetActive(false);
	}


    /// <summary>
    /// 设置并播放动画
    /// </summary>
    /// <param name="pos">指定特效的位置</param>
    public static void StartPlay(Vector3 pos)
    {
        DataConst.MonsterDiePre.transform.localPosition = pos;
        monsterDieAnimator.SetBool("delay", true);  //开始播放
        monsterDieEffect.SetActive(true);
    }


    //**********************这里就一直没有被执行过！！！！！***********************
    private void AnimationCallback()
    {
        if (DataConst.MonsterDiePre)
        {
            Debug.Log(Time.time + " 死亡动画的回调函数执行！");
            monsterDieEffect.SetActive(false);
        }
    }
}
