package com.kingston.jforgame.server.game.entity.user;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.db.BaseEntity;
import com.kingston.jforgame.orm.utils.DbName;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;


@Entity
@DbName(name = "proxool.master")
public class UserOtherData extends BaseEntity {



	@Id
	@Column
	@Protobuf(order = 1)
	private Long id;

	/**
	 用户ID
	 */
	@Column
	@Protobuf(order = 2)
	private long accountId;

	/**
	 新手教程到了哪一步
	 */
	@Column
	@Protobuf(order = 3)
	private int newStep;

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

	public int getNewStep() {
		return newStep;
	}

	public void setNewStep(int newStep) {
		this.newStep = newStep;
	}

}


