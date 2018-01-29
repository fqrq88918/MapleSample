using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIHelper : Singleton<UIHelper>
{
    private Dictionary<string, UIAtlas> m_atlasRes = new Dictionary<string, UIAtlas>();
    private Dictionary<string, Texture> m_textureRes = new Dictionary<string, Texture>();

    public void CreateWindow(Window win)
    {
        CreateWindow(win, null);
    }  

    public void CreateWindow(Window win, WindowFactory.VoidHandle handle,bool isShow=true)
    {
        Logger.DebugFormat("Create Window name：{0}，resType:{1}", win.Name, win.resType);
        if (WindowManager.instance.IsCloseingAll)
            return;
        if (win.IsLoad)
        {
            win.Show();
            return;
        }

        switch (win.resType)
        {
            case ResourceType.Resource:
                ResourceCenter.instance.LoadWindow(ResourceType.Resource, win.resUrl, delegate(object obj)
                {
                    win.m_gameObject = obj as GameObject;
                    WindowManager.instance.InsertWindow(win);
                    win.OnLoad();
                    if (handle != null)
                        handle();
                    if (isShow)
                        win.Show();
                   
                });
                break;
            case ResourceType.AssetBundle:
                ResourceCenter.instance.LoadWindow(ResourceType.AssetBundle, win.bundle, delegate(object obj)
                {
                    win.m_gameObject = obj as GameObject;
                    WindowManager.instance.InsertWindow(win);
                    win.OnLoad();
                    if (handle != null)
                        handle();
                    if (isShow)
                        win.Show();
                   
                });
                break;
        }
    }

    public GameObject Find(GameObject m_go, string goName)
    {
        foreach (Transform t in m_go.transform)
        {
            if (t.name.CompareTo(goName) == 0)
                return t.gameObject;
        }
        Logger.DebugErrorFormat("can't find gameObject Name:{0},parent:{1}", m_go.name, goName);
        return null;
    }
    public T Find<T>(GameObject m_go, string goName) where T : class
    {
        GameObject go = Find(m_go, goName);
        if (null != go)
        {
            T component = go.GetComponent<T>();
            return component;
        }
        Logger.DebugErrorFormat("can't find gameObject component:{0},Name:{1},parent:{2}", typeof(T).Name, m_go.name, goName);
        return null;
    }

    private void ReleaseAllAtlas()
    {
        var tmp = m_atlasRes.GetEnumerator();
        while (tmp.MoveNext())
        {
            Texture texture = tmp.Current.Value.texture;
            Resources.UnloadAsset(texture);
        }
        GC.Collect();
    }

    public void SetSprite(UISprite sprite,  string spriteName,string altasName="")
    {
        if (sprite == null)
        {
            Logger.DebugError("sprite is null");
            return;
        }
        if (string.IsNullOrEmpty(spriteName))
        {
            Logger.DebugError("spriteName is empty string");
            return;
        }
        Logger.DebugFormat("spriteName:{0},altasName={1}", spriteName, altasName);        
        if (!string.IsNullOrEmpty(altasName))
        {
            if (sprite.atlas!=null&&sprite.atlas.name == altasName)
                sprite.spriteName = spriteName;
            else
            {
                if (m_atlasRes.ContainsKey(altasName))
                {
                    sprite.atlas = m_atlasRes[altasName];
                    sprite.name = spriteName;
                }
                else
                {
                    ResourceCenter.instance.LoadAtlas(ResourceType.Resource, altasName, delegate(object param)
                    {
                        if (param != null)
                        {
                            UIAtlas tmp = ((UnityEngine.Object)param as GameObject).GetComponent<UIAtlas>();
                            if (m_atlasRes.ContainsKey(altasName) == false)
                                m_atlasRes.Add(altasName, tmp);
                            sprite.atlas = tmp;
                            sprite.spriteName = spriteName;
                        }
                    });
                }
            }
        }
        else
            sprite.spriteName = spriteName;
    }

    public void SetTexture(UITexture texture, string name)
    {
        if (texture == null)
        {
            Logger.DebugError("texture is null");
            return;
        }
        if (name == null)
        {
            Logger.DebugError("texture name is empty");
            return;
        }
        if (m_textureRes.ContainsKey(name))
            texture.mainTexture = m_textureRes[name];
        else
        {
            ResourceCenter.instance.LoadTexture(ResourceType.Resource, name, delegate(object obj)
            {
                if (obj != null)
                {
                    Texture m_texture = obj as Texture;
                    texture.mainTexture = m_texture;
                }
            });
        }
    }

    public void SetActive(GameObject go, bool isActive)
    {
        if (go == null)
            return;
        if (go.activeSelf)
        {
            if (isActive)
                go.transform.localScale = Vector3.one;
            else
                go.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.001f);
        }
        else
        {
            if (isActive)
                go.SetActive(true);           
        }

    }

  
}
