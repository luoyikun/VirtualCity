package com.kingston.jforgame.server.game.entity.master;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.db.BaseEntity;
import com.kingston.jforgame.orm.utils.DbName;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;


@Entity
@DbName(name = "proxool.master")
public class MoneyTree extends BaseEntity {


	@Id
/**
 ID
 */
	@Column
	@Protobuf(order = 1)
	private Long id;

	/**
	 用户Id
	 */
	@Column
	@Protobuf(order = 2)
	private long accountId;

	/**
	 模型数据ID
	 */
	@Column
	@Protobuf(order = 3)
	private long modelId;

	/**
	 开始收益时间
	 */
	@Column
	@Protobuf(order = 4)
	private String incomeTime;

	/**
	 是否开始收益
	 */
	@Column
	@Protobuf(order = 5)
	private int isIncome;

	/**
	 收益金额
	 */
	@Column
	@Protobuf(order = 6)
	private float income;

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

	public long getModelId() {
		return modelId;
	}

	public void setModelId(long modelId) {
		this.modelId = modelId;
	}

	public String getIncomeTime() {
		return incomeTime;
	}

	public void setIncomeTime(String incomeTime) {
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


