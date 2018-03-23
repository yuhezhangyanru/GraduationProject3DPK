using UnityEngine;
using System.Collections;
using System;

public class MyEventArgs : EventArgs {

	public int temperature;

    //构造函数
    public MyEventArgs(int temperature)
    {
        this.temperature = temperature;
    }
}
