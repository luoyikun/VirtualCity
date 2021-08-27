using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class Account {




[ProtoMember(1)]
	public long? id;

	/**
	 用户名
	 */
[ProtoMember(2)]
	public string userName;

	/**
	 电话号码
	 */
[ProtoMember(3)]
	public string phone;

	/**
	 密码
	 */
[ProtoMember(4)]
	public string password;

	/**
	 收货信息
	 */
[ProtoMember(5)]
	public string addressInfo;

	/**
	 提现账号信息
	 */
[ProtoMember(6)]
	public string userPayAccount;

	/**
	 推荐码
	 */
[ProtoMember(7)]
	public string commendCode;

	/**
	 是否有代理权："0" 没有代理权；“1”有代理权
	 */
[ProtoMember(8)]
	public int hadProxy;

	/**
	 "0"失效；“1”生效
	 */
[ProtoMember(9)]
	public int aliveable;

	/**
	 用户所在服务器
	 */
[ProtoMember(10)]
	public string serverIp;

	/**
	 是否已登录：“0”未登录，“1”已登录
	 */
[ProtoMember(11)]
	public int isLogin;

	/**
	 登陆时间
	 */
[ProtoMember(12)]
	public string logoutTime;

	/**
	 退出时间
	 */
[ProtoMember(13)]
	public string loginTime;

	/**
	 购物车
	 */
[ProtoMember(14)]
	public string goodsList;

	/**
	 好友ID列表
	 */
[ProtoMember(15)]
	public string friendList;

	/**
	 群ID列表
	 */
[ProtoMember(16)]
	public string groupList;

	/**
	 模型id
	 */
[ProtoMember(17)]
	public long modleId = 1;

	/**
	 性别：0 ‘女’，1：'男'
	 */
[ProtoMember(18)]
	public int sex;

	/**
	 玩家仓库列表
	 */
[ProtoMember(19)]
	public string partList;

	/**
	 开发地
	 */
[ProtoMember(20)]
	public string devlopmentArea;

	/**
	 建筑
	 */
[ProtoMember(21)]
	public string devlopmentList;

	/**
	 房屋
	 */
[ProtoMember(22)]
	public string house;

	/**
	 上一次每日重置时间戳
	 */
[ProtoMember(23)]
	public long lastDailyReset;

[ProtoMember(24)]
	public string updatetime;

[ProtoMember(25)]
	public string createtime;

	public long? getId() {
		return id;
	}

	public void setId(long? id) {
		this.id = id;
	}

	public string getUserName() {
		return userName;
	}

	public void setUserName(string userName) {
		this.userName = userName;
	}

	public string getPhone() {
		return phone;
	}

	public void setPhone(string phone) {
		this.phone = phone;
	}

	public string getPassword() {
		return password;
	}

	public void setPassword(string password) {
		this.password = password;
	}

	public string getAddressInfo() {
		return addressInfo;
	}

	public void setAddressInfo(string addressInfo) {
		this.addressInfo = addressInfo;
	}

	public string getUserPayAccount() {
		return userPayAccount;
	}

	public void setUserPayAccount(string userPayAccount) {
		this.userPayAccount = userPayAccount;
	}

	public string getCommendCode() {
		return commendCode;
	}

	public void setCommendCode(string commendCode) {
		this.commendCode = commendCode;
	}

	public int getHadProxy() {
		return hadProxy;
	}

	public void setHadProxy(int hadProxy) {
		this.hadProxy = hadProxy;
	}

	public int getAliveable() {
		return aliveable;
	}

	public void setAliveable(int aliveable) {
		this.aliveable = aliveable;
	}

	public string getServerIp() {
		return serverIp;
	}

	public void setServerIp(string serverIp) {
		this.serverIp = serverIp;
	}

	public int getIsLogin() {
		return isLogin;
	}

	public void setIsLogin(int isLogin) {
		this.isLogin = isLogin;
	}

	public string getLogoutTime() {
		return logoutTime;
	}

	public void setLogoutTime(string logoutTime) {
		this.logoutTime = logoutTime;
	}

	public string getLoginTime() {
		return loginTime;
	}

	public void setLoginTime(string loginTime) {
		this.loginTime = loginTime;
	}

	public string getGoodsList() {
		return goodsList;
	}

	public void setGoodsList(string goodsList) {
		this.goodsList = goodsList;
	}

	public string getFriendList() {
		return friendList;
	}

	public void setFriendList(string friendList) {
		this.friendList = friendList;
	}

	public string getGroupList() {
		return groupList;
	}

	public void setGroupList(string groupList) {
		this.groupList = groupList;
	}

	public long getModleId() {
		return modleId;
	}

	public void setModleId(long modleId) {
		this.modleId = modleId;
	}

	public int getSex() {
		return sex;
	}

	public void setSex(int sex) {
		this.sex = sex;
	}

	public string getPartList() {
		return partList;
	}

	public void setPartList(string partList) {
		this.partList = partList;
	}

	public string getDevlopmentArea() {
		return devlopmentArea;
	}

	public void setDevlopmentArea(string devlopmentArea) {
		this.devlopmentArea = devlopmentArea;
	}

	public string getDevlopmentList() {
		return devlopmentList;
	}

	public void setDevlopmentList(string devlopmentList) {
		this.devlopmentList = devlopmentList;
	}

	public string getHouse() {
		return house;
	}

	public void setHouse(string house) {
		this.house = house;
	}

	public long getLastDailyReset() {
		return lastDailyReset;
	}

	public void setLastDailyReset(long lastDailyReset) {
		this.lastDailyReset = lastDailyReset;
	}

	public string getUpdatetime() {
		return updatetime;
	}

	public void setUpdatetime(string updatetime) {
		this.updatetime = updatetime;
	}

	public string getCreatetime() {
		return createtime;
	}

	public void setCreatetime(string createtime) {
		this.createtime = createtime;
	}

[ProtoMember(26)]
	public AccountWallet wallet;

	public AccountWallet getWallet() {
		return wallet;
	}

	public void setWallet(AccountWallet wallet) {
		this.wallet = wallet;
	}

}


}