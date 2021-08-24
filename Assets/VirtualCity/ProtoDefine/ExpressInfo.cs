using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ExpressInfo {


[ProtoMember(1)]
	public long? id;

	/**
	 订单号
	 */
[ProtoMember(2)]
	public string orderNo;

	/**
	 快递公司编号
	 */
[ProtoMember(3)]
	public string expressBusinessCode;

	/**
	 快递公司名称
	 */
[ProtoMember(4)]
	public string expressBusinessName;

	/**
	 快递单号
	 */
[ProtoMember(5)]
	public string expressCode;

	/**
	 快递状态：0-无轨迹，1-已揽收，2-在途中，3-签收,4-问题件
	 */
[ProtoMember(6)]
	public int expressStatus;

	/**
	 预计送达时间
	 */
[ProtoMember(7)]
	public string expectReachTime;

	/**
	 快递信息json：备注，时间，描述
	 */
[ProtoMember(8)]
	public string expressMessage;

[ProtoMember(9)]
	public string createtime;

[ProtoMember(10)]
	public string updatetime;

	public long? getId() {
		return id;
	}

	public void setId(long? id) {
		this.id = id;
	}

	public string getOrderNo() {
		return orderNo;
	}

	public void setOrderNo(string orderNo) {
		this.orderNo = orderNo;
	}

	public string getExpressBusinessCode() {
		return expressBusinessCode;
	}

	public void setExpressBusinessCode(string expressBusinessCode) {
		this.expressBusinessCode = expressBusinessCode;
	}

	public string getExpressBusinessName() {
		return expressBusinessName;
	}

	public void setExpressBusinessName(string expressBusinessName) {
		this.expressBusinessName = expressBusinessName;
	}

	public string getExpressCode() {
		return expressCode;
	}

	public void setExpressCode(string expressCode) {
		this.expressCode = expressCode;
	}

	public int getExpressStatus() {
		return expressStatus;
	}

	public void setExpressStatus(int expressStatus) {
		this.expressStatus = expressStatus;
	}

	public string getExpectReachTime() {
		return expectReachTime;
	}

	public void setExpectReachTime(string expectReachTime) {
		this.expectReachTime = expectReachTime;
	}

	public string getExpressMessage() {
		return expressMessage;
	}

	public void setExpressMessage(string expressMessage) {
		this.expressMessage = expressMessage;
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

}


}