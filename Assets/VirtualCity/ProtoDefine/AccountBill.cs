using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class AccountBill {



[ProtoMember(1)]
	public long? id;

	/**
	 账户id
	 */
[ProtoMember(2)]
	public long accountId;

	/**
	 账单号
	 */
[ProtoMember(3)]
	public string billNo;

	/**
	 收支类别：“0”收入 ，“1”支出，“2”提现
	 */
[ProtoMember(4)]
	public string budgetType;

	/**
	 账单类别：“0” 购买代理权，“1”购买商品，“2”系统奖励, "3"提现
	 */
[ProtoMember(5)]
	public string billType;

	/**
	 支付方式：“0”支付宝，“1”微信
	 */
[ProtoMember(6)]
	public int payType;

	/**
	 支付时间
	 */
[ProtoMember(7)]
	public string payTime;

	/**
	 支付凭证xml
	 */
[ProtoMember(8)]
	public string payInfo;

	/**
	 现金
	 */
[ProtoMember(9)]
	public double money;

	/**
	 购物金
	 */
[ProtoMember(10)]
	public double sMoney;

	/**
	 额外现金
	 */
[ProtoMember(11)]
	public double oMoney;

	/**
	 收入来源层级
	 */
[ProtoMember(12)]
	public int incomeCengji;

	/**
	 收入来源
	 */
[ProtoMember(13)]
	public long incomeSource;

	/**
	 是否已经与支付平台核对过
	 */
[ProtoMember(14)]
	public int isChecked;

[ProtoMember(15)]
	public string createtime;

[ProtoMember(16)]
	public string updatetime;

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

	public string getBillNo() {
		return billNo;
	}

	public void setBillNo(string billNo) {
		this.billNo = billNo;
	}

	public string getBudgetType() {
		return budgetType;
	}

	public void setBudgetType(string budgetType) {
		this.budgetType = budgetType;
	}

	public string getBillType() {
		return billType;
	}

	public void setBillType(string billType) {
		this.billType = billType;
	}

	public int getPayType() {
		return payType;
	}

	public void setPayType(int payType) {
		this.payType = payType;
	}

	public string getPayTime() {
		return payTime;
	}

	public void setPayTime(string payTime) {
		this.payTime = payTime;
	}

	public string getPayInfo() {
		return payInfo;
	}

	public void setPayInfo(string payInfo) {
		this.payInfo = payInfo;
	}

	public double getMoney() {
		return money;
	}

	public void setMoney(double money) {
		this.money = money;
	}

	public double getSMoney() {
		return sMoney;
	}

	public void setSMoney(double sMoney) {
		this.sMoney = sMoney;
	}

	public double getOMoney() {
		return oMoney;
	}

	public void setOMoney(double oMoney) {
		this.oMoney = oMoney;
	}

	public int getIncomeCengji() {
		return incomeCengji;
	}

	public void setIncomeCengji(int incomeCengji) {
		this.incomeCengji = incomeCengji;
	}

	public long getIncomeSource() {
		return incomeSource;
	}

	public void setIncomeSource(long incomeSource) {
		this.incomeSource = incomeSource;
	}

	public int getIsChecked() {
		return isChecked;
	}

	public void setIsChecked(int isChecked) {
		this.isChecked = isChecked;
	}

	public string getCreatetime() {
		return createtime;
	}

	public void setCreatetime(string createtime) {
		this.createtime = createtime;
	}

	public string getUpdatetime() {
		return updatetime;
	}

	public void setUpdatetime(string updatetime) {
		this.updatetime = updatetime;
	}

	public AccountBill() {
	}

}


}