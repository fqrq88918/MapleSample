using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager:Singleton<EventManager>
{
    public delegate void VoidHandle(params object[] param);
    private Dictionary<UIEventType, Dictionary<WindowType, VoidHandle>> m_eventList = new Dictionary<UIEventType, Dictionary<WindowType, VoidHandle>>();

    /// <summary>
    /// 注册事件监听
    /// </summary>
    /// <param name="m_event"></param>
    /// <param name="m_win"></param>
    /// <param name="handle"></param>
    public void RegistEvent(UIEventType m_event,WindowType m_win,VoidHandle handle)
    {
        if (m_eventList.ContainsKey(m_event) == false)
        {
            m_eventList.Add(m_event, new Dictionary<WindowType, VoidHandle>());
        }
        if(m_eventList[m_event].ContainsKey(m_win)==false)
        {
            m_eventList[m_event].Add(m_win, handle);
        }
    }

    /// <summary>
    /// 移除时间监听
    /// </summary>
    /// <param name="m_event"></param>
    public void RemoveEvent(UIEventType m_event)
    {
        if (m_eventList.ContainsKey(m_event))
        {
            m_eventList.Remove(m_event);
        }
    }

    /// <summary>
    /// 分发消息事件
    /// </summary>
    /// <param name="m_event"></param>
    /// <param name="param"></param>
    public void NotifyUIEvent(UIEventType m_event,params object[] param)
    {
        if (m_eventList.ContainsKey(m_event))
        {
            foreach (var tmp in m_eventList[m_event])
            {
                tmp.Value(param);
            }
        }
        else
        {
            Debug.LogErrorFormat("事件{0}未注册",m_event.ToString());
        }
    }
	
}
