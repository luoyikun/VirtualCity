package com.kingston.jforgame.server.game.entity.mall;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.db.BaseEntity;
import com.kingston.jforgame.orm.utils.DbName;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;


@Entity
@DbName(name = "proxool.mall")
public class Business extends BaseEntity {


@Id
@Column
@Protobuf(order = 1)
private Long id;

/**
 商家名称
*/
@Column
@Protobuf(order = 2)
private String name;

/**
 商家类别ID
*/
@Column
@Protobuf(order = 3)
private long businessTypeId;

/**
 社会统一信用代码
*/
@Column
@Protobuf(order = 4)
private String code;

/**
 商家信息描述
*/
@Column
@Protobuf(order = 5)
private String info;

/**
 注册号码
*/
@Column
@Protobuf(order = 6)
private String businessAccount;

/**
 密码
*/
@Column
@Protobuf(order = 7)
private String password;

/**
 企业账户类型：“0”支付宝，“1”微信
*/
@Column
@Protobuf(order = 8)
private int payType;

/**
 支付账号
*/
@Column
@Protobuf(order = 9)
private String payAccount;

/**
 商家退款收货地址和电话格式:收货地址：xxx；电话：xxx
*/
@Column
@Protobuf(order = 10)
private String refundInfo;

/**
 商家电话
*/
@Column
@Protobuf(order = 11)
private String phone;

@Column
@Protobuf(order = 12)
private String createtime;

@Column
@Protobuf(order = 13)
private String updatetime;

public Long getId() {
	return id;
}

public void setId(Long id) {
	this.id = id;
}

public String getName() {
	return name;
}

public void setName(String name) {
	this.name = name;
}

public long getBusinessTypeId() {
	return businessTypeId;
}

public void setBusinessTypeId(long businessTypeId) {
	this.businessTypeId = businessTypeId;
}

public String getCode() {
	return code;
}

public void setCode(String code) {
	this.code = code;
}

public String getInfo() {
	return info;
}

public void setInfo(String info) {
	this.info = info;
}

public String getBusinessAccount() {
	return businessAccount;
}

public void setBusinessAccount(String businessAccount) {
	this.businessAccount = businessAccount;
}

public String getPassword() {
	return password;
}

public void setPassword(String password) {
	this.password = password;
}

public int getPayType() {
	return payType;
}

public void setPayType(int payType) {
	this.payType = payType;
}

public String getPayAccount() {
	return payAccount;
}

public void setPayAccount(String payAccount) {
	this.payAccount = payAccount;
}

public String getRefundInfo() {
	return refundInfo;
}

public void setRefundInfo(String refundInfo) {
	this.refundInfo = refundInfo;
}

public String getPhone() {
	return phone;
}

public void setPhone(String phone) {
	this.phone = phone;
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


