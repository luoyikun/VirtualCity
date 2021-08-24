using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class MoneyTree {


/**
 ID
 */
[ProtoMember(1)]
	public long? id;

	/**
	 用户Id
	 */
[ProtoMember(2)]
	public long accountId;

	/**
	 模型数据ID
	 */
[ProtoMember(3)]
	public long modelId;

	/**
	 开始收益时间
	 */
[ProtoMember(4)]
	public string incomeTime;

	/**
	 是否开始收益
	 */
[ProtoMember(5)]
	public int isIncome;

	/**
	 收益金额
	 */
[ProtoMember(6)]
	public float income;

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

	public long getModelId() {
		return modelId;
	}

	public void setModelId(long modelId) {
		this.modelId = modelId;
	}

	public string getIncomeTime() {
		return incomeTime;
	}

	public void setIncomeTime(string incomeTime) {
		this.incomeTime = incomeTime;
	}

	public int getIsIncome() {
		return isIncome;
	}

	public void setIsIncome(int isIncome) {
		this.isIncome = isIncome;
	}

	public float getIncome() {
		return income;
	}

	public void setIncome(float income) {
		this.income = income;
	}

}


}