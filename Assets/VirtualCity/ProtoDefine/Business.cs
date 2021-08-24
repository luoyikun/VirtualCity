using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class Business {


[ProtoMember(1)]
public long? id;

/**
 商家名称
*/
[ProtoMember(2)]
public string name;

/**
 商家类别ID
*/
[ProtoMember(3)]
public long businessTypeId;

/**
 社会统一信用代码
*/
[ProtoMember(4)]
public string code;

/**
 商家信息描述
*/
[ProtoMember(5)]
public string info;

/**
 注册号码
*/
[ProtoMember(6)]
public string businessAccount;

/**
 密码
*/
[ProtoMember(7)]
public string password;

/**
 企业账户类型：“0”支付宝，“1”微信
*/
[ProtoMember(8)]
public int payType;

/**
 支付账号
*/
[ProtoMember(9)]
public string payAccount;

/**
 商家退款收货地址和电话格式:收货地址：xxx；电话：xxx
*/
[ProtoMember(10)]
public string refundInfo;

/**
 商家电话
*/
[ProtoMember(11)]
public string phone;

[ProtoMember(12)]
public string createtime;

[ProtoMember(13)]
public string updatetime;

public long? getId() {
	return id;
}

public void setId(long? id) {
	this.id = id;
}

public string getName() {
	return name;
}

public void setName(string name) {
	this.name = name;
}

public long getBusinessTypeId() {
	return businessTypeId;
}

public void setBusinessTypeId(long businessTypeId) {
	this.businessTypeId = businessTypeId;
}

public string getCode() {
	return code;
}

public void setCode(string code) {
	this.code = code;
}

public string getInfo() {
	return info;
}

public void setInfo(string info) {
	this.info = info;
}

public string getBusinessAccount() {
	return businessAccount;
}

public void setBusinessAccount(string businessAccount) {
	this.businessAccount = businessAccount;
}

public string getPassword() {
	return password;
}

public void setPassword(string password) {
	this.password = password;
}

public int getPayType() {
	return payType;
}

public void setPayType(int payType) {
	this.payType = payType;
}

public string getPayAccount() {
	return payAccount;
}

public void setPayAccount(string payAccount) {
	this.payAccount = payAccount;
}

public string getRefundInfo() {
	return refundInfo;
}

public void setRefundInfo(string refundInfo) {
	this.refundInfo = refundInfo;
}

public string getPhone() {
	return phone;
}

public void setPhone(string phone) {
	this.phone = phone;
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