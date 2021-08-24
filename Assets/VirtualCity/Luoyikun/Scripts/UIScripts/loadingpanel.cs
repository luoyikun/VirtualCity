using Framework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class loadingpanel : UGUIPanel
{

    public Slider m_slider;
    public Text m_textPrecent;

    //待缓冲的物体
    public Dictionary<string, Type> m_dicObjToLoad = new Dictionary<string, Type>();
    public List<string> m_listObjToPool = new List<string>();
    static public int m_preCnt = 0;
    static public int m_preMax = 0;
    static public int m_preLoadSprMax = 0;
    static public int m_preLoadSprCnt = 0;
    public UnityAction m_finishLoad;
    public UnityAction m_finishLoadBefore;
    // 已经缓存的obj
    public static Dictionary<string, GameObject> m_dicBufObj = new Dictionary<string, GameObject>();

    // 已经缓存的图集
    public static Dictionary<string, string> m_dicBufSpr = new Dictionary<string, string>();

    //待缓存的图集
    public Dictionary<string, string> m_dicSprToLoad = new Dictionary<string, string>();
    public override void OnOpen()
    {
        m_dicObjToLoad.Clear();
        m_dicSprToLoad.Clear();
        m_listObjToPool.Clear();
        m_preCnt = 0;
        m_preMax = 0;
        m_preLoadSprCnt = 0;
        m_preLoadSprMax = 0;
        m_slider.value = 0;
        m_finishLoad = null;
        m_finishLoadBefore = null;
    }

    public override void OnClose()
    {

    }

    public void ClearLast()
    {

    }

    public static void RemoveAll()
    {
        foreach (var item in m_dicBufObj)
        {
            if (item.Value != null)
            {
                Destroy(item.Value);
            }
        }
        m_dicBufObj.Clear();
    }
    public void AddObjPreLoad(string name)
    {
        if (m_dicBufObj.ContainsKey(name) == false)
        {
            m_dicObjToLoad[name] = typeof(GameObject);
        }
    }


    public void AddSprPreLoad(string name, string abName)
    {

        if (m_dicBufSpr.ContainsKey(abName) == false)
        {
            m_dicSprToLoad[name] = abName;
        }
    }

    public void AddToPool(string name)
    {
        if (PoolMgr.Instance.IsExistValue(name) == false)
        {
            m_listObjToPool.Add(name);
        }
    }

    public void AllPreLoad()
    {
        m_preMax += m_dicObjToLoad.Count + m_dicSprToLoad.Count + m_listObjToPool.Count;
        m_preLoadSprMax += m_dicSprToLoad.Count;

        Debug.Log("总共要加载:" + m_preMax);
        if (m_preMax == 0)
        {
            if (m_finishLoadBefore != null)
            {
                m_finishLoadBefore();
            }
            UIManager.Instance.PopSelf();
            if (m_finishLoad != null)
            {

                m_finishLoad();
            }
            return;
        }

        if (m_preLoadSprMax != 0)
        {
            foreach (var item in m_dicSprToLoad)
            {
                m_dicBufSpr[item.Value] = item.Value;
                AssetMgr.Instance.CreateSpr(item.Key, item.Value, SprPreAdd);
            }
        }
        else if (m_preLoadSprMax == 0)
        {
            PreLoadObj();
        }
       
    }

    void PreLoadObj()
    {
        foreach (var item in m_dicObjToLoad)
        {
            if (item.Value == typeof(GameObject))
            {
                AssetMgr.Instance.CreateObjOne(item.Key, item.Key, VirtualCityMgr.m_instance.m_bufferPar, Vector3.zero, Vector3.zero, new Vector3(-10000, 0, 0), ObjPreAdd);
            }

            else if (item.Value == typeof(Sprite))
            {

            }
        }


        for (int i = 0; i < m_listObjToPool.Count; i++)
        {
            if (PoolMgr.Instance.IsExistKey(m_listObjToPool[i]) == false)
            {
                PoolMgr.Instance.CreateKey(m_listObjToPool[i]);
            }
            Transform poolPar = PoolMgr.Instance.GetPar(m_listObjToPool[i]);
            AssetMgr.Instance.CreateObj(m_listObjToPool[i], m_listObjToPool[i], poolPar, Vector3.zero, Vector3.zero, new Vector3(-10000, 0, 0), ObjAddPool);
        }

    }
    void ObjAddPool(GameObject obj)
    {
        m_preCnt++;
        Debug.Log("预加载计数:" + m_preCnt);
        m_slider.value = (float)m_preCnt / (float)(m_preMax);
        m_textPrecent.text = (m_slider.value * 100.0f).ToString("0.0") + "%";
        if (obj.GetComponent<PoolKeyName>() == null)
        {
            obj.AddComponent<PoolKeyName>();
        }
        obj.GetComponent<PoolKeyName>().m_keyName = obj.name;
        PoolMgr.Instance.RecycleObj(obj);
        //m_dicBufObj[obj.name] = obj;
        if (m_preCnt == m_preMax)
        {
            Debug.Log("预加载完成");
            if (m_finishLoadBefore != null)
            {
                m_finishLoadBefore();
            }
            UIManager.Instance.PopSelf();
            if (m_finishLoad != null)
            {
                m_finishLoad();
            }
        }
    }
    void ObjPreAdd(GameObject obj)
    {
        m_preCnt++;
        Debug.Log("预加载计数:" + m_preCnt + ":" + obj.name);
        m_slider.value = (float)m_preCnt / (float)(m_preMax);
        m_textPrecent.text = (m_slider.value * 100.0f).ToString("0.0") + "%";
        m_dicBufObj[obj.name] = obj;
        if (m_preCnt == m_preMax)
        {
            Debug.Log("预加载完成");
            if (m_finishLoadBefore != null)
            {
                m_finishLoadBefore();
            }
            UIManager.Instance.PopSelf();
            if (m_finishLoad != null)
            {

                m_finishLoad();
            }
        }
    }

    void SprPreAdd(Sprite spr)
    {
        m_preCnt++;
        m_preLoadSprCnt++;
        Debug.Log("预加载计数:" + m_preCnt);
        m_slider.value = (float)m_preCnt / (float)(m_preMax);
        m_textPrecent.text = (m_slider.value * 100.0f).ToString("0.0") + "%";

        if (m_preLoadSprCnt == m_preLoadSprMax)
        {
            PreLoadObj();
        }
        if (m_preCnt == m_preMax)
        {
            Debug.Log("预加载完成");
            if (m_finishLoadBefore != null)
            {
                m_finishLoadBefore();
            }
            UIManager.Instance.PopSelf();
            if (m_finishLoad != null)
            {

                m_finishLoad();
            }
        }
    }

}
