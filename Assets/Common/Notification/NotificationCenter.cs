using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Notification
{
    /// <summary>
    /// 通知发送者
    /// </summary>
    public GameObject sender;

    /// <summary>
    /// 通知内容
    /// 备注：在发送消息时需要装箱、解析消息时需要拆箱
    /// </summary>
    public object param;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="param"></param>
    public Notification(object param,GameObject sender = null)
    {
        this.sender = sender;
        this.param = param;
    }

    /// <summary>
    /// 实现ToString方法
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("sender={0},param={1}", this.sender, this.param);
    }
}

public class NotificationCenter {

    /// <summary>
    /// 通知中心单例
    /// </summary>
    private static NotificationCenter instance = null;
    public delegate void OnNotification(Notification notific);
    public static NotificationCenter Get()
    {
        if (instance == null)
        {
            instance = new NotificationCenter();
            return instance;
        }
        return instance;
    }


    //实现一对多的消息
    private Dictionary<string, Dictionary<GameObject, OnNotification>> m_dicEvent
    = new Dictionary<string, Dictionary<GameObject, OnNotification>>();

    public void ObjAddEventListener(string eventKey, GameObject obj,OnNotification eventListener)
    {
        if (!m_dicEvent.ContainsKey(eventKey))
        {
            Dictionary<GameObject, OnNotification> dic = new Dictionary<GameObject, OnNotification>();
            dic[obj] = eventListener;
            m_dicEvent[eventKey] = dic;
        }
        else
        {
            m_dicEvent[eventKey][obj] = eventListener;
        }
    }

    public void ObjRemoveEventListener(string eventKey,GameObject obj)
    {
        if (!m_dicEvent.ContainsKey(eventKey))
            return;

        m_dicEvent[eventKey].Remove(obj);
    }


    public void ObjDispatchEvent(string eventKey, object param = null,GameObject sender = null)
    {
        if (!m_dicEvent.ContainsKey(eventKey))
            return;
        List<GameObject> listRemoveKey = new List<GameObject>();
        List<OnNotification> listNoti = new List<OnNotification>();
        lock (m_dicEvent)
        {
            foreach (var it in m_dicEvent[eventKey])
            {
                if (it.Key != null)
                {
                    //it.Value(new Notification(param, sender));
                    listNoti.Add(it.Value);
                }
                else
                {
                    listRemoveKey.Add(it.Key);
                }
            }

            //删除已经Destory的GameObject
            for (int i = 0; i < listRemoveKey.Count; i++)
            {
                m_dicEvent[eventKey].Remove(listRemoveKey[i]);
            }

            for ( int  i = 0; i < listNoti.Count; i++)
            {
                listNoti[i](new Notification(param, sender));
            }
        }
    }

    /// <summary>
    /// 是否存在指定事件的监听器
    /// </summary>
    public bool HasEventListener(string eventKey)
    {
        return m_dicEvent.ContainsKey(eventKey);
    }

}
