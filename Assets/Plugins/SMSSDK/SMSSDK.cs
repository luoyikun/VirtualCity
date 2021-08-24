using System;
using System.Collections;
using UnityEngine;

namespace cn.SMSSDK.Unity
{
	public class SMSSDK : MonoBehaviour 
	{
		private SMSSDKInterface smssdkImpl;
		private SMSSDKHandler handler;
		//集成UI时用
		private SMSSDKGUIInterface smsSDKGUIImpl;
		//注:此处区分仅为demo测试而分，实际使用时可以不区分安卓或ios
		#if UNITY_ANDROID
		public string appKey = "moba6b6c6d6";
		public string appSerect = "b89d2427a3bc7ad1aea1e1e8c1d36bf3";
		public bool isWarn = true;
		#elif UNITY_IPHONE

		//		public string appKey = "114d7a34cf7ea";
		//		public string appSerect = "678ff550d7328de446585757c4e5de3f";
		//		public bool isWarn = true;
		#endif


		void Awake ()
		{
			Debug.Log("[SMSSDK]SMSSDK  ===>>>  Awake" );
			#if UNITY_ANDROID
			smssdkImpl = new AndroidImpl (gameObject);
			smsSDKGUIImpl = new AndroidGUIImpl (gameObject);
			#elif UNITY_IPHONE
			smssdkImpl = new iOSImpl (gameObject);
			//集成UI时用
			smsSDKGUIImpl = new iOSGUIImpl (gameObject);
			#endif
			//此处如果解注释，会在Unity里面造成报错
			//			smssdkImpl.init (appKey ,appSerect ,isWarn);
			//			smssdkImpl.enableWarn (isWarn);
		}


		/// <summary>
		/// Calls the back.
		/// </summary>
		/// <param name="callBackData">Call back data.</param>
		private void _callBack (string callBackData)
		{
//			print ("从OC层回调到C#层");

			if (callBackData == null)
			{
				return;
			}

			Hashtable res = (Hashtable)MiniJSON.jsonDecode(callBackData);
			if (res == null || res.Count <= 0)
			{
				return;
			}
//			print ("Hashtable*******数据"+ res);
			int status = Convert.ToInt32(res["status"]);
			int action = Convert.ToInt32(res["action"]);
			// Success = 1, Fail = 2
			switch (status)
			{
			case 1:
				{ 
					Debug.Log(callBackData);
					object resp = res["res"];
//					print ("回调成功"+resp);
					if(handler != null)
						handler.onComplete(action, resp);
					break;
				}
			case 2:
				{
					Debug.Log(callBackData);
					object throwable = res["res"];
//					print ("回调失败" + throwable);
					if (handler != null)
						handler.onError(action, throwable);
					break;
				}
			}

		}

		/// <summary>
		/// Sets the handler.
		/// </summary>
		/// <param name="handler">Handler.</param>
		public void setHandler(SMSSDKHandler handler)
		{
			this.handler = handler;
		}


		/// <summary>
		/// Init the specified appKey, appSerect and isWarn.
		/// </summary>
		/// <param name="appKey">App key.</param>
		/// <param name="appSerect">App serect.</param>
		/// <param name="isWarn">If set to <c>true</c> is warn.</param>
		public void init (string appKey, string appSerect, bool isWarn)
		{
			if (smssdkImpl != null) {
				smssdkImpl.init (appKey, appSerect,isWarn);
			}
		}


		/// <summary>
		/// Gets the code.
		/// </summary>
		/// <param name="getCodeMethodType">Get code method type.</param>
		/// <param name="phoneNumber">Phone number.</param>
		/// <param name="zone">Zone.</param>
		public void getCode (CodeType getCodeMethodType, string phoneNumber, string zone, string tempCode)
		{
			if (smssdkImpl != null) 
			{
				smssdkImpl.getCode (getCodeMethodType, phoneNumber, zone, tempCode);
			}
		}


		/// <summary>
		/// Commits the code.
		/// </summary>
		/// <param name="verificationCode">Verification code.</param>
		/// <param name="phoneNumber">Phone number.</param>
		/// <param name="zone">Zone.</param>
		public void commitCode ( string phoneNumber,string zone, string verificationCode)
		{
			if (smssdkImpl != null) {
				smssdkImpl.commitCode (phoneNumber, zone, verificationCode);
			}
		}


		/// <summary>
		/// Gets the supported country code.
		/// </summary>
		public void  getSupportedCountryCode ()
		{
			if (smssdkImpl != null) {
				smssdkImpl.getSupportedCountry ();
			}
		}


		/// <summary>
		/// Gets the friends.
		/// </summary>
		public void getFriends ()
		{
			if (smssdkImpl != null) {
				smssdkImpl.getFriends ();
			}
		}


		/// <summary>
		/// Submits the user info.
		/// </summary>
		/// <param name="userInfo">User info.</param>
		public void submitUserInfo (UserInfo userInfo)
		{
			if (smssdkImpl != null) {
				smssdkImpl.submitUserInfo (userInfo);
			}
		}


		/// <summary>
		/// Gets the version.
		/// </summary>
		/// <returns>The version.</returns>
		public void getVersion ()
		{
			if (smssdkImpl != null) {
				smssdkImpl.getVersion ();
			}
		}


		/// <summary>
		/// Enables the warn.
		/// </summary>
		/// <param name="state">If set to <c>true</c> state.</param>
		public void enableWarn (bool state)
		{
			if (smssdkImpl != null) {
				smssdkImpl.enableWarn (state);
			}
		}

		//下面两个方法是集成UI的方法
		/// <summary>
		/// Shows the register page.
		/// </summary>
		public void showRegisterPage(CodeType getCodeMethodType, string tempCode)
		{
			if (smsSDKGUIImpl != null)
			{
				smsSDKGUIImpl.showRegisterPage (getCodeMethodType, tempCode);
			}
		}

		public void showContactsPage()
		{
			if(smsSDKGUIImpl != null)
			{
				smsSDKGUIImpl.showContactsPage ();
			}

		}

	}

}
