using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace cn.SMSSDK.Unity
{
	#if UNITY_IPHONE || UNITY_IOS

	public class iOSImpl : SMSSDKInterface 
	{
		// Use this for initialization
		[DllImport("__Internal")]
		private static extern void __iosSMSSDKRegisterAppWithAppKeyAndAppSerect (string appKey, string appSerect);
		[DllImport("__Internal")]
		private static extern void __iosGetVerificationCode (CodeType getCodeMethod, string phoneNumber, string WindZone, string tempCode, string observer);
		[DllImport("__Internal")]
		private static extern void __iosCommitVerificationCode (string phoneNumber, string zone, string verificationCode,string observer);
		[DllImport("__Internal")]
		private static extern void __iosGetCountryZone (string observer);
		[DllImport("__Internal")]
		private static extern void __iosGetAllContractFriends (string observer);
		[DllImport("__Internal")]
		private static extern void __iosSubmitUserInfo (string userInfoString,string observer);
		[DllImport("__Internal")]
		private static extern void __iosSMSSDKVersion (string observer);
		[DllImport("__Internal")]
		private static extern void __iosEnableAppContractFriends (bool state);


		private string _callbackObjectName = "Main Camera";

		public iOSImpl (GameObject go) 
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
			if(Application.platform == RuntimePlatform.IPhonePlayer)
			{
				__iosSMSSDKRegisterAppWithAppKeyAndAppSerect (appKey, appSerect);
			
			}
		}


		/// <summary>
		/// Gets the code.
		/// </summary>
		/// <param name="getCodeMethod">Get code method.</param>
		/// <param name="phoneNumber">Phone number.</param>
		/// <param name="zone">Zone.</param>
		public override void getCode (CodeType getCodeMethod, string phoneNumber, string zone, string tempCode)
		{
			__iosGetVerificationCode (getCodeMethod, phoneNumber, zone, tempCode, _callbackObjectName);
		}


		/// <summary>
		/// Commits the code.
		/// </summary>
		/// <param name="phoneNumber">Phone number.</param>
		/// <param name="zone">Zone.</param>
		/// <param name="verificationCode">Verification code.</param>
		public override void commitCode (string phoneNumber, string zone, string verificationCode)
		{
			__iosCommitVerificationCode (phoneNumber,zone,verificationCode,_callbackObjectName);
		}



		/// <summary>
		/// Gets the supported country.
		/// </summary>
		public override void getSupportedCountry ()
		{
			__iosGetCountryZone (_callbackObjectName);
		}

		/// <summary>
		/// Gets the friends.
		/// </summary>
		public override void getFriends ()
		{
			__iosGetAllContractFriends (_callbackObjectName);
		}

		/// <summary>
		/// Submits the user info.
		/// </summary>
		/// <param name="userInfo">User info.</param>
		public override void submitUserInfo (UserInfo userInfo)
		{
			Hashtable userHash = new Hashtable ();
			userHash.Add ("avatar",userInfo.avatar);
			userHash.Add ("phoneNumber",userInfo.phoneNumber);
			userHash.Add ("nickName",userInfo.nickName);
			userHash.Add ("uid",userInfo.uid);

			string userInfoString = MiniJSON.jsonEncode (userHash);
			__iosSubmitUserInfo (userInfoString,_callbackObjectName);
		}

		/// <summary>
		/// Gets the version.
		/// </summary>
		public override void getVersion ()
		{
			__iosSMSSDKVersion (_callbackObjectName);
		}
		/// <summary>
		/// Enables the warn.
		/// </summary>
		/// <param name="state">If set to <c>true</c> state.</param>
		public override void enableWarn (bool state)
		{
			__iosEnableAppContractFriends (state);
		}
	}
	#endif
}