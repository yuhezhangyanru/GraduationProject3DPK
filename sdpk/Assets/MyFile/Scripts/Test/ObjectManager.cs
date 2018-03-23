using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour {

    //public static List<GameObject> ObjList = new List<GameObject>();  //对象池列表
    //新建一个对象字典，
    public static Dictionary<string, GameObject> ObjList = new Dictionary<string, GameObject>(); 
    public string testStr = null;

	// Use this for initialization
	void Start () {
    }
	
	void Update () {
        if(Input.GetKeyDown("1"))  //显示对象列表
        {
            Debug.Log(Time.time+"obj 1："+ ObjList["Cube"].name);
            Debug.Log(Time.time + "obj 2：" + ObjList["Sphare"].name);
            Debug.Log(Time.time + "obj 3：" + ObjList["Capsule"].name);
        }
        if(Input.GetKeyDown("2"))  //创建对象
        {
            Debug.Log(Time.time + " test str is: " + testStr);
            ObjInList(testStr);
        }
        if (Input.GetKeyDown("3"))
        {
            HindObj(testStr);
        }

	}



    /// <summary>
    /// 返回想要创建的对象，列表中有就直接返回，没有则新创建一个加入队列
    /// </summary>
    /// <param name="objName"></param>
    /// <returns></returns>
   public static GameObject ObjInList(string objName)  //根据预设的名字来创建一个对象
    {
        if (ObjList.ContainsKey(objName))
        {
            ObjList[objName].gameObject.SetActive(true);
            if (ObjList[objName].gameObject.GetComponent<BoxCollider>())
            {
                Debug.Log(Time.time + " boxcollider is exist!");
                ObjList[objName].gameObject.GetComponent<BoxCollider>().enabled = true;
            }
            return ObjList[objName].gameObject;
        }
        else
        {
            GameObject go = Instantiate(Resources.Load("OneObj/"+objName), Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
            ObjList.Add(objName, go);
            return go;
        }
    }

    /// <summary>
    /// 隐藏想要销毁的对象，再次使用还能够拿出来
    /// </summary>
    /// <param name="objName"></param>
    public static void HindObj(string objName)
   {
       if (ObjList.ContainsKey(objName))
       {
           ObjList[objName].gameObject.SetActive(false);
       }
   }
}
