using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;

public class ObjPoolMgr : MonoBehaviour {

    //public static ObjPoolMgr m_instance = null;

    //public static ObjPoolMgr mInstance
    //{
    //    get
    //    {
    //        if (m_instance == null)
    //        {
    //            var g = new GameObject("ObjPoolMgr");
    //            m_instance = g.AddComponent<ObjPoolMgr>();
    //            UnityEngine.Object.DontDestroyOnLoad(g);
    //        }
    //        return m_instance;
    //    }
    //}

    public static GameObject SpawnPrefab(GameObject tmp,Vector3 pos, Quaternion qua, Vector3 scale, Transform par,Transform poolPar = null)
    {
        GameObject clone = LeanPool.SpawnObj(tmp, pos, qua, par,poolPar);
        clone.SetActive(true);
        clone.transform.localScale = scale;
        return clone;
    }

    public static void Despawn(GameObject obj)
    {
        LeanPool.Despawn(obj);
    }
}
