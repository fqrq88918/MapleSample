using UnityEngine;
using System.Collections;

public class MessageWindow : SingletonWindow<MessageWindow>
{
    public GameObject confirmWnd;
    public GameObject okAndCancleWnd;
    public UILabel m_title;
    public UILabel m_context;
    public MessageWndType type;
    private OKHandler okHandle;
    private CancleHandler cancleHandle;

    public override string resUrl
    {
        get
        {
            return Setup.UIResPath + "UI_ConfirmMsg";
        }
    }

    public MessageWindow()
    {
        SortingOrder = 80;
        m_wndType = winType.TypeCache;
    }

    public override void OnInit()
    {
        base.OnInit();
        confirmWnd = Find("ConfirmWin");
        okAndCancleWnd = Find("OkCancleWin");
        m_title = Find<UILabel>("title");
        m_context = Find<UILabel>("content");
        Register(Find(okAndCancleWnd, "OKButton")).onClick = OnClickOKBtn;
        Register(Find(okAndCancleWnd, "CancleButton")).onClick = OnClickCancleBtn;
        Register(Find(confirmWnd, "OKButton")).onClick = OnClickConfirmBtn;

    }

    public override void OnShow()
    {
        base.OnShow();
        Logger.DebugFormat("type:{0}",type);
        switch (type)
        {
            case MessageWndType.TypeConfirm:
                SetActive(confirmWnd, true);
                SetActive(okAndCancleWnd, false);
                break;
            case MessageWndType.TypeOKAndCancle:
                SetActive(confirmWnd, false);
                SetActive(okAndCancleWnd, true);
                break;
        }
    }

    public override void RegistEvents()
    {
        base.RegistEvents();
        RegistEvent(UIEventType.TypeOpenMessageWin, delegate(object[] param)
        {
            type = (MessageWndType)param[0];
            Logger.DebugFormat("type:{0}", type);
            m_title.text = (string)param[1];
            m_context.text = (string)param[2];
            okHandle = (OKHandler)param[3];
            cancleHandle = (CancleHandler)param[4];
        });
    }

    void OnClickOKBtn(GameObject go)
    {
        if (okHandle != null)
            okHandle();
        Hide();

    }

    void OnClickCancleBtn(GameObject go)
    {
        if (cancleHandle != null)
            cancleHandle();
        Hide();
    }

    void OnClickConfirmBtn(GameObject go)
    {
        Logger.Debug("关闭窗口");
        Hide();
    }
}
