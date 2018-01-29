using UnityEngine;
using System.Collections;

public class Window 
{
   

    #region UI事件
    protected void RegistEvent(UIEventType eventType, EventManager.VoidHandle voidHandle)
    {
        EventManager.instance.RegistEvent(eventType, this.m_windType, voidHandle);
    }

    protected void RemoveEvent(UIEventType eventType)
    {
        EventManager.instance.RemoveEvent(eventType);
    }
    #endregion

    #region 虚函数
    /// <summary>
    /// 每次打开界面的时候调用
    /// </summary>
    public virtual void OnShow() { }

    /// <summary>
    /// 每次关闭界面的时候调用
    /// </summary>
    public virtual void OnHide() { }

    public virtual void OnDestory() { }

    /// <summary>
    /// 初始化，只执行一次
    /// </summary>
    public virtual void OnInit()
    {
        if (m_gameObject.GetComponent<UIPanel>() == null)
        {
            m_gameObject.AddComponent<UIPanel>();

        }
        m_panel.GetComponent<UIPanel>();
        m_sortingOrder = m_gameObject.transform.GetSiblingIndex();
        m_panel.depth = m_sortingOrder;
    }

    public virtual void RegistEvents()
    { }

    public virtual void RemoveEvents()
    { }
    #endregion

    #region 成员
    /// <summary>
    /// 窗口panel
    /// </summary>
    public UIPanel m_panel;
    /// <summary>
    /// 窗口成员
    /// </summary>
    public GameObject m_gameObject;
    /// <summary>
    /// 窗口类型
    /// </summary>
    private WindowType m_windType;
    private bool m_isInit = false;
    public WindowType WindType
    {
        get { return m_windType; }
        set { m_windType = value; }
    }

    private int m_sortingOrder;
    
    public int SortingOrder
    {
        get { return m_sortingOrder; }
        set
        {
            m_sortingOrder = value;
            if (m_panel != null)
            {
                m_panel.depth = m_sortingOrder;
            }
        }
    }
    public virtual string m_resUrl
    {
        get { return string.Empty; }
    }


    public virtual string m_bundle
    {
        get { return string.Empty; }
    }
    

    #endregion

    protected void ShowWindow()
    {
        if (m_isInit == false)
        {
            RegistEvents();
            OnInit();
            m_isInit = true;
        }
        m_gameObject.transform.localPosition = new Vector3(0, 0, 0);
        OnShow();
        m_gameObject.SetActive(true);
        
    }

    protected void CloseWindow()
    {
        m_gameObject.SetActive(false);
    }


    protected void HideWindowAndRunInBackground()
    {
        m_gameObject.transform.localPosition = new Vector3(10000, 0, 0);
    }

    

}
