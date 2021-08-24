using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Framework.Tools;


namespace Framework.UI
{
    public class Hint : UGUIPanel
    {
        public RectTransform mImageRectTrans;
        public Text mText;

		private Tween[] mTweens;

		private static BufferPool mTipsPool;
		private static List<Hint> mHintList = new List<Hint>();

        public CanvasGroup m_group;
		void Start()
		{
			mHintList.Add (this);
		}

		void OnDestroy()
		{
			mHintList.Remove (this);
		}

        public void Show(string text, Color color, float time)
        {
			if (mTweens != null)
			{
				for (var i = 0; i < mTweens.Length; i++)
				{
					if (mTweens [i] != null)
					{
						mTweens [i].Kill ();
					}
				}
			}
			else
			{
				mTweens = new Tween[2];
			}

            mText.text = text;
            mText.color = color;

            //mText.transform.localScale = new Vector3(1f, 0.0001f, 1);
            //mImageRectTrans.sizeDelta = new Vector2(mText.preferredWidth + 100f, mText.preferredHeight + 100f);
            //mImageRectTrans.localScale = new Vector3(0.0001f, 1, 1);

            //mTweens[0] = mImageRectTrans.DOScale(Vector3.one, 0.5f);
            //mTweens[1] = mText.GetComponent<RectTransform>().DOScale(Vector3.one, 0.5f).SetDelay(0.5f);
            //mTweens[2] = mImageRectTrans.DOScale(new Vector3(1, 0.001f, 1), 0.5f).SetDelay(time - 0.5f);

            m_group.alpha = 0;
            mTweens[0] =  m_group.DOFade(1, 0.5f);

            mTweens[1] = m_group.DOFade(0, 0.5f).SetDelay(time - 0.5f); ;
            Invoke ("DelayHide", time);
        }

		private void HideImmediate()
		{
			CancelInvoke ("DelayHide");
			if (mTweens != null)
			{
				for (var i = 0; i < mTweens.Length; i++)
				{
					if (mTweens [i] != null)
					{
						mTweens [i].Kill ();
					}
				}
			}
			mTipsPool.Recycle (this.gameObject);
		}

		private void DelayHide()
		{
			mTipsPool.Recycle (this.gameObject);
		}

        public static float LoadTips(string text)
        {
            //float tempSpeed = 2.5f;
            float tempSpeed = 3.5f;
            int time = Mathf.CeilToInt(text.Length / tempSpeed);
            float fTime = (float)time;
            fTime = Mathf.Clamp(fTime, 0.5f, 1.5f);

            LoadTips(text, Color.white, fTime);
            return fTime;
        }

        public static float LoadTips(string text, Color color)
		{
			//float tempSpeed = 2.5f;
            float tempSpeed = 3.5f;
            int time = Mathf.CeilToInt(text.Length / tempSpeed);
            float fTime = (float)time;
            fTime = Mathf.Clamp(fTime, 0.5f, 1.5f);

            LoadTips (text, color, fTime);
			return fTime;
		}

		public static void LoadTips(string text, Color color, float time)
		{
			if (mTipsPool == null)
			{
                GameObject tempPrefab = UIManager.Instance.GetLoadObject("UIPrefabs/hintpanel");

                    //UIManager.Instance.LoadPanel(UIPanelName.hintpanel,UIManager.CanvasType.Screen).gameObject;
                Transform tempParent = UIManager.Instance.GetParent (UIManager.CanvasType.Screen);
				mTipsPool = new BufferPool (tempPrefab, tempParent, 1);
			}
			GameObject tempObj = mTipsPool.GetObject ();
			tempObj.transform.SetAsLastSibling ();
			Hint tempHint = tempObj.GetComponent<Hint>();
			tempHint.Show (text, color, time);
		}

		public static void Hide()
		{
			for (var i = 0; i < mHintList.Count; i++)
			{
				Hint tempHint = mHintList [i];
				if (tempHint == null || tempHint.gameObject.activeSelf == false)
				{
					continue;
				}
				tempHint.HideImmediate ();
			}
		}
    }
}

