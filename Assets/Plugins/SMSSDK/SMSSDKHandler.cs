using System.Collections;

namespace cn.SMSSDK.Unity
{
	public interface SMSSDKHandler
	{
		void onComplete(int action, object resp);
		void onError(int action, object resp);
	}
}
