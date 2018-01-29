using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloatMessageWindow : SingletonWindow<FloatMessageWindow>
{
    private List<string> m_msgList = new List<string>();
    private List<string> m_playingList = new List<string>();
    private List<GameObject> m_cacheGoLst = new List<GameObject>();
    private GameObject templateGo;
    private Transform goParent;
    private bool m_canPlay;
    private Vector3 startPos=new Vector3(0,80,0);
    private Vector3 endPos=new Vector3(0,160,0);
    private WaitForSeconds wfs = new WaitForSeconds(0.3f);
    public override string resUrl
    {
        get
        {
            return Setup.UIResPath + "UI_ConfirmMsg";
        }
    }

    public FloatMessageWindow()
    {
        SortingOrder = 90;
        m_wndType = winType.TypeCache;
    }

    public override void RegistEvents()
    {
        base.RegistEvents();
        RegistEvent(UIEventType.TypeShowFloatMessageWin, delegate(object[] param) 
        {
            string msgStr = (string)param[0];
            m_msgList.Add(msgStr);
        });
    }

    public override void OnInit()
    {
        base.OnInit();
        m_canPlay = true;
        templateGo = Find("msgItem");
        goParent = m_gameObject.transform;
        m_mono.StartCoroutine(CountCD());
    }

    public override void OnShow()
    {
        base.OnShow();
        if (m_canPlay&&m_msgList.Count>0&&m_canPlay==true)
        {
            Play();
        }
    }

    public override void Close()
    {
        base.Close();
        m_mono.StopAllCoroutines();
    }


    private GameObject GetMsgItem()
    {
        for (int i = 0; i < m_cacheGoLst.Count; i++)
        {
            if (m_cacheGoLst[i].activeSelf == false || m_cacheGoLst[i].transform.localScale.x < 0.1f)
            {
                SetActive(m_cacheGoLst[i], false);
                return m_cacheGoLst[i];
            }
        }
        GameObject go = GameObject.Instantiate(templateGo);
        if (go != null)
        {
            go.transform.parent = goParent;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = startPos;
            return go;
        }
        return null;
    }

    private void Play()
    {
        GameObject go = GetMsgItem();
        Find<UILabel>(go, "msgLabel").text = m_msgList[0];
        m_msgList.RemoveAt(0);
        m_canPlay = false;
        Utility.instance.PlayTweenPosition(go, startPos, endPos, 1, delegate() 
        { 
            //recycle play item
            SetActive(go, false);
            m_cacheGoLst.Add(go);
        });
        Utility.instance.PlayTweenAlpha(go, 0, 1,02f,delegate() 
        {
            Utility.instance.PlayTweenAlpha(go, 1, 1, 0.6f, delegate() 
            {
                Utility.instance.PlayTweenAlpha(go, 1, 0, 0.2f);
            });
               
        });
      
    }


    

    IEnumerator CountCD()
    {
        yield return wfs;
        if (m_msgList.Count > 0)
        {
            Play();
        }
        
    }
}
