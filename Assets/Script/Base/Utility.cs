using UnityEngine;
using System.Collections;
using System.Reflection;

public class Utility : Singleton<Utility>
{

    public GameObject Find(GameObject go, string childName)
    {
        foreach (Transform t in go.transform)
        {
            if (go.name.CompareTo(childName) == 0)
            {
                return t.gameObject;
            }
        }
        Debug.LogErrorFormat("{0} 节点下找不到GameObject {1}", go.name, childName);
        return null;
    }

    public GameObject Find(string parentName, string childName)
    {
        GameObject go = GameObject.Find(parentName);
        if (go != null)
        {
            return Find(go, childName);
        }
        else
        {
            Debug.LogErrorFormat("找不到父节点：{0}", parentName);
            return null;
        }
    }

    public GameObject AddChild(string parentName, string childName, string AttachParentName)
    {
        GameObject go = Find(parentName, childName);
        if (go != null)
        {
            GameObject newParent = GameObject.Find(AttachParentName);
            if (newParent != null)
            {
                GameObject child = Instantiate(go) as GameObject;
                child.transform.parent = newParent.transform;
                return child;

            }
            else
            {
                Debug.LogErrorFormat("the attachName：{0} is not found" + AttachParentName);
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    public void SaveDataToLocal(string key, object data)
    {
        if (string.IsNullOrEmpty(key))
        {
            Logger.DebugError("key can't be empty");
            return;
        }

        if (data is string)
            PlayerPrefs.SetString(key, (string)data);
        else if (data is int)
            PlayerPrefs.SetInt(key, (int)data);
        else if (data is float)
            PlayerPrefs.SetFloat(key, (float)data);

        else
            Logger.DebugErrorFormat("not support type:{0}", data);
    }

    public object LoadDataFromLoad(string key, LocalDataType type)
    {
        if (string.IsNullOrEmpty(key))
        {
            Logger.DebugError("key can't be empty");
            return null;
        }
        if (type == LocalDataType.Type_STRING)
            return PlayerPrefs.GetString(key);
        else if (type == LocalDataType.Type_INT)
            return PlayerPrefs.GetInt(key);
        else if (type == LocalDataType.Type_FLOAT)
            return PlayerPrefs.GetFloat(key);
        else return null;
    }

    public string GetColorString(string color, string content)
    {
        return string.Format("[{0}]{1}[-]", color, content);
    }

    public GameObject Instantiate(GameObject src, string name = "")
    {
        if (null == src)
            return null;
        GameObject clone = GameObject.Instantiate(src);
        if (!string.IsNullOrEmpty(name))
            clone.name = name;
        return clone;
    }

    public static bool checkInScreen(GameObject go)
    {
        if (go == null)
            return false;

        Vector2 m_screenPoint = Global.instance.m_camera.WorldToScreenPoint(go.transform.position);
        if (m_screenPoint.x <= 0 || m_screenPoint.y <= 0 || m_screenPoint.x >= Screen.width || m_screenPoint.y >= Screen.height)
            return true;
        return false;
    }

    public void ShowConfirmMessage(MessageWndType type, string content, string title = "", OKHandler okHandle = null, CancleHandler cancleHandle = null)
    {
        WindowFactory.instance.CreateWindow(WindowType.MessageWindow, delegate(object[] param)
        {
            EventManager.instance.NotifyUIEvent(UIEventType.TypeOpenMessageWin, type, title, content, okHandle, cancleHandle);
        });
    }

    public void PlayTweenPosition(GameObject go, Vector3 from, Vector3 to, float time, EventDelegate.Callback callBack = null)
    {
        TweenPosition tp = go.GetComponent<TweenPosition>();
        tp.from = from;
        tp.to = to;
        tp.duration = 1f;
        tp.ResetToBeginning();
        if (callBack != null)
            tp.AddOnFinished(callBack);
        tp.PlayForward();
    }

    public void PlayTweenAlpha(GameObject go, float from, float to, float time, EventDelegate.Callback callBack = null)
    {
        TweenAlpha ta = go.GetComponent<TweenAlpha>();
        ta.from = from;
        ta.to = to;
        ta.duration = time;
        ta.ResetToBeginning();
        if (callBack != null)
            ta.AddOnFinished(callBack);
        ta.PlayForward();
    }

    public void PlayTweenScale(GameObject go, Vector3 from, Vector3 to, float time, EventDelegate.Callback callBack = null)
    {
        TweenScale ts = go.GetComponent<TweenScale>();
        ts.from = from;
        ts.to = to;
        ts.duration = 1f;
        ts.ResetToBeginning();
        if (callBack != null)
            ts.AddOnFinished(callBack);
        ts.PlayForward();
    }

}

public enum LocalDataType
{
    Type_INT = 1,
    Type_FLOAT = 2,
    Type_STRING = 3,
}

public delegate void OKHandler();
public delegate void CancleHandler();
