package com.kingston.jforgame.server.game.entity.mall;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.db.BaseEntity;
import com.kingston.jforgame.orm.utils.DbName;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;


@Entity
@DbName(name = "proxool.mall")
public class Order extends BaseEntity {



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
	 商家名称
	 */
	@Column
	@Protobuf(order = 3)
	private String businessName;

	/**
	 商家id
	 */
	@Column
	@Protobuf(order = 4)
	private long businessId;

	/**
	 订单号
	 */
	@Column
	@Protobuf(order = 5)
	private String no;

	/**
	 商品id
	 */
	@Column
	@Protobuf(order = 6)
	private long goodsId;

	/**
	 商品款式名称
	 */
	@Column
	@Protobuf(order = 7)
	private String goodsKindname;

	/**
	 商品款式id
	 */
	@Column
	@Protobuf(order = 8)
	private long goodsKindId;

	/**
	 商品url
	 */
	@Column
	@Protobuf(order = 9)
	private String goodsKindUrl;

	/**
	 商品数量
	 */
	@Column
	@Protobuf(order = 10)
	private int goodsNum;

	/**
	 客户支付状态：“0”未支付，“1”已支付，“2”支付异常
	 */
	@Column
	@Protobuf(order = 11)
	private String payStatus;

	/**
	 支付方式：“0”支付宝，“1”微信
	 */
	@Column
	@Protobuf(order = 12)
	private int payType;

	/**
	 支付时间
	 */
	@Column
	@Protobuf(order = 13)
	private String payTime;

	/**
	 微信支付宝流水号
	 */
	@Column
	@Protobuf(order = 14)
	private String payInfo;

	/**
	 总金额（*提现金额）
	 */
	@Column
	@Protobuf(order = 15)
	private double payNum;

	/**
	 使用游戏内现金
	 */
	@Column
	@Protobuf(order = 16)
	private double payMoney;

	/**
	 使用购物金
	 */
	@Column
	@Protobuf(order = 17)
	private double paySMoney;

	/**
	 收货信息
	 */
	@Column
	@Protobuf(order = 18)
	private String expressInfo;

	/**
	 收货人电话（商户可按照收货电话判断是否在合并发货）
	 */
	@Column
	@Protobuf(order = 19)
	private String customerTel;

	/**
	 商家是否发货
	 */
	@Column
	@Protobuf(order = 20)
	private int hasSend;

	/**
	 商家发货时间
	 */
	@Column
	@Protobuf(order = 21)
	private String sendTime;

	/**
	 快递单号
	 */
	@Column
	@Protobuf(order = 22)
	private String expressCode;

	/**
	 是否已收货
	 */
	@Column
	@Protobuf(order = 23)
	private int hasGet;

	/**
	 订单状态：0“已下单待付款”，1“已付款待发货”，2“已发货待签收”，3“已签收待确认”，4“确认收货待评论”，5“退货申请中待审批”，6“退款审批完成”，7“客户已发货待商家确认”，8“商家确认收货待退款”，9“已退款”，10 “申请提现待审核”，11 “申请提现审核完成”12 "已评论"
	 */
	@Column
	@Protobuf(order = 24)
	private String orderStatus;

	/**
	 是否申请退货
	 */
	@Column
	@Protobuf(order = 25)
	private int isRefund;

	/**
	 申请退货原因
	 */
	@Column
	@Protobuf(order = 26)
	private String refundReason;

	/**
	 退款数量
	 */
	@Column
	@Protobuf(order = 27)
	private int refundNum;

	/**
	 退款金额
	 */
	@Column
	@Protobuf(order = 28)
	private double refundMoney;

	/**
	 GM审批结果 0“不通过”，1“通过”
	 */
	@Column
	@Protobuf(order = 29)
	private int gmApproveResault;

	/**
	 GM批准时间
	 */
	@Column
	@Protobuf(order = 30)
	private String gmApproveTime;

	/**
	 审批不通过原因
	 */
	@Column
	@Protobuf(order = 31)
	private String gmApproveReason;

	/**
	 商家退货信息
	 */
	@Column
	@Protobuf(order = 32)
	private String refundExpressInfo;

	/**
	 退货物流号码
	 */
	@Column
	@Protobuf(order = 33)
	private String refundExpressCode;

	/**
	 退货发货时间
	 */
	@Column
	@Protobuf(order = 34)
	private String refundSendTime;

	/**
	 商家是否收到退货
	 */
	@Column
	@Protobuf(order = 35)
	private int hasBusinessGet;

	/**
	 是否已退款
	 */
	@Column
	@Protobuf(order = 36)
	private int hasRefund;

	/**
	 微信支付宝流水号
	 */
	@Column
	@Protobuf(order = 37)
	private String refundPayInfo;

	/**
	 是否已打货款给商家
	 */
	@Column
	@Protobuf(order = 38)
	private int hasPayBusiness;

	/**
	 备注（*提现账户信息、*退货退款的说明）
	 */
	@Column
	@Protobuf(order = 39)
	private String remark;

	@Column
	@Protobuf(order = 40)
	private String createtime;

	@Column
	@Protobuf(order = 41)
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

	public String getBusinessName() {
		return businessName;
	}

	public void setBusinessName(String businessName) {
		this.businessName = businessName;
	}

	public long getBusinessId() {
		return businessId;
	}

	public void setBusinessId(long businessId) {
		this.businessId = businessId;
	}

	public String getNo() {
		return no;
	}

	public void setNo(String no) {
		this.no = no;
	}

	public long getGoodsId() {
		return goodsId;
	}

	public void setGoodsId(long goodsId) {
		this.goodsId = goodsId;
	}

	public String getGoodsKindname() {
		return goodsKindname;
	}

	public void setGoodsKindname(String goodsKindname) {
		this.goodsKindname = goodsKindname;
	}

	public long getGoodsKindId() {
		return goodsKindId;
	}

	public void setGoodsKindId(long goodsKindId) {
		this.goodsKindId = goodsKindId;
	}

	public String getGoodsKindUrl() {
		return goodsKindUrl;
	}

	public void setGoodsKindUrl(String goodsKindUrl) {
		this.goodsKindUrl = goodsKindUrl;
	}

	public int getGoodsNum() {
		return goodsNum;
	}

	public void setGoodsNum(int goodsNum) {
		this.goodsNum = goodsNum;
	}

	public String getPayStatus() {
		return payStatus;
	}

	public void setPayStatus(String payStatus) {
		this.payStatus = payStatus;
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

	public String getExpressInfo() {
		return expressInfo;
	}

	public void setExpressInfo(String expressInfo) {
		this.expressInfo = expressInfo;
	}

	public String getCustomerTel() {
		return customerTel;
	}

	public void setCustomerTel(String customerTel) {
		this.customerTel = customerTel;
	}

	public int getHasSend() {
		return hasSend;
	}

	public void setHasSend(int hasSend) {
		this.hasSend = hasSend;
	}

	public String getSendTime() {
		return sendTime;
	}

	public void setSendTime(String sendTime) {
		this.sendTime = sendTime;
	}

	public String getExpressCode() {
		return expressCode;
	}

	public void setExpressCode(String expressCode) {
		this.expressCode = expressCode;
	}

	public int getHasGet() {
		return hasGet;
	}

	public void setHasGet(int hasGet) {
		this.hasGet = hasGet;
	}

	public String getOrderStatus() {
		return orderStatus;
	}

	public void setOrderStatus(String orderStatus) {
		this.orderStatus = orderStatus;
	}

	public int getIsRefund() {
		return isRefund;
	}

	public void setIsRefund(int isRefund) {
		this.isRefund = isRefund;
	}

	public String getRefundReason() {
		return refundReason;
	}

	public void setRefundReason(String refundReason) {
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

	public String getGmApproveTime() {
		return gmApproveTime;
	}

	public void setGmApproveTime(String gmApproveTime) {
		this.gmApproveTime = gmApproveTime;
	}

	public String getGmApproveReason() {
		return gmApproveReason;
	}

	public void setGmApproveReason(String gmApproveReason) {
		this.gmApproveReason = gmApproveReason;
	}

	public String getRefundExpressInfo() {
		return refundExpressInfo;
	}

	public void setRefundExpressInfo(String refundExpressInfo) {
		this.refundExpressInfo = refundExpressInfo;
	}

	public String getRefundExpressCode() {
		return refundExpressCode;
	}

	public void setRefundExpressCode(String refundExpressCode) {
		this.refundExpressCode = refundExpressCode;
	}

	public String getRefundSendTime() {
		return refundSendTime;
	}

	public void setRefundSendTime(String refundSendTime) {
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

	public String getRefundPayInfo() {
		return refundPayInfo;
	}

	public void setRefundPayInfo(String refundPayInfo) {
		this.refundPayInfo = refundPayInfo;
	}

	public int getHasPayBusiness() {
		return hasPayBusiness;
	}

	public void setHasPayBusiness(int hasPayBusiness) {
		this.hasPayBusiness = hasPayBusiness;
	}

	public String getRemark() {
		return remark;
	}

	public void setRemark(String remark) {
		this.remark = remark;
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


