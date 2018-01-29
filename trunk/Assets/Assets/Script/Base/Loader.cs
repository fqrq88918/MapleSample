using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
public class Loader : MonoBehaviour {
    public static Dictionary<Type, Type> m_map = new Dictionary<Type, Type>();
	// Use this for initialization
	
      
        
	// Use this for initialization
	void Start () {
	 
      
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void LoadUI(WindowType winType, Action OnLoad = null)
    {
        if (m_map.Count == 0)
        { init(); }
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type typeName=Type.GetType(winType.ToString());       
        object wintype = m_map[typeName].Assembly.CreateInstance(m_map[typeName].Name);
        PropertyInfo tmp = typeName.GetProperty("m_resUrl");
        PropertyInfo tmp1 = typeName.GetProperty("m_bundle");
       // Debug.Log(tmp.GetValue(wintype, null) as string);
        if (!string.IsNullOrEmpty(tmp.GetValue(wintype, null) as string))
        {
            Debug.Log("加载Resource资源");
        }
        if (!string.IsNullOrEmpty(tmp1.GetValue(wintype, null) as string))
        {
            Debug.Log("加载Resource资源");
        }
    }

    private static void init()
    {
        List<Type> types = new List<Type>();
        Assembly assembly = Assembly.GetExecutingAssembly();
        types.AddRange(assembly.GetTypes());
        for (int i = 0; i < types.Count; i++)
        {
            //  Debug.Log(types[i].Name);
            m_map.Add(types[i], types[i]);
        }
    }
}
