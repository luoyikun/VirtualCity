using UnityEngine;
using System.Collections;
using System;
using cn.SMSSDK.Unity; 

namespace cn.SMSSDK.Unity
{
	public class Demo : MonoBehaviour,SMSSDKHandler {

		// Use this for initialization
		public GUISkin demoSkin;
		public SMSSDK smssdk;
		public UserInfo userInfo;

		//please add your phone number
		private string phone = "";
		private string zone = "86";
		private string tempCode= "1319972";
		private string code = "";
		private string result = null;


		void Start () 
		{
			Debug.Log("[SMSSDK]Demo  ===>>>  Start" );
			smssdk = gameObject.GetComponent<SMSSDK>();
			smssdk.init("moba6b6c6d6","b89d2427a3bc7ad1aea1e1e8c1d36bf3",true);
			userInfo = new UserInfo ();
			smssdk.setHandler(this);
		}

		// Update is called once per frame
		void Update () 
		{
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Application.Quit();
			}
		}


		void OnGUI ()
		{
			GUI.skin = demoSkin;

			float scale = 1.0f;
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				scale = Screen.width / 320;
			} else if (Application.platform == RuntimePlatform.Android) {
				if (Screen.orientation == ScreenOrientation.Portrait) {
					scale = Screen.width / 320;
				} else {
					scale = Screen.height / 320;
				}
			}
				
			float btnWidth = 200 * scale;
			float btnHeight = 30 * scale;
			float btnTop = 50 * scale;
			GUI.skin.button.fontSize = Convert.ToInt32(14 * scale);
			GUI.skin.label.fontSize = Convert.ToInt32 (14 * scale);
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.skin.textField.fontSize = Convert.ToInt32 (14 * scale);
			GUI.skin.textField.alignment = TextAnchor.MiddleCenter;

			float labelWidth = 60 * scale;
			GUI.Label (new Rect ((Screen.width - btnWidth) / 2, btnTop+5, labelWidth, btnHeight), "手机号");

			phone = GUI.TextField(new Rect((Screen.width - btnWidth) / 2 + labelWidth, btnTop, btnWidth - labelWidth, btnHeight), phone);

			btnTop += btnHeight + 10 * scale;

			GUI.Label (new Rect ((Screen.width - btnWidth) / 2, btnTop+5, labelWidth, btnHeight), "区号");

			zone = GUI.TextField(new Rect((Screen.width - btnWidth) / 2 + labelWidth, btnTop, btnWidth - labelWidth, btnHeight), zone);

			btnTop += btnHeight + 10 * scale;

			GUI.Label (new Rect ((Screen.width - btnWidth) / 2, btnTop+5, labelWidth, btnHeight), "验证码");

			code = GUI.TextField(new Rect((Screen.width - btnWidth) / 2 + labelWidth, btnTop, btnWidth - labelWidth, btnHeight), code);

			btnTop += btnHeight + 10 * scale;

			if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "GetCodeSMS"))
			{
				smssdk.getCode (CodeType.TextCode, phone, zone, tempCode);
			}

			btnTop += btnHeight + 10 * scale;
			if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "CommitCode"))
			{
                Debug.Log("提交验证码:" + phone + "," + "zone" + "," + code);
				smssdk.commitCode (phone, zone, code);
			}

			btnTop += btnHeight + 10 * scale;
			if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "GetCodeVoice"))
			{
				smssdk.getCode (CodeType.VoiceCode, phone, zone, tempCode);
			}
				
			btnTop += btnHeight + 10 * scale;
			if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "GetCountryCode"))
			{
				smssdk.getSupportedCountryCode ();
			}


			btnTop += btnHeight + 10 * scale;
			if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "GetFriends"))
			{

				smssdk.getFriends ();
			}

			btnTop += btnHeight + 10 * scale;
			if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "SubmitUserInfo"))
			{
				userInfo.avatar = "www.mob.com";
				userInfo.phoneNumber = phone;
				userInfo.zone = zone;
				userInfo.nickName = "David";
				userInfo.uid = "1234567890";
				smssdk.submitUserInfo (userInfo);
			}

			btnTop += btnHeight + 10 * scale;
			if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "GetVersionNumber"))
			{

				smssdk.getVersion ();
			}

			btnTop += btnHeight + 10 * scale;
			if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "EnableWarn"))
			{
				smssdk.enableWarn (true);
			}

			//展示register UI界面
			btnTop += btnHeight + 10 * scale;
			if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "showRegisterUIView"))
			{
				// 模板号可以为空
				smssdk.showRegisterPage (CodeType.TextCode, tempCode);
			}

			//展示contractFriends UI界面
			btnTop += btnHeight + 10 * scale;
			if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "showContractsUIView"))
			{
				smssdk.showContactsPage ();
			}
			//展示回调结果
			btnTop += btnHeight + 10 * scale;
			GUIStyle style=new GUIStyle();
			style.normal.textColor=new Color(1,0,0);   //字体颜色
			// style.fontSize = 30;
			style.fontSize = (int)(20 * scale);       //字体大小
			GUI.Label(new Rect(20, btnTop, Screen.width - 20 - 20, Screen.height - btnTop), result, style);
		}

		public void onComplete(int action, object resp)
		{
			ActionType act = (ActionType)action;
			if (resp != null)
			{
				result = resp.ToString();
			}
			if (act == ActionType.GetCode) {
				string responseString = (string)resp;
				Debug.Log ("isSmart :" + responseString);
			} else if (act == ActionType.GetVersion) {
				string version = (string)resp;
				Debug.Log ("version :" + version);
				print ("Demo*version*********" + version);

			} else if (act == ActionType.GetSupportedCountries) {

				string responseString = (string)resp;
				Debug.Log ("zoneString :" + responseString);

			} else if (act == ActionType.GetFriends) {
				string responseString = (string)resp;
				Debug.Log ("friendsString :" + responseString);

			} else if (act == ActionType.CommitCode) {

				string responseString = (string)resp;
				Debug.Log ("commitCodeString :" + responseString);

			} else if (act == ActionType.SubmitUserInfo) {

				string responseString = (string)resp;
				Debug.Log ("submitString :" + responseString);

			} else if (act == ActionType.ShowRegisterView) {

				string responseString = (string)resp;
				Debug.Log ("showRegisterView :" + responseString);

			} else if (act == ActionType.ShowContractFriendsView) {

				string responseString = (string)resp;
				Debug.Log ("showContractFriendsView :" + responseString);
			}
		}

		public void onError(int action, object resp)
		{
            ActionType act = (ActionType)action;
            Debug.Log("Smss error:" + act.ToString());
            Debug.Log("Error :" + resp);
			result = resp.ToString();
			print ("OnError ******resp"+resp);
		}
	}
}
