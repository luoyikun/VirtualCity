package com.kingston.jforgame.server.game.entity.bill;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.db.BaseEntity;
import com.kingston.jforgame.orm.utils.DbName;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;


@Entity
@DbName(name = "proxool.bill")
public class AccountBill extends BaseEntity {



	@Id
	@Column
	@Protobuf(order = 1)
	private Long id;

	/**
	 账户id
	 */
	@Column
	@Protobuf(order = 2)
	private long accountId;

	/**
	 账单号
	 */
	@Column
	@Protobuf(order = 3)
	private String billNo;

	/**
	 收支类别：“0”收入 ，“1”支出，“2”提现
	 */
	@Column
	@Protobuf(order = 4)
	private String budgetType;

	/**
	 账单类别：“0” 购买代理权，“1”购买商品，“2”系统奖励, "3"提现
	 */
	@Column
	@Protobuf(order = 5)
	private String billType;

	/**
	 支付方式：“0”支付宝，“1”微信
	 */
	@Column
	@Protobuf(order = 6)
	private int payType;

	/**
	 支付时间
	 */
	@Column
	@Protobuf(order = 7)
	private String payTime;

	/**
	 支付凭证xml
	 */
	@Column
	@Protobuf(order = 8)
	private String payInfo;

	/**
	 现金
	 */
	@Column
	@Protobuf(order = 9)
	private double money;

	/**
	 购物金
	 */
	@Column
	@Protobuf(order = 10)
	private double sMoney;

	/**
	 额外现金
	 */
	@Column
	@Protobuf(order = 11)
	private double oMoney;

	/**
	 收入来源层级
	 */
	@Column
	@Protobuf(order = 12)
	private int incomeCengji;

	/**
	 收入来源
	 */
	@Column
	@Protobuf(order = 13)
	private long incomeSource;

	/**
	 是否已经与支付平台核对过
	 */
	@Column
	@Protobuf(order = 14)
	private int isChecked;

	@Column
	@Protobuf(order = 15)
	private String createtime;

	@Column
	@Protobuf(order = 16)
	private String updatetime;

	public Long getId() {
		return id;
	}

	public void setId(Long id) {
		this.id = id;
	}

	public long getAccountId() {
		return accountId;
	}

	public void setAccountId(long accountId) {
		this.accountId = accountId;
	}

	public String getBillNo() {
		return billNo;
	}

	public void setBillNo(String billNo) {
		this.billNo = billNo;
	}

	public String getBudgetType() {
		return budgetType;
	}

	public void setBudgetType(String budgetType) {
		this.budgetType = budgetType;
	}

	public String getBillType() {
		return billType;
	}

	public void setBillType(String billType) {
		this.billType = billType;
	}

	public int getPayType() {
		return payType;
	}

	public void setPayType(int payType) {
		this.payType = payType;
	}

	public String getPayTime() {
		return payTime;
	}

	public void setPayTime(String payTime) {
		this.payTime = payTime;
	}

	public String getPayInfo() {
		return payInfo;
	}

	public void setPayInfo(String payInfo) {
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

	public String getCreatetime() {
		return createtime;
	}

	public void setCreatetime(String createtime) {
		this.createtime = createtime;
	}

	public String getUpdatetime() {
		return updatetime;
	}

	public void setUpdatetime(String updatetime) {
		this.updatetime = updatetime;
	}

	public AccountBill() {
	}

}


