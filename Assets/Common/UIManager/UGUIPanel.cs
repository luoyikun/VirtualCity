using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AssetBundles;
using UnityEngine.Events;

namespace Framework.UI
{
    public class UGUIPanel : BasePanel
    {
        public UnityAction m_actOpenFinish;
        private CanvasGroup mCanvasGroup;
        public bool m_isOpen = false;
        /// <summary>
        /// 页面进入显示，可交互
        /// </summary>
        public override void OnEnter(bool isAni = false)
        {
            EventEnable(true);

            gameObject.SetActive(true);

            if (isAni == true)
            {
                uiloadpanel.Instance.Open(false);
                //Vector3 oriScale = transform.Find("Content").localScale;
                Vector3 oriScale = Vector3.one;
                transform.Find("Content").localScale = Vector3.zero;
                transform.Find("Content").DOScale(oriScale, 0.15f).OnComplete( ()=> { m_isOpen = true; uiloadpanel.Instance.Close(); if(m_actOpenFinish != null) m_actOpenFinish(); });
            }

            OnOpen();

            if (isAni == false)
            {
                m_isOpen = true;
                if (m_actOpenFinish != null) m_actOpenFinish();
            }
        }

        //界面打开执行的操作
        public override void OnOpen()
        {

        }


        public override void OnClose()
        {
            m_isOpen = false;
        }
        /// <summary>
        /// 页面暂停（弹出了其他页面），不可交互
        /// </summary>
        public override void OnPause(bool isHide = false,bool isAcceptMsg = false)
        {
            if (isAcceptMsg == false)
            {
                OnClose();
            }
            if (isHide == true)
            {
                gameObject.SetActive(false);
            }
            EventEnable(false);
        }

        /// <summary>
        /// 页面继续显示（其他页面关闭），可交互
        /// </summary>
        public override void OnResume()
        {
            OnOpen();
            EventEnable(true);

            gameObject.SetActive(true);
        }

        /// <summary>
        /// 本页面被关闭（移除），不再显示在界面上
        /// </summary>
        public override void OnExit()
        {
            OnClose();
            gameObject.SetActive(false);
            //GameObject.Destroy(gameObject);
            //if (UIManager.Instance.panelDict.ContainsKey(m_type))
            //{
            //    UIManager.Instance.panelDict.Remove(m_type);
            //}
            //AssetBundleManager.UnloadAssetBundle(m_type, false, false);
        }

        public override void DestroySelf()
        {
            GameObject.Destroy(gameObject);
        }

        protected void EventEnable(bool enabled)
        {
            if (mCanvasGroup == null)
            {
                mCanvasGroup = transform.GetComponent<CanvasGroup>();
            }
            if (mCanvasGroup != null)
            {
                mCanvasGroup.blocksRaycasts = enabled;
            }
        }

        private void DelayEventEnable()
        {
            EventEnable(true);
        }
    }
}
