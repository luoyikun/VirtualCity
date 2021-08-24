using UnityEngine;

namespace Framework.UI
{
    //所有面板的公共基类
    public class BasePanel : MonoBehaviour
    {
        public string m_type;
        /// <summary>
        /// 页面进入显示，可交互
        /// </summary>
        public virtual void OnEnter(bool isAni =false)
        {
        }

        /// <summary>
        /// 页面暂停（弹出了其他页面），不可交互
        /// </summary>
        public virtual void OnPause(bool isHide = false,bool isAcceptMsg = false)
        {

        }

        /// <summary>
        /// 页面继续显示（其他页面关闭），可交互
        /// </summary>
        public virtual void OnResume()
        {

        }

        /// <summary>
        /// 本页面被关闭（移除），不再显示在界面上
        /// </summary>
        public virtual void OnExit() { }

        public virtual void OnOpen()
        {

        }


        public virtual void OnClose()
        {
        }

        public virtual void DestroySelf()
        {
            GameObject.Destroy(gameObject);
        }
    }
}