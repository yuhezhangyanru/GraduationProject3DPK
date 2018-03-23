///author:Yanru Zhang
///date:2014-5-6 15:16:33
///function:stored the rectange area, test if need turn left/right, rectanges are in a list

using UnityEngine;
using System.Collections;

public class Rectangle : MonoBehaviour {
    
    public float X_Min = 0f;
    public float X_Max = 0f;
    public float Z_Min = 0f;
    public float Z_Max = 0f;


    /// <summary>
    /// 四个参的构造函数，用于结构化数据
    /// </summary>
    /// <param name="x_min"></param>
    /// <param name="x_max"></param>
    /// <param name="z_min"></param>
    /// <param name="z_max"></param>
    public Rectangle(float x_min, float x_max, float z_min, float z_max)  //构造函数，用于定义矩形区域的结构
    {
        X_Min = x_min;
        X_Max = x_max;
        Z_Min = z_min;
        Z_Max = z_max;
    }

	void Start () {
	
	}
	
	void Update () {
	
	}
}
