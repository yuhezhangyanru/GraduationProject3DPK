///author:Yanru Zhang
///date:2014-3-19 8:57:33
///function:binded to main block header

using UnityEngine;
using System.Collections;

public class MainHeaderControl : MonoBehaviour {
    private Animator blockHeaderAnimator;
    private UISprite headerSprite;

    private void Start()
    {
        blockHeaderAnimator = this.gameObject.GetComponent<Animator>();
        headerSprite = this.gameObject.GetComponent<UISprite>();
        blockHeaderAnimator.enabled = true;
    }

    private void HeaderTipEndCallback()
    {
        if (headerSprite.spriteName != "record_btn_icon")
            headerSprite.spriteName = "record_btn_icon";
    }
}
