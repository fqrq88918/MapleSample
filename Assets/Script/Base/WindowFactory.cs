using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class WindowFactory:Singleton<WindowFactory> {
    private Dictionary<WindowType, Window> m_winList = new Dictionary<WindowType, Window>();
    public delegate void VoidHandle(params object[] param);

    public void CreateWindow(WindowType winType, VoidHandle handle=null,bool isShow=true)
    {
        Window win = WindowManager.instance.GetWindow(winType);
        if (win == null)
        {
            win = (Window)Activator.CreateInstance(Type.GetType(WindowManager.instance.GetWindowTypeName(winType)));
            UIHelper.instance.CreateWindow(win, handle, isShow);
        }
        else
        {
            if (isShow)
            {                
                if (handle != null)
                    handle();
                win.Show();
            }
        }
    }

    

    public void CloseWindow(WindowType winType)
    {
        if (m_winList.ContainsKey(winType))        
            m_winList[winType].Close();        
    }

    public void HideWindow(WindowType winType)
    {
        if (m_winList.ContainsKey(winType))
            m_winList[winType].Hide();
    }


    public void OnUpdate()
    {
 
    }

}
