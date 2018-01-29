using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class ResourceCenter : Singleton<ResourceCenter>
{
    private List<ResLoadInfo> m_waitList = new List<ResLoadInfo>();
    private List<ResLoadInfo> m_cacheList = new List<ResLoadInfo>();
    private List<ResLoadInfo> m_loadedList = new List<ResLoadInfo>();
    private const int maxReq = 5;

    public void OnUpdate()
    {
        for (int i = 0; i < m_cacheList.Count; i++)
        {
            if (m_cacheList[i].m_isLoad)
            {
                if (m_cacheList[i].onLoad != null)
                {
                    m_cacheList[i].onLoad(m_cacheList[i].obj);
                }
                m_loadedList.Add(m_cacheList[i]);
                m_cacheList.RemoveAt(i);
                i--;
            }
        }

        if (m_waitList.Count > 0 && m_cacheList.Count < maxReq)
        {
            Main.m_mono.StartCoroutine(Load(m_waitList[0]));
            m_cacheList.Add(m_waitList[0]);
            m_waitList.RemoveAt(0);
        }

    }

    public void LoadUI(ResourceType type, string name, ResLoadInfo.loadHandle onLoad = null)
    {
        m_waitList.Add(new ResLoadInfo(type, name, onLoad));

    }

    public void LoadWindow(ResourceType type, string name, ResLoadInfo.loadHandle onLoad = null)
    {
        m_waitList.Add(new ResLoadInfo(type, name, onLoad));
    }

    IEnumerator Load(ResLoadInfo info)
    {
        string pathUrl = "";
        object obj = null;
        switch (info.m_type)
        {
            case ResourceType.Resource:
                pathUrl += info.resName;
                Logger.Debug(pathUrl);
                ResourceRequest resReq = Resources.LoadAsync(pathUrl);
                yield return resReq;
                if (resReq.isDone)
                {
                    if (resReq.asset != null)
                    {
                        obj = UnityEngine.Object.Instantiate(resReq.asset);
                        yield return obj;
                        info.obj = obj;
                        info.m_isLoad = true;
                    }
                }
                break;
            case ResourceType.AssetBundle: pathUrl += info.resName;
                WWW www = new WWW(pathUrl);
                yield return www;
                info.m_bundle = www.assetBundle;
                obj = UnityEngine.Object.Instantiate(info.m_bundle);
                yield return obj; info.obj = obj;
                info.m_isLoad = true;
                break;
        }
    }



    public void LoadAtlas(ResourceType type, string altasName, ResLoadInfo.loadHandle onLoad = null)
    {
        string filePath = "";
        filePath = Setup.UIResAtlasPath + altasName;
        m_waitList.Add(new ResLoadInfo(type, filePath, onLoad));
    }

    public void LoadTexture(ResourceType type, string textureName, ResLoadInfo.loadHandle onLoad = null)
    {
        string filePath = "";
        filePath = Setup.UIResTexturePath + textureName;
        m_waitList.Add(new ResLoadInfo(type, filePath, onLoad));
    }
    
}

public class ResLoadInfo
{
    public ResourceType m_type;
    public string resName;
    public bool m_isLoad;
    public delegate void loadHandle(object obj);
    public loadHandle onLoad;
    public object obj;
    public AssetBundle m_bundle;
    public ResLoadInfo(ResourceType param1, string param2, loadHandle param3)
    {
        m_type = param1;
        resName = param2;
        onLoad = param3;
        m_isLoad = false;
        m_bundle = null;
    }
}
