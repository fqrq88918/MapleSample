using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class Main : MonoBehaviour {
    public static MonoBehaviour m_mono;
    public UIAtlas m_atlas;
	void Start () {
		 Logger.Debug("=====================MainImpl is awake===========");
        m_mono = this;
        DontDestroyOnLoad(this);        
        WindowManager.instance.OnInit();
        GameState.instance.OnInit();     
	}

    void Update()
    {
        GameState.instance.OnUpdate();
    }
		
}
