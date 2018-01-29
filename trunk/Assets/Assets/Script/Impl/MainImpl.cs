using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class MainImpl : MonoBehaviour {
    public Dictionary<Type, Type> m_map = new Dictionary<Type, Type>();
	// Use this for initialization
	void Start () {
        if(gameObject.GetComponent<Loader>()==null)
        {
            gameObject.AddComponent<Loader>();
        }
        List<Type> types = new List<Type>();
        Assembly assembly = Assembly.GetExecutingAssembly();
        types.AddRange(assembly.GetTypes());
        for (int i = 0; i < types.Count; i++)
        {
           // Debug.Log(types[i].Name);
            m_map.Add(types[i],types[i]);
        }
        WindowManager.CreateWindow(WindowType.mainWin);
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
