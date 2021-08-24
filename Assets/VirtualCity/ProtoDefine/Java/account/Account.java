package com.kingston.jforgame.server.game.entity.master;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.db.BaseEntity;
import com.kingston.jforgame.orm.utils.DbName;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;


@Entity
@DbName(name = "proxool.master")
public class Account extends BaseEntity {




	@Id
	@Column
	@Protobuf(order = 1)
	private Long id;

	/**
	 用户名
	 */
	@Column
	@Protobuf(order = 2)
	private String userName;

	/**
	 电话号码
	 */
	@Column
	@Protobuf(order = 3)
	private String phone;

	/**
	 密码
	 */
	@Column
	@Protobuf(order = 4)
	private String password;

	/**
	 收货信息
	 */
	@Column
	@Protobuf(order = 5)
	private String addressInfo;

	/**
	 提现账号信息
	 */
	@Column
	@Protobuf(order = 6)
	private String userPayAccount;

	/**
	 推荐码
	 */
	@Column
	@Protobuf(order = 7)
	private String commendCode;

	/**
	 是否有代理权："0" 没有代理权；“1”有代理权
	 */
	@Column
	@Protobuf(order = 8)
	private int hadProxy;

	/**
	 "0"失效；“1”生效
	 */
	@Column
	@Protobuf(order = 9)
	private int aliveable;

	/**
	 用户所在服务器
	 */
	@Column
	@Protobuf(order = 10)
	private String serverIp;

	/**
	 是否已登录：“0”未登录，“1”已登录
	 */
	@Column
	@Protobuf(order = 11)
	private int isLogin;

	/**
	 登陆时间
	 */
	@Column
	@Protobuf(order = 12)
	private String logoutTime;

	/**
	 退出时间
	 */
	@Column
	@Protobuf(order = 13)
	private String loginTime;

	/**
	 购物车
	 */
	@Column
	@Protobuf(order = 14)
	private String goodsList;

	/**
	 好友ID列表
	 */
	@Column
	@Protobuf(order = 15)
	private String friendList;

	/**
	 群ID列表
	 */
	@Column
	@Protobuf(order = 16)
	private String groupList;

	/**
	 模型id
	 */
	@Column
	@Protobuf(order = 17)
	private long modleId;

	/**
	 性别：0 ‘女’，1：'男'
	 */
	@Column
	@Protobuf(order = 18)
	private int sex;

	/**
	 玩家仓库列表
	 */
	@Column
	@Protobuf(order = 19)
	private String partList;

	/**
	 开发地
	 */
	@Column
	@Protobuf(order = 20)
	private String devlopmentArea;

	/**
	 建筑
	 */
	@Column
	@Protobuf(order = 21)
	private String devlopmentList;

	/**
	 房屋
	 */
	@Column
	@Protobuf(order = 22)
	private String house;

	/**
	 上一次每日重置时间戳
	 */
	@Column
	@Protobuf(order = 23)
	private long lastDailyReset;

	@Column
	@Protobuf(order = 24)
	private String updatetime;

	@Column
	@Protobuf(order = 25)
	private String createtime;

	public Long getId() {
		return id;
	}

	public void setId(Long id) {
		this.id = id;
	}

	public String getUserName() {
		return userName;
	}

	public void setUserName(String userName) {
		this.userName = userName;
	}

	public String getPhone() {
		return phone;
	}

	public void setPhone(String phone) {
		this.phone = phone;
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

	public String getAddressInfo() {
		return addressInfo;
	}

	public void setAddressInfo(String addressInfo) {
		this.addressInfo = addressInfo;
	}

	public String getUserPayAccount() {
		return userPayAccount;
	}

	public void setUserPayAccount(String userPayAccount) {
		this.userPayAccount = userPayAccount;
	}

	public String getCommendCode() {
		return commendCode;
	}

	public void setCommendCode(String commendCode) {
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

	public String getServerIp() {
		return serverIp;
	}

	public void setServerIp(String serverIp) {
		this.serverIp = serverIp;
	}

	public int getIsLogin() {
		return isLogin;
	}

	public void setIsLogin(int isLogin) {
		this.isLogin = isLogin;
	}

	public String getLogoutTime() {
		return logoutTime;
	}

	public void setLogoutTime(String logoutTime) {
		this.logoutTime = logoutTime;
	}

	public String getLoginTime() {
		return loginTime;
	}

	public void setLoginTime(String loginTime) {
		this.loginTime = loginTime;
	}

	public String getGoodsList() {
		return goodsList;
	}

	public void setGoodsList(String goodsList) {
		this.goodsList = goodsList;
	}

	public String getFriendList() {
		return friendList;
	}

	public void setFriendList(String friendList) {
		this.friendList = friendList;
	}

	public String getGroupList() {
		return groupList;
	}

	public void setGroupList(String groupList) {
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

	public String getPartList() {
		return partList;
	}

	public void setPartList(String partList) {
		this.partList = partList;
	}

	public String getDevlopmentArea() {
		return devlopmentArea;
	}

	public void setDevlopmentArea(String devlopmentArea) {
		this.devlopmentArea = devlopmentArea;
	}

	public String getDevlopmentList() {
		return devlopmentList;
	}

	public void setDevlopmentList(String devlopmentList) {
		this.devlopmentList = devlopmentList;
	}

	public String getHouse() {
		return house;
	}

	public void setHouse(String house) {
		this.house = house;
	}

	public long getLastDailyReset() {
		return lastDailyReset;
	}

	public void setLastDailyReset(long lastDailyReset) {
		this.lastDailyReset = lastDailyReset;
	}

	public String getUpdatetime() {
		return updatetime;
	}

	public void setUpdatetime(String updatetime) {
		this.updatetime = updatetime;
	}

	public String getCreatetime() {
		return createtime;
	}

	public void setCreatetime(String createtime) {
		this.createtime = createtime;
	}

	@Protobuf(order = 27)
	private AccountWallet wallet;

	public AccountWallet getWallet() {
		return wallet;
	}

	public void setWallet(AccountWallet wallet) {
		this.wallet = wallet;
	}

}


