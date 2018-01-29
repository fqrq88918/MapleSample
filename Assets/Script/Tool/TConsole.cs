using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TConsole : MonoBehaviour
{
    public readonly string message;
    public readonly string stackTrace;

    public static bool isShowLog = false;
    private bool isTouching = false;
    private GUIContent clearLabel;
    private GUIContent collapseLabel;
    private GUIContent scrollToBottomLabel;
    private GUIContent normalLabel;
    private GUIContent errorLabel;
    private Rect windowRect;
    const int margin = 20;
    private bool isCollapse = true;
    private bool ScrollToBottom = true;
    private bool isNormal = true;
    private bool isError = true;
    public List<ConsoleMessage> msgList = new List<ConsoleMessage>();
    Vector2 scrollPosition = new Vector2(0, 0);
    float value = 0;
    public struct ConsoleMessage
    {
        public readonly string message;
        public readonly string stackTrace;
        public readonly LogType type;
        public ConsoleMessage(string message, string stackTrace, LogType type)
        {
            this.message = message;
            this.stackTrace = stackTrace;
            this.type = type;
        }
    }

    // Use this for initialization
    void Start()
    {
        clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
        collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");
        normalLabel = new GUIContent("Normal", "Show the normal log");
        errorLabel = new GUIContent("Error", "Show the error log");
        scrollToBottomLabel = new GUIContent("ScrollToBottom", "Scroll bar always at bottom");
        windowRect = new Rect(margin, margin, Screen.width * 0.5f - (2 * margin), Screen.height - (2 * margin));
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                isShowLog = !isShowLog;
            }
        }
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount >= 4 && !isTouching)
            {
                isShowLog = !isShowLog;
                isTouching = true;
            }
            else
            {
                if (Input.touchCount == 0)
                {
                    isTouching = false;
                }
            }
        }
    }

    void OnGUI()
    {
        if (!isShowLog)
        {
            return;
        }
        GUILayout.BeginHorizontal();
        isCollapse = GUILayout.Toggle(isCollapse, collapseLabel, GUILayout.ExpandWidth(true));
        ScrollToBottom = GUILayout.Toggle(ScrollToBottom, scrollToBottomLabel, GUILayout.ExpandWidth(true));
        isNormal = GUILayout.Toggle(isNormal, normalLabel, GUILayout.ExpandWidth(true));
        isError = GUILayout.Toggle(isError, errorLabel, GUILayout.ExpandWidth(true));      
        GUILayout.EndHorizontal();
        windowRect = GUILayout.Window(123456, windowRect, ConsoleWindow, "Console");
    }

    void ConsoleWindow(int windowID)
    {
        if (ScrollToBottom)
        {
            scrollPosition = GUILayout.BeginScrollView(Vector2.up * msgList.Count * 100.0f);
        }
        else
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
        }  
        for (int i = 0; i < msgList.Count; i++)
        {
            ConsoleMessage entry = msgList[i];
            if (isCollapse && checkIsSame(entry.message,msgList.GetRange(0,i)))
            {
                continue;
            }
            else
            {
                switch (entry.type)
                {
                    case LogType.Error:
                        if (isError)
                        {
                            GUI.contentColor = Color.red;
                            GUILayout.Label(entry.message + "\r\n" + entry.stackTrace);
                        } break;
                    case LogType.Log: if (isNormal) { GUI.contentColor = Color.white; GUILayout.Label(entry.message + "\r\n" + entry.stackTrace); } break;

                }
            }
        }     
        GUI.contentColor = Color.white;
        GUILayout.EndScrollView();

        GUI.DragWindow(new Rect(0, 0, 10000, scrollPosition.y));


    }


    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    void HandleLog(string message, string stackTrace, LogType type)
    {
        ConsoleMessage entry = new ConsoleMessage(message, stackTrace, type);
        msgList.Add(entry);
    }

    bool checkIsSame(string msg,List<ConsoleMessage> param)
    {        
        for (int i = 0; i < param.Count; i++)
        {
            if (param[i].message.CompareTo(msg) == 0)
            {
                return true;
            }           
        }
        return false;
    }
}


