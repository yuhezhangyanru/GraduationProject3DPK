//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Widget container is a generic type class that acts like a non-resizeable widget when selecting things in the scene view.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Widget Container")]
public class UIWidgetContainer : MonoBehaviour
{
    public delegate void ClickDelegate();   //鼠标点击，方便调用
    public delegate void HoverDelegate();  //鼠标划过的委托，方便调用
    public delegate void VoidDelegate(GameObject go);
    public delegate void BoolDelegate(GameObject go, bool state);
    public delegate void FloatDelegate(GameObject go, float delta);
    public delegate void VectorDelegate(GameObject go, Vector2 delta);
    public delegate void StringDelegate(GameObject go, string text);
    public delegate void ObjectDelegate(GameObject go, GameObject draggedObject);
    public delegate void KeyCodeDelegate(GameObject go, KeyCode key);

    public static UIWidgetContainer currentWidget;
    public object parameter;
    private ClickDelegate clickDelegate = null;
    private HoverDelegate hoverDelegate = null;

    public VoidDelegate onSubmit;
    public VoidDelegate onClick;
    public VoidDelegate onDoubleClick;
    public VoidDelegate onHover;
    public BoolDelegate onPress;
    public BoolDelegate onSelect;
    public FloatDelegate onScroll;
    public VectorDelegate onDrag;
    public ObjectDelegate onDrop;
    public StringDelegate onInput;
    public KeyCodeDelegate onKey;

    private float time = 0f;
    /// <summary>
    /// OnClick函数相应间隔
    /// </summary>
    public float responseTime = 0.0f;

    void OnSubmit() { if (onSubmit != null) onSubmit(gameObject); }

    //鼠标点击
    void OnClick() {
        float now = RealTime.time;
        if (onClick != null && (responseTime == 0f || now - time - responseTime > 0.0f)) 
        {
            currentWidget = this;
            onClick(gameObject);
            time = now;
        }
        currentWidget = this;
        if (clickDelegate != null)
            clickDelegate();
        currentWidget = null;
    }

    //鼠标划过事件，与点击事件类似
    void OnHover()
    {
        float now = RealTime.time;
        if (onHover != null && (responseTime == 0f || now - time - responseTime > 0.0f)) 
        {
            currentWidget = this;
            onHover(gameObject);
            time = now;
        }
        currentWidget = this;
        if (hoverDelegate != null)
            hoverDelegate();
        currentWidget = null;
    }


    void OnDoubleClick()
    {
        if (onDoubleClick != null)
        {
            onDoubleClick(gameObject);
            currentWidget = this;
        }
    }

    //void OnHover(bool isOver) { if (onHover != null) onHover(gameObject, isOver); }
    void OnPress(bool isPressed) 
    {
        if (onPress != null)
        {
            if(isPressed)
            {
                if (currentWidget != null)
                    currentWidget.SetActive("gaoguang", false);
            }

            currentWidget = this;
            onPress(gameObject, isPressed);
            if (isPressed)
                currentWidget.SetActive("gaoguang", true);
        }
    }

    void OnSelect(bool selected) { if (onSelect != null) onSelect(gameObject, selected); }
    void OnScroll(float delta)
    {
        if (onScroll != null)
        {
            currentWidget = this;
            onScroll(gameObject, delta);
        }
    }
    
    void OnDrag(Vector2 delta) 
    {
        if (onDrag != null)
        {
            currentWidget = this;
            onDrag(gameObject, delta);
        }
    }
    
    void OnDrop(GameObject go) 
    {
        if (onDrop != null)
        {
            currentWidget = this;
            onDrop(gameObject, go);
        }
    }
    void OnInput(string text) 
    {
        if (onInput != null)
        {
            currentWidget = this;
            onInput(gameObject, text);
        }
    }
    void OnKey(KeyCode key) 
    {
        if (onKey != null)
        {
            currentWidget = this;
            onKey(gameObject, key);
        }
    }

    public void SetActive(string path, bool active)
    {
        gameObject.SetChildActive(path, active);
    }

    public GameObject FindChild(string name)
    {
        return NGUITools.FindChild(gameObject, name);
    }

    public T FindInChild<T>(string path) where T : Component
    {
        return NGUITools.FindInChild<T>(gameObject, path);
    }
}