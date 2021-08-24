using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class UserOtherData {



[ProtoMember(1)]
	public long? id;

	/**
	 用户ID
	 */
[ProtoMember(2)]
	public long accountId;

	/**
	 新手教程到了哪一步
	 */
[ProtoMember(3)]
	public int newStep;

	public long? getId() {
		return id;
	}

	public void setId(long? id) {
		this.id = id;
	}

	public long getAccountId() {
		return accountId;
	}

	public void setAccountId(long accountId) {
		this.accountId = accountId;
	}

	public int getNewStep() {
		return newStep;
	}

	public void setNewStep(int newStep) {
		this.newStep = newStep;
	}

}


}