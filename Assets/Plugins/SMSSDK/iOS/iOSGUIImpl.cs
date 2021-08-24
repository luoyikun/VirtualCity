using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace cn.SMSSDK.Unity
{
	#if UNITY_IPHONE
	public class iOSGUIImpl : SMSSDKGUIInterface {

		[DllImport("__Internal")]
		 static extern void __iosSMSSDKRegisterAppWithAppKeyAndAppSerect (string appKey, string appSerect);
		[DllImport("__Internal")]
		static extern void __showRegisterView (CodeType getCodeMethod,string tempCode,string observer);
		[DllImport("__Internal")]
		static extern void __showContractFriendsView (string observer);



		private string _callbackObjectName = "Main Camera";

		public iOSGUIImpl (GameObject go) 
		{
			try{
				_callbackObjectName = go.name;
			} catch(Exception e) {
				Console.WriteLine("{0} Exception caught.", e);
			}
		}


		/// <summary>
		/// Init the specified appKey, appSerect and isWarn.
		/// </summary>
		/// <param name="appKey">App key.</param>
		/// <param name="appSerect">App serect.</param>
		/// <param name="isWarn">If set to <c>true</c> is warn.</param>
		public override void init (string appKey, string appSerect, bool isWarn)
		{
			__iosSMSSDKRegisterAppWithAppKeyAndAppSerect (appKey ,appSerect);
		}

		/// <summary>
		/// Shows the register view.
		/// </summary>
		public override void showRegisterPage(CodeType getCodeMethodType,string tempCode)
		{
			__showRegisterView (getCodeMethodType,tempCode, _callbackObjectName);
		}

		/// <summary>
		/// Shows the contracts friends view.
		/// </summary>
		public override void showContactsPage()
		{
			__showContractFriendsView (_callbackObjectName);
		}
	}
	#endif
}