using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]





public class RspLoginMessage {

[ProtoMember(1)]
	public int code;
[ProtoMember(2)]
	public string tips;
[ProtoMember(3)]
	public string serverIp;
[ProtoMember(4)]
	public long? accountId;
	/**
	 * 是否已经在线需要顶号
	 * 0，否
	 * 1，是
	 */
[ProtoMember(5)]
	public int isOnline;


	public RspLoginMessage() {
	}

	public string tostring() {
		return "RspLoginMessage{" +
				"code=" + code +
				", tips='" + tips + '\'' +
				", serverIp='" + serverIp + '\'' +
				", accountId=" + accountId +
				'}';
	}

	public RspLoginMessage(int code, string tips, string serverIp, long? accountId,int isOnline) {
		this.code = code;
		this.tips = tips;
		this.serverIp = serverIp;
		this.accountId = accountId;
		this.isOnline = isOnline;
	}

	public int getCode() {
		return code;
	}

	public void setCode(int code) {
		this.code = code;
	}

	public string getTips() {
		return tips;
	}

	public void setTips(string tips) {
		this.tips = tips;
	}

	public string getServerIp() {
		return serverIp;
	}

	public void setServerIp(string serverIp) {
		this.serverIp = serverIp;
	}

	public long? getAccountId() {
		return accountId;
	}

	public void setAccountId(long? accountId) {
		this.accountId = accountId;
	}

	public int getIsOnline() {
		return isOnline;
	}

	public void setIsOnline(int isOnline) {
		this.isOnline = isOnline;
	}
}
}