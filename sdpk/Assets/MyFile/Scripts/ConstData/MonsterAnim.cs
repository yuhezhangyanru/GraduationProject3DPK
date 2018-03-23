using UnityEngine;
using System.Collections;

public class MonsterAnim : MonoBehaviour {

    public static string[] AnimNormal = new string[] {"walk", "walk", "hit1", "hit2" };  //在撞到人之前就会播放这种动画
    public static string[] AnimAttack = new string[] { "attack1", "taunt", "attack2"};  //人在碰到蜘蛛之后播放的动画
    public static string AnimFight = "hit2";
    public static string AnimDeath = "death1";  //死亡动画
}
