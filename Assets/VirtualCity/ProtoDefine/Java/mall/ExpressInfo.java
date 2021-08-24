package com.kingston.jforgame.server.game.entity.mall;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.db.BaseEntity;
import com.kingston.jforgame.orm.utils.DbName;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;


@Entity
@DbName(name = "proxool.mall")
public class ExpressInfo extends BaseEntity {


	@Id
	@Column
	@Protobuf(order = 1)
	private Long id;

	/**
	 订单号
	 */
	@Column
	@Protobuf(order = 2)
	private String orderNo;

	/**
	 快递公司编号
	 */
	@Column
	@Protobuf(order = 3)
	private String expressBusinessCode;

	/**
	 快递公司名称
	 */
	@Column
	@Protobuf(order = 4)
	private String expressBusinessName;

	/**
	 快递单号
	 */
	@Column
	@Protobuf(order = 5)
	private String expressCode;

	/**
	 快递状态：0-无轨迹，1-已揽收，2-在途中，3-签收,4-问题件
	 */
	@Column
	@Protobuf(order = 6)
	private int expressStatus;

	/**
	 预计送达时间
	 */
	@Column
	@Protobuf(order = 7)
	private String expectReachTime;

	/**
	 快递信息json：备注，时间，描述
	 */
	@Column
	@Protobuf(order = 8)
	private String expressMessage;

	@Column
	@Protobuf(order = 9)
	private String createtime;

	@Column
	@Protobuf(order = 10)
	private String updatetime;

	public Long getId() {
		return id;
	}

	public void setId(Long id) {
		this.id = id;
	}

	public String getOrderNo() {
		return orderNo;
	}

	public void setOrderNo(String orderNo) {
		this.orderNo = orderNo;
	}

	public String getExpressBusinessCode() {
		return expressBusinessCode;
	}

	public void setExpressBusinessCode(String expressBusinessCode) {
		this.expressBusinessCode = expressBusinessCode;
	}

	public String getExpressBusinessName() {
		return expressBusinessName;
	}

	public void setExpressBusinessName(String expressBusinessName) {
		this.expressBusinessName = expressBusinessName;
	}

	public String getExpressCode() {
		return expressCode;
	}

	public void setExpressCode(String expressCode) {
		this.expressCode = expressCode;
	}

	public int getExpressStatus() {
		return expressStatus;
	}

	public void setExpressStatus(int expressStatus) {
		this.expressStatus = expressStatus;
	}

	public String getExpectReachTime() {
		return expectReachTime;
	}

	public void setExpectReachTime(String expectReachTime) {
		this.expectReachTime = expectReachTime;
	}

	public String getExpressMessage() {
		return expressMessage;
	}

	public void setExpressMessage(String expressMessage) {
		this.expressMessage = expressMessage;
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

}


