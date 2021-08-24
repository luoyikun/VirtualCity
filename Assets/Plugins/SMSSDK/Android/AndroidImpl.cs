using System;
using System.Collections;
using UnityEngine; 

namespace cn.SMSSDK.Unity
{
	#if UNITY_ANDROID
	public class AndroidImpl : SMSSDKInterface
	{
	private AndroidJavaObject smssdk;

	public AndroidImpl (GameObject go) 
	{
			Debug.Log("[SMSSDK]AndroidImpl  ===>>>  AndroidImpl" );
	try{
	smssdk = new AndroidJavaObject("cn.smssdk.unity3d.SMSSDKUtils", go.name, "_callBack");
	} catch(Exception e) {
	Console.WriteLine("{0} Exception caught.", e);
	}
	}

	public override void init(string appKey, string appsecret, bool isWarn)
	{
			Debug.Log("[SMSSDK]AndroidImpl ==>>> InitSDK ===" + appKey + ";" + appsecret);
	if(smssdk != null) {
	smssdk.Call("init", appKey,appsecret,isWarn);
	}
	}

		public override void getCode(CodeType type, string phoneNumber, string zone, string tempCode)
	{
			Debug.Log("[SMSSDK]AndroidImpl ==>>> getCode " + zone + ";" + phoneNumber + ";" + tempCode);
	if(smssdk != null) {
	if(type == CodeType.TextCode) {
	smssdk.Call("getTextCode", zone, phoneNumber, tempCode);
	} else if(type == CodeType.VoiceCode) {
	smssdk.Call("getVoiceCode", zone, phoneNumber);
	}
	}
	}


	public override void commitCode(string phoneNumber, string zone, string code)
	{
			Debug.Log("[SMSSDK]AndroidImpl ==>>> commitCode" + zone + ";" + phoneNumber + ";" + code);
	if(smssdk != null) {
	smssdk.Call("submitCode", zone,phoneNumber,code);
	}
	}

	public override void getSupportedCountry()
	{
			Debug.Log("[SMSSDK]AndroidImpl ==>>> getSupportedCountry ===");
	if(smssdk != null) {
	smssdk.Call("getSupportedCountry");
	}
	}

	public override void getFriends()
	{
			Debug.Log("[SMSSDK]AndroidImpl ==>>> getFriends");
	if(smssdk != null) {
	smssdk.Call("getFriendsInApp");
	}
	}

	public override void submitUserInfo(UserInfo userInfo)
	{
			Debug.Log("[SMSSDK]AndroidImpl ==>>> submitUserInfo ===");
	if(smssdk != null) {
	smssdk.Call("submitUserInfo", userInfo.uid,userInfo.nickName,userInfo.avatar,userInfo.zone,userInfo.phoneNumber);
	}
	}

	public override void getVersion()
	{
	Debug.Log("AndroidImpl ==>>> getVersion");
	if(smssdk != null) {
	smssdk.Call("getVersion");
	}
	}

	public override void enableWarn(bool isWarn)
	{
			Debug.Log("[SMSSDK]AndroidImpl ==>>> enableWarn");
	if(smssdk != null) {
	smssdk.Call("enableWarn", isWarn);
	}
	}

	}
	#endif
}
