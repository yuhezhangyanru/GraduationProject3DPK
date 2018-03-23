///atuhor:Yanru Zhang
///date:2014-3-11 11:14:01
///function: create a singleton

using UnityEngine;
using System.Collections;
using System;

public class Singleton<T>where T:new ()
{
    private static T instance = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
    public static T Instance
    {
        get 
        {
            return Singleton<T>.instance;
        }
    }
    protected Singleton()
    {
    }
}
