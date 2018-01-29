﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class WindowManager:Singleton<WindowManager>  {
    private Dictionary<WindowType, Window> m_winDic = new Dictionary<WindowType, Window>();
	private List<Window> m_winList = new List<Window> ();
    private bool m_isCloseingAll;

    public bool IsCloseingAll
    {
        get
        {
            return m_isCloseingAll;
        }     
    }

    public  void InsertWindow(WindowType wintype,Window win)
    {
        if (m_winDic.ContainsKey(wintype))        
            return;
        m_winDic.Add(wintype, win);
        m_winList.Add(win);
    }
    public void InsertWindow(Window win)
    {
        WindowType winType = (WindowType)Enum.Parse(typeof(WindowType), win.Name);
        if (m_winDic.ContainsKey(winType))
            return;
        m_winDic.Add(winType, win);
        m_winList.Add(win);
    }

    public void RemoveWindow(Window win)
    {
        WindowType winType = (WindowType)Enum.Parse(typeof(WindowType), win.Name);
        if (m_winDic.ContainsKey(winType))
            m_winDic.Remove(winType);
        m_winList.Remove(win);
        win = null;
    }

    public string GetWindowTypeName(WindowType wintype)
    {
        string str;
        if (m_winDic.ContainsKey(wintype))
        {
            str = m_winDic[wintype].Name;
            return str;
        }
        str = wintype.ToString();
        return str;
        
    }

    public void OnInit()
    {
        m_isCloseingAll = false;
        m_winDic.Clear();
        m_winList.Clear();
    }

	public void OnUpdate()
	{
		for (int i = 0; i < m_winList.Count; i++) {
            m_winList[i].OnUpdate();
		}
	}

    public void CloseWindow(WindowType winType)
    {
        if (m_winDic.ContainsKey(winType))
        {
            m_winDic[winType].Close();
            m_winList.Remove(m_winDic[winType]);
            m_winDic.Remove(winType);          
        }
    }

    public Window GetWindow(WindowType winType)
    {
        if (m_winDic.ContainsKey(winType))
            return m_winDic[winType];
        return null;
    }

    public void HideWindow(WindowType winType)
    {
        if (m_winDic.ContainsKey(winType))
            m_winDic[winType].Hide();
    }

    public void CloseAllWindows()
    {
        m_isCloseingAll = true;
        for (int i = 0; i < m_winList.Count; i++)
        {
            if (m_winList[i].m_wndType == winType.TypeCache)
                m_winList[i].Hide();
            if (m_winList[i].m_wndType == winType.TypeNormal)
            {
                m_winList[i].Close();
                i--;                 
            }
        }
        m_isCloseingAll = false;
    }

    public void CloseAllWIndows(Window win)
    {
        m_isCloseingAll = true;
        for (int i = 0; i < m_winList.Count; i++)
        {
            if (m_winList[i].Name ==win.Name)
                continue;
            if (m_winList[i].m_wndType == winType.TypeCache)
                m_winList[i].Hide();
            if (m_winList[i].m_wndType == winType.TypeNormal)
            {
                m_winList[i].Close();
                i--;
            }
        }
        m_isCloseingAll = false;
    }

}
