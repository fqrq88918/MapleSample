using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Global:Singleton<Global> {
    public GameObject UIRoot;
    private GameObject m_uiroot;
    public Camera m_camera;

    public void Init()
    {
        m_uiroot = GameObject.Find("UI Root");
        m_camera = GameObject.Find("Camera").GetComponent<Camera>();
		Logger.Debug("=======Global is Init========");
    }

    
}
