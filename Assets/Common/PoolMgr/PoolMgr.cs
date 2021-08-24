using DynamicFogAndMist;
using Framework.Pattern;
using KunPoolMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KunPoolMgr
{
    public class PoolData
    {
        public Transform m_poolPar;
        public Queue<GameObject> m_queChild = new Queue<GameObject>();

        //public PoolData(Transform _poolParent, List<GameObject> _pooledObjects)
        //{
        //    poolParent = _poolParent;
        //    pooledObjects = new List<GameObject>();
        //    pooledObjects = _pooledObjects;
        //}
    }
}

public class PoolMgr : SingletonMono<PoolMgr>
{
    public Dictionary<string, PoolData> m_dicPool = new Dictionary<string, PoolData>();
    List<Type> m_listType = new List<Type>();
    Transform m_trans;
    public void StartUp()
    {
        m_trans = this.transform;
        m_listType.Add(typeof(PoolKeyName));
        m_listType.Add(typeof(NavMeshSourceTag));
        m_listType.Add(typeof(NavMeshModifier));
        m_listType.Add(typeof(NavMeshSurface));
        m_listType.Add(typeof(ResetShader));
        m_listType.Add(typeof(MouseOrbitImproved));
        m_listType.Add(typeof(DynamicFog));
        m_listType.Add(typeof(AniSpeed));
    }

    public bool IsExistKey(string key)
    {
        return m_dicPool.ContainsKey(key);
    }

    public bool IsExistValue(string key)
    {
        bool ret = true;
        if (m_dicPool.ContainsKey(key) == false)
        {
            return false;
        }
        if (m_dicPool[key].m_queChild.Count == 0)
        {
            return false;
        }
        return ret;
    }

    public Transform GetPar(string key)
    {
        return m_dicPool[key].m_poolPar;
    }

    public void  CreateKey(string key)
    {
        GameObject obj = new GameObject(key);
        obj.transform.parent = m_trans;
        obj.SetActive(false);
        PoolData pool = new PoolData();
        pool.m_poolPar = obj.transform;
        m_dicPool[key] = pool;
    }

    //从缓冲池里得到一个obj
    public GameObject GetOne(string key)
    {
        GameObject go = PoolMgr.Instance.m_dicPool[key].m_queChild.Dequeue();
        return go;
    }

    //把一个创建好的obj 直接放到队列中
    //public void EnqueueOne(GameObject obj)
    //{
    //    m_dicPool[keyName].m_queChild.Enqueue(obj);
    //}

    //public void GetOne(string key, string assetName, string abName, Transform par, Vector3 pos, Vector3 angles, Vector3 scale, UnityAction<GameObject> onFinish = null, string path = "")
    //{
    //    // 当前不存在此条缓冲池
    //    if (IsExitKey(key) == false)
    //    {
    //        PoolData pool = new PoolData();
    //        pool.m_poolPar = CreateKey(key); 
    //    }

    //    if (m_dicPool[key].m_queChild.Count == 0)
    //    {
    //        AssetMgr.Instance.CreateObj(assetName, abName, par, pos, angles, scale, onFinish, path);
    //    }
    //}

    public void RecycleObj(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        if (obj.GetComponent<PoolKeyName>() == null)
        {
            Debug.Log("当前物体不可回收:"+ obj.name);
            return;
        }
        string keyName = obj.GetComponent<PoolKeyName>().m_keyName;
        List<MonoBehaviour> listDelete = new List<MonoBehaviour>();
        foreach (var item in obj.transform.GetComponentsInChildren<MonoBehaviour>())
        {
            bool isDelete = true;
            if (item == null)
            {
                Debug.LogError(item.name + ":回收项目为null");
            }
            for (int i = 0; i < m_listType.Count; i++)
            {
                if (item.GetType() == m_listType[i])
                {
                    isDelete = false;
                    break;
                }
            }
            if (isDelete == true)
            {
                listDelete.Add(item);
            }
        }

        foreach ( var item in obj.transform.GetComponentsInChildren<NavMeshAgent>())
        {
            item.enabled = false;
        }
        
        for (int i = listDelete.Count - 1; i >= 0; i--)
        {
            Destroy(listDelete[i]);
        }

        obj.transform.parent = m_dicPool[keyName].m_poolPar;
        m_dicPool[keyName].m_queChild.Enqueue(obj);
    }
}
