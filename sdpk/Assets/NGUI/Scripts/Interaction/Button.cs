///date:2014-3-12 10:01:55
///function: Button script replace UIButton

using UnityEngine;
using System.Collections;

public class Button : UIWidgetContainer {
    
    public GameObject hightLight;
    public UILabel label;
    public UISprite background;
    private Color lastColor;
    private Color mask = new Color(73f / 255f, 73f / 255f, 73f / 255f);

    void Awake()
    {
        Transform tr = transform.FindChild("highlight");
        if(tr)
        {
            hightLight = tr.gameObject;
            hightLight.SetActive(false); //默认不显示高亮
        }
        tr = transform.FindChild("label");
        if (tr)
            label = tr.GetComponent<UILabel>();
        this.onPress += Press;
        if (background == null)
            background = NGUITools.FindInChild<UISprite>(gameObject, "background");
        if (background == null)
            background = GetComponent<UISprite>();
     
    }

    public bool isEnabled
    {
        get
        {
            Collider col = collider;
            return col && col.enabled;
        }
        set 
        {
            Collider col = collider;
            if (!col) return;

            if(col.enabled != value)
            {
                col.enabled = value;
            }
        }
    }

    void Press(GameObject go, bool isPress)
    {
        Transform tr = go.transform.FindChild("highlight");
        if (tr)
        {
            if (isPress)
            {

                tr.gameObject.SetActive(true);
            }
            else
            {
                tr.gameObject.SetActive(false);
            }
        }
        else if(background)
        {
            if(isPress && background.color != mask)
            {
                lastColor = background.color;
                background.color = mask;
            }
            else if(background.color == mask)
            {
                Invoke("normal", responseTime);
            }
        }
    }

    void normal()
    {
        background.color = lastColor;
    }
}
