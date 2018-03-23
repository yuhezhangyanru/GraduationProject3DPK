///author:Yanru Zhang
///date:2014-4-4 0:12:41
///function:give game tip

using UnityEngine;
using System.Collections;

namespace myfiles.scripts.ui.helpview
{
    public class HelpView : BaseView
    {
        private Button btnClose = null;         //关闭结束界面的按钮

        void Awake()
        {
            btnClose = FindInChild<Button>("btnClose");
            btnClose.onClick = OnbtnClose;
        }

        public override void HandleAfteerOpenView()
        {
            base.HandleAfteerOpenView();
        }

        //点击了关闭结束的按钮
        private void OnbtnClose(GameObject go)
        {
            CloseView();
            OpenView("StartView");
        }

    }
}