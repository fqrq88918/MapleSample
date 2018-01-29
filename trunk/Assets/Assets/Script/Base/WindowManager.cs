using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class WindowManager  {
    private Dictionary<WindowType, Window> m_winList = new Dictionary<WindowType, Window>();

    public static void CreateWindow(WindowType wintype,Action OnLoad=null)
    {
        Loader.LoadUI(wintype);
    }
}
