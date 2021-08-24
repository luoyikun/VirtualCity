/*
* NotificationDelegCenter
* 使用代理监听和派发事件
* 相对SendMessage使用delegate,提高了消息派发效率
* author : 大帅纷纭
*/
using System.Collections.Generic;
using Framework.Pattern;

namespace Framework.Event
{
    //C#在类定义外可以声明方法的签名（Delegate，代理或委托），但是不能声明真正的方法。
    public delegate void OnNotificationDelegate(EventData note);

    public class EventManager : Singleton<EventManager>
    {
        private Dictionary<string, OnNotificationDelegate> eventListerners = new Dictionary<string, OnNotificationDelegate>();

        /*
         * 监听事件
         */

        //添加监听事件
        public void AddEventListener(string type, OnNotificationDelegate listener)
        {
            if (!eventListerners.ContainsKey(type))
            {
                eventListerners.Add(type, null);
            }
            eventListerners[type] += listener;
        }

        //移除监听事件
        public void RemoveEventListener(string type, OnNotificationDelegate listener)
        {
            if (!eventListerners.ContainsKey(type))
            {
                return;
            }
            eventListerners[type] -= listener;
        }

        //移除某一类型所有的监听事件
        public void RemoveEventListener(string type)
        {
            if (eventListerners.ContainsKey(type))
            {
                eventListerners.Remove(type);
            }
        }

        /*
         * 派发事件
         */

        //派发数据
        public void DispatchEvent(string type, EventData note)
        {
            if (eventListerners.ContainsKey(type))
            {
                if (eventListerners[type] != null)
                {
                    eventListerners[type](note);
                }
            }
        }

        //派发无数据
        public void DispatchEvent(string type)
        {
            DispatchEvent(type, null);
        }

        //查找是否有当前类型事件监听
        public bool HasEventListener(string type)
        {
            return eventListerners.ContainsKey(type);
        }
    }
}
