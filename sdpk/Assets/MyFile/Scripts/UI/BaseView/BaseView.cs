///author:Yanru Zhang
///date:2014-3-11 9:57:39
///fucntion:all view classes inherit this class

using UnityEngine;
using System.Collections;
using myfiles.scripts.tools;
using System;
using System.Collections.Generic;
using myfiles.scripts.ui;

namespace myfiles.scripts.ui
{

    public enum ViewLayer
    {
        LowLayer = 0, //战斗的UI，暂时没有用到
        MiddleLayer = 1,  //UI层
        HighLayer  = 2     //消息框层
    }

    public class BaseView:MonoBehaviour//<T> : Singleton<T>, IView where T : new() //暂时取消单例！<T>: Singleton<T> where T:new()
    {
        public GameObject UIRoot = null;   //UI根节点，用于管理界面
        public GameObject SubViewNow = null;   //当前界面的临时对象
        public static List<GameObject> SubView = new List<GameObject>();
        //所有的Ui名字都要存到这里
        public static string[] ViewName = { "MainView", "SettingView", "RecordView", "StartView", "HelpView", "AwardView"};

        public virtual ViewLayer layerType{get{return ViewLayer.MiddleLayer; }}
        public virtual bool IsFullUI{get{return false;}}

        //初始化控件的时候使用，子类重写，只在创建对象的时候执行一次
  
        //唤醒函数
        void Awake()
        {
            UIRoot = GameObject.Find("UI(Clone)");  //UI的根节点
            //将子界面全部加入到界面队列
            GameObject go = new GameObject();
            for (int i = 0; i < ViewName.Length; i ++ )
            {
                go = ToolsFunction.FindChild(UIRoot, ViewName[i]);
                SubView.Add(go);
            }
        }


        //出要处理的 是界面的初始化工作
        public virtual void Init()
        {
        }

        //刷新函数update
        public virtual void Update()
        {
        }


        /// <summary>
        ///关闭当前界面的函数，暂时未测
        /// </summary>
        public void CloseView()
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }


        /// <summary>
        /// 打开指定界面，界面名字调用即可
        /// </summary>
        /// <param name="viewName">界面表示的对象名字</param>
        public virtual GameObject OpenView(string viewName)
        {
            int index = Array.IndexOf(ViewName, viewName);   //返回界面对应的下表
            SubView[index].SetActive(true);
            return SubView[index];   //返回即将打开的界面对象
        }


        /// <summary>
        /// 打开界面之后的处理函数，
        /// </summary>
        public virtual void HandleAfteerOpenView()
        {
            Init();
        }

        /// <summary>
        ///关闭指定的view
        /// </summary>
        /// <param name="viewName"></param>
        public void CloseView(string viewName)
        {
            GameObject child = ToolsFunction.FindChild(UIRoot, viewName);
            child.SetActive(false);
        }

        /// <summary>
        /// 查找子对象的工具函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T FindInChild<T>(string path = null) where T : Component
        {
            if (string.IsNullOrEmpty(path))
            {
                return gameObject.GetComponent<T>();
            }
            else
            {
                return NGUITools.FindInChild<T>(gameObject, path);
            }
        }



        /// <summary>
        /// 将标签设置为指定的值，
        /// </summary>
        /// <param name="label"></param>
        /// <param name="dstNum">目标参数</param>
        public void SetLabelValue(UILabel label, int dstNum)
        {
            if(this.gameObject.active == true)
                StartCoroutine(ChangeLabel(label, dstNum));
        }


        /// <summary>
        /// 改变分数显示的协程函数，在SetLabelValue中调用
        /// </summary>
        /// <param name="label"></param>
        /// <param name="dstNum"></param>
        /// <returns></returns>
        IEnumerator ChangeLabel(UILabel label, int dstNum)
        {
            int present = int.Parse(label.text);
            bool increace = (present >= dstNum) ? false : true;
            while (present != dstNum && present >= 0)  //值没有变化完, 并且值是可以减的
            {
                label.text = present.ToString();
                if (increace)
                    present += 20;
                else
                    present -= 20;
                yield return new WaitForSeconds(0.000001f);
            }
        }
    }


    //BaseView用到的借口
    public interface IView
    {
        GameObject gameObject { get; set; }
        Transform transform { get; set; }
        ViewLayer layerType { get; }

        bool IsFullUI { get; }
        void CloseView();
        void OpenView();
        void Update();
    }
    public static class ViewManager
    {
        public static List<IView> openViewList = new List<IView>();
        public static void Register(IView obj)
        {
            lock (openViewList)
            {
                if (obj.IsFullUI)
                {
                    HidePrevFullUI(obj);
                }

            }
        }

        private static void HidePrevFullUI(IView topObj)
        {
            for (int i = openViewList.Count - 1; i >= 0; i--)
            {
                IView view = openViewList[i];
                if (view != topObj && view.IsFullUI)
                {
                    view.gameObject.SetActive(false);
                    return;
                }
            }
        }

        private static void ShowPrevFullUI()
        {
            for (int i = openViewList.Count - 1; i >= 0; i--)
            {
                IView view = openViewList[i];
                if (view.IsFullUI)
                {
                    view.gameObject.SetActive(true);
                    return;
                }
            }
        }

        public static void UnRegister(IView obj)
        {
            lock (openViewList)
            {
                openViewList.Remove(obj);
                if (openViewList.Contains(obj))
                {

                }
                if (obj.IsFullUI)
                {
                    ShowPrevFullUI();
                }
            }
        }

        public static void CloseAll()
        {
            lock (openViewList)
            {
                foreach (IView temp in openViewList)
                {
                    temp.CloseView();
                }
                openViewList.Clear();
            }
        }

        public static void Update()
        {
            lock (openViewList)
            {
                int length = openViewList.Count;
                for (int i = 0; i < length; i++)
                {
                    int curLength = openViewList.Count;
                    openViewList[i].Update();

                    length = openViewList.Count;
                    if (curLength - length > 0)
                    {
                        i -= curLength - length;
                        if (i < 0)
                        {
                            i = 0;
                        }
                    }
                }
            }
        }

        private static void AddView(IView view)
        {
            //if(view.layerType != ViewLayer.NoneLayer)
            //{
            //    int depth = GetMaxDepth(view.layerType);
            //    depth++;
            //    UIPanel[] pannels = view.gameObject.GetComponentsInChildren<UIPanel>(true);
            //    Array.Sort(pannels,DepthCompareFunc);
            //    int lastDepth = -9999;
            //    foreach(UIPanel pannel in pannels)
            //    {
            //        if(pannel.depth == lastDepth)
            //        {
            //            pannel.depth = depth-3;
            //        }
            //        else
            //        {
            //            lastDepth = pannel.depth;
            //            pannel.depth = depth;
            //            depth+=3;
            //        }
            //    }
            //}
            if (!openViewList.Contains(view))
            {
                openViewList.Add(view);
            }
        }
    }
}