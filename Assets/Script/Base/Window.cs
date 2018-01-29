using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Window
{


    public Window()
    {
        m_name = GetType().Name;
    }

    #region UI事件

    protected void RegistEvent(UIEventType UIEventType, EventManager.VoidHandle voidHandle)
    {
        EventManager.instance.RegistEvent(UIEventType, this, voidHandle);
    }

    protected void RemoveEvent(UIEventType UIEventType)
    {
        EventManager.instance.RemoveEvent(UIEventType);
    }

    #endregion

    #region 虚函数

    public virtual void OnShow()
    {
    }

    public virtual void OnDestory()
    {
    }

    public virtual void OnInit()
    {
        if (!m_isInit)
        {
            if (m_gameObject.GetComponent<UIPanel>() == null)
            {
                m_gameObject.AddComponent<UIPanel>();
            }
            if (m_mono == null)
            {
                m_mono = m_gameObject.AddComponent<emptyMono>();
            }
            m_panel = m_gameObject.GetComponent<UIPanel>();
            m_sortingOrder = m_gameObject.transform.GetSiblingIndex();
            m_panel.depth = m_sortingOrder;
            RegistEvents();
        }
    }

    public virtual void RegistEvents()
    {
        Logger.Debug("registEvents");
    }

    private void RemoveEvents()
    {
        m_eventLsit.Clear();
    }

    public virtual void OnUpdate()
    {

    }

    #endregion

    #region 成员

    public UIPanel m_panel;
    private bool m_isInit = false;
    public UnityEngine.GameObject m_gameObject;
    public List<UIEventType> m_eventLsit = new List<UIEventType>();
    private bool m_isShow = false;
    public emptyMono m_mono;
    public bool IsShow
    {
        get { return m_isShow; }
    }
    private bool m_isLoad = false;

    public bool IsLoad
    {
        get { return m_isLoad; }
    }

    public winType m_wndType;

    private int m_sortingOrder;

    public int SortingOrder
    {
        get { return m_sortingOrder; }
        set
        {
            m_sortingOrder = value;
        }
    }

    public virtual string resUrl
    {
        get { return string.Empty; }
    }

    public virtual string bundle
    {
        get { return string.Empty; }
    }

    public virtual ResourceType resType
    {
        get { return ResourceType.Resource; }
    }

    public string Name
    {
        get
        {
            return m_name;
        }
    }

    private string m_name;

    public bool m_done = false;

    public Dictionary<int, Dictionary<string, object>> m_cache_go = new Dictionary<int, Dictionary<string, object>>();


    #endregion
    public void OnLoad()
    {
        RemoveEvents();
        if (m_panel == null)
        {
            m_panel = m_gameObject.GetComponent<UIPanel>();
        }
        m_panel.depth = m_sortingOrder;
        m_gameObject.transform.parent = Global.instance.m_camera.transform;
        m_gameObject.transform.localScale = UnityEngine.Vector3.one;
        m_gameObject.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
        if (m_isInit == false)
        {
            OnInit();
            m_isInit = true;
        }
        m_isLoad = true;
    }




    public virtual void Close()
    {
        RemoveEvents();
        m_cache_go.Clear();
        m_done = false;
        m_isLoad = false;
        m_isShow = false;
        GameObject.Destroy(m_gameObject);
        WindowManager.instance.RemoveWindow(this);
    }


    public void Hide()
    {
        UIHelper.instance.SetActive(m_gameObject, false);
        m_isShow = false;
    }

    public void Show()
    {
        UIHelper.instance.SetActive(m_gameObject, true);
        m_isShow = true;
        OnShow();
    }

    public UIEventListener Register(GameObject go)
    {
        return UIEventListener.Get(go);
    }

    public T Find<T>(string goName) where T : class
    {
        if (string.IsNullOrEmpty(goName))
            return null;
        var tmpValue = m_cache_go.GetEnumerator();
        while (tmpValue.MoveNext())
        {
            Dictionary<string, object> tmpValue1 = tmpValue.Current.Value;
            if (tmpValue1.ContainsKey(goName))
                return (tmpValue1[goName] as GameObject).GetComponent<T>();
        }

        GameObject tmp = Find(goName);
        if (tmp != null)
            return tmp.GetComponent<T>();

        return null;
    }

    public GameObject Find(string goName)
    {
        if (string.IsNullOrEmpty(goName))
            return null;
        var tmpValue = m_cache_go.GetEnumerator();
        while (tmpValue.MoveNext())
        {
            Dictionary<string, object> tmpValue1 = tmpValue.Current.Value;
            if (tmpValue1.ContainsKey(goName))
                return (tmpValue1[goName] as GameObject);
        }

        GameObject go = UIHelper.instance.Find(m_gameObject, goName);
        if (go != null)
        {
            int parentID = go.transform.parent.gameObject.GetInstanceID();
            if (m_cache_go.ContainsKey(parentID) == false)
                m_cache_go.Add(parentID, new Dictionary<string, object>());
            if (m_cache_go[parentID].ContainsKey(goName) == false)
                m_cache_go[parentID].Add(goName, go);
        }
        return go;
    }

    public GameObject Find(GameObject go, string goName)
    {
		
        if (string.IsNullOrEmpty(goName))
            return null;
        int parentID = go.GetInstanceID();
        if (m_cache_go.ContainsKey(parentID))
        {
            if (m_cache_go[parentID].ContainsKey(goName))
                return m_cache_go[parentID][goName] as GameObject;
        }

        GameObject tmpObj = UIHelper.instance.Find(go, goName);
        if (tmpObj != null)
        {
            if (m_cache_go.ContainsKey(parentID) == false)
                m_cache_go.Add(parentID, new Dictionary<string, object>());
            m_cache_go[parentID].Add(goName, tmpObj);
        }
        return tmpObj;
    }

    public T Find<T>(GameObject go, string goName) where T : MonoBehaviour
    {
        GameObject tmpObj = Find(go, goName);
        if (tmpObj != null)
        {
            return tmpObj.GetComponent<T>();
        }
        return null;
    }

    public GameObject Find(string parentName, string goName)
    {
        if (string.IsNullOrEmpty(parentName) || string.IsNullOrEmpty(goName))
            return null;
        string key = parentName + goName;


        GameObject go = null;
        GameObject parent = null;
        parent = UIHelper.instance.Find(m_gameObject, parentName);
        if (parent != null)
        {
            go = Find(parent, goName);
            return go;
        }
        else
        {
            Logger.DebugErrorFormat("can't find parent:{0} ", parentName);
            return null;
        }
    }

    public T Find<T>(string parentName, string goName) where T : class
    {
        if (string.IsNullOrEmpty(parentName) || string.IsNullOrEmpty(goName))
            return null;
        GameObject go = Find(parentName, goName);
        T component = go.GetComponent<T>();
        if (component != null)
            return component;
        return null;
    }

    public void SetSprite(UISprite sprite, string spriteName, string altasName = "")
    {
        UIHelper.instance.SetSprite(sprite, spriteName, altasName);
    }

    public void SetTexture(UITexture texture, string textName)
    {
        UIHelper.instance.SetTexture(texture, textName);
    }

    public void SetActive(GameObject go, bool isActive)
    {
        UIHelper.instance.SetActive(go, isActive);
    }
}

public enum winType
{
    TypeNormal = 1,
    TypeCache = 2,
}
