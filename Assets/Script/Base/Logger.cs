using System.Collections;

public class Logger :Singleton<Logger> {
    private static bool m_isDebug=true;

    public static void Debug(string msg)
    {
        if (m_isDebug)
        {
            UnityEngine.Debug.Log(msg);
        }
    }

    public static void DebugFormat(string format, params object[] msg)
    {
        if (m_isDebug)
        {
            UnityEngine.Debug.LogFormat(format, msg);
        }
    }

    public static void DebugError(string errorMsg)
    {
        if(m_isDebug)
        {
            UnityEngine.Debug.LogError(errorMsg);
        }
    }

    public static void DebugErrorFormat(string format,params object[] msg)
    {
        if (m_isDebug)
        {
            UnityEngine.Debug.LogErrorFormat(format, msg);
        }
    }
}
