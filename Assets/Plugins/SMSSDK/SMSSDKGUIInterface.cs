namespace cn.SMSSDK.Unity
{
	public abstract class SMSSDKGUIInterface
	{
		/// <summsary>
		/// initialize SMSSDK
		/// </summary>
		public abstract void init(string appKey, string appsecret, bool isWarn);

		/// <summary>
		/// Shows the register page.
		/// </summary>
		public abstract void showRegisterPage(CodeType getCodeMethodType, string tempCode);

		/// <summary>
		/// Shows the contacts page.
		/// </summary>
		public abstract void showContactsPage();

	}
}
