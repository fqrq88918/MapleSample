using UnityEngine;
using System.Collections;

public class MainWin : SingletonWindow<MainWin>
{
    public override string resUrl
    {
        get
        {
            return Setup.UIResPath+"UI_main";
        }
    }

    public override void RegistEvents()
    {
        base.RegistEvents();
        Logger.Debug("mianview event regist");
        RegistEvent(UIEventType.TypeOpenMainView, delegate(object[] param) 
        {
            Logger.Debug((string)param[0]);
        });
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Q))
            Utility.instance.ShowConfirmMessage(MessageWndType.TypeOKAndCancle, "你该吃药啦", "提示", delegate() { Logger.Debug("点了确定"); }, delegate() { Logger.Debug("点了取消"); });
        if (Input.GetKeyDown(KeyCode.W))
            Utility.instance.ShowConfirmMessage(MessageWndType.TypeConfirm,  "showLog","提示");
    }

    public override void OnInit()
    {
        base.OnInit();
        Register(GameObject.Find("Button")).onClick = delegate (GameObject go) {
            count++;
            Logger.Debug("count1:"+count);
        };
        Register(GameObject.Find("Button")).onClick = delegate (GameObject go) {
            count++;
            Logger.Debug("count2:" + count);           
        };
    }
    private uint count = 0;
    public override void OnShow()
    {
        base.OnShow();
        Logger.Debug("=====show=====");
        UISprite sprite=Find<UISprite>("sprite");
        Logger.Debug(sprite.gameObject.name);
        SetSprite(sprite, "Flag-US", "Wooden Atlas");
        SetTexture(Find<UITexture>("texture"), "Backdrop");
    }
}
