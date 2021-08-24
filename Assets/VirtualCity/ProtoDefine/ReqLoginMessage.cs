using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



/**
 * 请求－账号登录
 */
public class ReqLoginMessage {
	
	/** 账号ID */
[ProtoMember(1)]
	public long? accountId;
	/**手机号码*/
[ProtoMember(2)]
	public  string phone;
[ProtoMember(3)]
	public byte[] content;


	public long? getAccountId() {
		return accountId;
	}

	public void setAccountId(long? accounteId) {
		this.accountId = accounteId;
	}


	public string getPhone() {
		return phone;
	}

	public void setPhone(string phone) {
		this.phone = phone;
	}

	public byte[] getContent() {
		return content;
	}

	public void setContent(byte[] content) {
		this.content = content;
	}

	public string tostring() {
		return "ReqChatLoginMessage{" +
				"accountId=" + accountId +
				", phone='" + phone + '\'' +
				'}';
	}

}
}