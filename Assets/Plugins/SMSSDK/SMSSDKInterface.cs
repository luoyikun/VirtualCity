using System;

namespace cn.SMSSDK.Unity
{

	public enum CodeType
	{
		TextCode = 0,
		VoiceCode = 1,
	}

	public enum ActionType
	{
		GetCode = 1,
		CommitCode = 2,
		GetSupportedCountries = 3,
		SubmitUserInfo = 4,
		GetFriends = 5,
		GetVersion = 6,
		ShowRegisterView = 7,
		ShowContractFriendsView = 8,
	}

	public abstract class SMSSDKInterface
	{
		/// <summary>
		/// Init the specified appKey, appSerect and isWarn.
		/// </summary>
		/// <param name="appKey">App key.</param>
		/// <param name="appSerect">App serect.</param>
		/// <param name="isWarn">If set to <c>true</c> is warn.</param>
		public abstract void init (string appKey, string appSerect, bool isWarn);


		/// <summary>
		/// Gets the code.
		/// </summary>
		/// <param name="getCodeMethod">Get code method.</param>
		/// <param name="phoneNumber">Phone number.</param>
		/// <param name="zone">Zone.</param>
		public abstract void getCode (CodeType getCodeMethod, string phoneNumber, string zone, string tempCode);


		/// <summary>
		/// Commits the code.
		/// </summary>
		/// <param name="phoneNumber">Phone number.</param>
		/// <param name="zone">Zone.</param>
		/// <param name="verificationCode">Verification code.</param>
		public abstract void commitCode (string phoneNumber, string zone, string verificationCode);



		/// <summary>
		/// Gets the supported country.
		/// </summary>
		public abstract void getSupportedCountry ();



		/// <summary>
		/// Gets the friends.
		/// </summary>
		public abstract void getFriends ();


		/// <summary>
		/// Submits the user info.
		/// </summary>
		/// <param name="userInfo">User info.</param>
		public abstract void submitUserInfo (UserInfo userInfo);



		/// <summary>
		/// Gets the version.
		/// </summary>
		public abstract void getVersion ();

		/// <summary>
		/// Enables the warn.
		/// </summary>
		/// <param name="state">If set to <c>true</c> state.</param>
		public abstract void enableWarn (bool state);
	}
}