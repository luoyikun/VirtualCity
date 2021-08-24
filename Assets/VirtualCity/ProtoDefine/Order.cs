using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class Order {



[ProtoMember(1)]
	public long? id;

	/**
	 账户id
	 */
[ProtoMember(2)]
	public long accountId;

	/**
	 商家名称
	 */
[ProtoMember(3)]
	public string businessName;

	/**
	 商家id
	 */
[ProtoMember(4)]
	public long businessId;

	/**
	 订单号
	 */
[ProtoMember(5)]
	public string no;

	/**
	 商品id
	 */
[ProtoMember(6)]
	public long goodsId;

	/**
	 商品款式名称
	 */
[ProtoMember(7)]
	public string goodsKindname;

	/**
	 商品款式id
	 */
[ProtoMember(8)]
	public long goodsKindId;

	/**
	 商品url
	 */
[ProtoMember(9)]
	public string goodsKindUrl;

	/**
	 商品数量
	 */
[ProtoMember(10)]
	public int goodsNum;

	/**
	 客户支付状态：“0”未支付，“1”已支付，“2”支付异常
	 */
[ProtoMember(11)]
	public string payStatus;

	/**
	 支付方式：“0”支付宝，“1”微信
	 */
[ProtoMember(12)]
	public int payType;

	/**
	 支付时间
	 */
[ProtoMember(13)]
	public string payTime;

	/**
	 微信支付宝流水号
	 */
[ProtoMember(14)]
	public string payInfo;

	/**
	 总金额（*提现金额）
	 */
[ProtoMember(15)]
	public double payNum;

	/**
	 使用游戏内现金
	 */
[ProtoMember(16)]
	public double payMoney;

	/**
	 使用购物金
	 */
[ProtoMember(17)]
	public double paySMoney;

	/**
	 收货信息
	 */
[ProtoMember(18)]
	public string expressInfo;

	/**
	 收货人电话（商户可按照收货电话判断是否在合并发货）
	 */
[ProtoMember(19)]
	public string customerTel;

	/**
	 商家是否发货
	 */
[ProtoMember(20)]
	public int hasSend;

	/**
	 商家发货时间
	 */
[ProtoMember(21)]
	public string sendTime;

	/**
	 快递单号
	 */
[ProtoMember(22)]
	public string expressCode;

	/**
	 是否已收货
	 */
[ProtoMember(23)]
	public int hasGet;

	/**
	 订单状态：0“已下单待付款”，1“已付款待发货”，2“已发货待签收”，3“已签收待确认”，4“确认收货待评论”，5“退货申请中待审批”，6“退款审批完成”，7“客户已发货待商家确认”，8“商家确认收货待退款”，9“已退款”，10 “申请提现待审核”，11 “申请提现审核完成”12 "已评论"
	 */
[ProtoMember(24)]
	public string orderStatus;

	/**
	 是否申请退货
	 */
[ProtoMember(25)]
	public int isRefund;

	/**
	 申请退货原因
	 */
[ProtoMember(26)]
	public string refundReason;

	/**
	 退款数量
	 */
[ProtoMember(27)]
	public int refundNum;

	/**
	 退款金额
	 */
[ProtoMember(28)]
	public double refundMoney;

	/**
	 GM审批结果 0“不通过”，1“通过”
	 */
[ProtoMember(29)]
	public int gmApproveResault;

	/**
	 GM批准时间
	 */
[ProtoMember(30)]
	public string gmApproveTime;

	/**
	 审批不通过原因
	 */
[ProtoMember(31)]
	public string gmApproveReason;

	/**
	 商家退货信息
	 */
[ProtoMember(32)]
	public string refundExpressInfo;

	/**
	 退货物流号码
	 */
[ProtoMember(33)]
	public string refundExpressCode;

	/**
	 退货发货时间
	 */
[ProtoMember(34)]
	public string refundSendTime;

	/**
	 商家是否收到退货
	 */
[ProtoMember(35)]
	public int hasBusinessGet;

	/**
	 是否已退款
	 */
[ProtoMember(36)]
	public int hasRefund;

	/**
	 微信支付宝流水号
	 */
[ProtoMember(37)]
	public string refundPayInfo;

	/**
	 是否已打货款给商家
	 */
[ProtoMember(38)]
	public int hasPayBusiness;

	/**
	 备注（*提现账户信息、*退货退款的说明）
	 */
[ProtoMember(39)]
	public string remark;

[ProtoMember(40)]
	public string createtime;

[ProtoMember(41)]
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

	public string getBusinessName() {
		return businessName;
	}

	public void setBusinessName(string businessName) {
		this.businessName = businessName;
	}

	public long getBusinessId() {
		return businessId;
	}

	public void setBusinessId(long businessId) {
		this.businessId = businessId;
	}

	public string getNo() {
		return no;
	}

	public void setNo(string no) {
		this.no = no;
	}

	public long getGoodsId() {
		return goodsId;
	}

	public void setGoodsId(long goodsId) {
		this.goodsId = goodsId;
	}

	public string getGoodsKindname() {
		return goodsKindname;
	}

	public void setGoodsKindname(string goodsKindname) {
		this.goodsKindname = goodsKindname;
	}

	public long getGoodsKindId() {
		return goodsKindId;
	}

	public void setGoodsKindId(long goodsKindId) {
		this.goodsKindId = goodsKindId;
	}

	public string getGoodsKindUrl() {
		return goodsKindUrl;
	}

	public void setGoodsKindUrl(string goodsKindUrl) {
		this.goodsKindUrl = goodsKindUrl;
	}

	public int getGoodsNum() {
		return goodsNum;
	}

	public void setGoodsNum(int goodsNum) {
		this.goodsNum = goodsNum;
	}

	public string getPayStatus() {
		return payStatus;
	}

	public void setPayStatus(string payStatus) {
		this.payStatus = payStatus;
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

	public double getPayNum() {
		return payNum;
	}

	public void setPayNum(double payNum) {
		this.payNum = payNum;
	}

	public double getPayMoney() {
		return payMoney;
	}

	public void setPayMoney(double payMoney) {
		this.payMoney = payMoney;
	}

	public double getPaySMoney() {
		return paySMoney;
	}

	public void setPaySMoney(double paySMoney) {
		this.paySMoney = paySMoney;
	}

	public string getExpressInfo() {
		return expressInfo;
	}

	public void setExpressInfo(string expressInfo) {
		this.expressInfo = expressInfo;
	}

	public string getCustomerTel() {
		return customerTel;
	}

	public void setCustomerTel(string customerTel) {
		this.customerTel = customerTel;
	}

	public int getHasSend() {
		return hasSend;
	}

	public void setHasSend(int hasSend) {
		this.hasSend = hasSend;
	}

	public string getSendTime() {
		return sendTime;
	}

	public void setSendTime(string sendTime) {
		this.sendTime = sendTime;
	}

	public string getExpressCode() {
		return expressCode;
	}

	public void setExpressCode(string expressCode) {
		this.expressCode = expressCode;
	}

	public int getHasGet() {
		return hasGet;
	}

	public void setHasGet(int hasGet) {
		this.hasGet = hasGet;
	}

	public string getOrderStatus() {
		return orderStatus;
	}

	public void setOrderStatus(string orderStatus) {
		this.orderStatus = orderStatus;
	}

	public int getIsRefund() {
		return isRefund;
	}

	public void setIsRefund(int isRefund) {
		this.isRefund = isRefund;
	}

	public string getRefundReason() {
		return refundReason;
	}

	public void setRefundReason(string refundReason) {
		this.refundReason = refundReason;
	}

	public int getRefundNum() {
		return refundNum;
	}

	public void setRefundNum(int refundNum) {
		this.refundNum = refundNum;
	}

	public double getRefundMoney() {
		return refundMoney;
	}

	public void setRefundMoney(double refundMoney) {
		this.refundMoney = refundMoney;
	}

	public int getGmApproveResault() {
		return gmApproveResault;
	}

	public void setGmApproveResault(int gmApproveResault) {
		this.gmApproveResault = gmApproveResault;
	}

	public string getGmApproveTime() {
		return gmApproveTime;
	}

	public void setGmApproveTime(string gmApproveTime) {
		this.gmApproveTime = gmApproveTime;
	}

	public string getGmApproveReason() {
		return gmApproveReason;
	}

	public void setGmApproveReason(string gmApproveReason) {
		this.gmApproveReason = gmApproveReason;
	}

	public string getRefundExpressInfo() {
		return refundExpressInfo;
	}

	public void setRefundExpressInfo(string refundExpressInfo) {
		this.refundExpressInfo = refundExpressInfo;
	}

	public string getRefundExpressCode() {
		return refundExpressCode;
	}

	public void setRefundExpressCode(string refundExpressCode) {
		this.refundExpressCode = refundExpressCode;
	}

	public string getRefundSendTime() {
		return refundSendTime;
	}

	public void setRefundSendTime(string refundSendTime) {
		this.refundSendTime = refundSendTime;
	}

	public int getHasBusinessGet() {
		return hasBusinessGet;
	}

	public void setHasBusinessGet(int hasBusinessGet) {
		this.hasBusinessGet = hasBusinessGet;
	}

	public int getHasRefund() {
		return hasRefund;
	}

	public void setHasRefund(int hasRefund) {
		this.hasRefund = hasRefund;
	}

	public string getRefundPayInfo() {
		return refundPayInfo;
	}

	public void setRefundPayInfo(string refundPayInfo) {
		this.refundPayInfo = refundPayInfo;
	}

	public int getHasPayBusiness() {
		return hasPayBusiness;
	}

	public void setHasPayBusiness(int hasPayBusiness) {
		this.hasPayBusiness = hasPayBusiness;
	}

	public string getRemark() {
		return remark;
	}

	public void setRemark(string remark) {
		this.remark = remark;
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