using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class ResourceCenter : MonoBehaviour
{
    public static List<ResLoadInfo> m_waitList = new List<ResLoadInfo>();
    public static bool isLoading = false;
    // Use this for initialization

    void Awake()
    {
        isLoading = false;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isLoading == false)
        {
            if (m_waitList.Count > 0)
            {
                isLoading = false;
                StartCoroutine(Load(m_waitList[0]));
                m_waitList.RemoveAt(0);
            }
        }
    }

    public static void LoadUI(ResourceType type, string name, Action onLoad = null)
    {
        m_waitList.Add(new ResLoadInfo(type, name, onLoad));

    }
    IEnumerator Load(ResLoadInfo info)
    {
        string pathUrl = "";
        WWW www;
        switch (info.m_type)
        {
            case ResourceType.Resource: pathUrl += info.resName;
                ResourceRequest resReq = Resources.LoadAsync(pathUrl);
                yield return resReq;
                GameObject go = Instantiate(resReq.asset) as GameObject;
                break;
            case ResourceType.AssetBundle: pathUrl += info.resName; break;
        }

        //yield return www;
        //if (info.OnLoad != null)
        //{
        //    info.OnLoad();
        //}
        //isLoading = false;

    }
}

public class ResLoadInfo
{
    public ResourceType m_type;
    public string resName;
    public Action OnLoad;
    public ResLoadInfo(ResourceType param1, string param2, Action param3)
    {
        m_type = param1;
        resName = param2;
        OnLoad = param3;
    }
}
