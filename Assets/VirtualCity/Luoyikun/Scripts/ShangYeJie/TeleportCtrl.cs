using Framework.Event;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCtrl : MonoBehaviour {

    public ShopsProperties m_info;
    // Use this for initialization
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("前往：" + m_info.moduleId);
            EventManager.Instance.DispatchEvent(Common.EventStr.StopNav);
            VirtualCityMgr.GotoShop((long)m_info.id);
        }
    }


}
