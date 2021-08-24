package com.kingston.jforgame.server.game.login.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;

import com.kingston.jforgame.server.game.Modules;

import com.kingston.jforgame.server.game.login.LoginDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;


@MessageMeta(module=Modules.LOGIN, cmd=LoginDataPool.RSP_LOGIN)
public class RspLoginMessage extends Message {

	@Protobuf(order = 1)
	private int code;
	@Protobuf(order = 2)
	private String tips;
	@Protobuf(order = 3)
	private String serverIp;
	@Protobuf(order = 4)
	private Long accountId;
	/**
	 * 是否已经在线需要顶号
	 * 0，否
	 * 1，是
	 */
	@Protobuf(order = 5)
	private int isOnline;


	public RspLoginMessage() {
	}

	@Override
	public String toString() {
		return "RspLoginMessage{" +
				"code=" + code +
				", tips='" + tips + '\'' +
				", serverIp='" + serverIp + '\'' +
				", accountId=" + accountId +
				'}';
	}

	public RspLoginMessage(int code, String tips, String serverIp, Long accountId,int isOnline) {
		this.code = code;
		this.tips = tips;
		this.serverIp = serverIp;
		this.accountId = accountId;
		this.isOnline = isOnline;
	}

	public int getCode() {
		return code;
	}

	public void setCode(int code) {
		this.code = code;
	}

	public String getTips() {
		return tips;
	}

	public void setTips(String tips) {
		this.tips = tips;
	}

	public String getServerIp() {
		return serverIp;
	}

	public void setServerIp(String serverIp) {
		this.serverIp = serverIp;
	}

	public Long getAccountId() {
		return accountId;
	}

	public void setAccountId(Long accountId) {
		this.accountId = accountId;
	}

	public int getIsOnline() {
		return isOnline;
	}

	public void setIsOnline(int isOnline) {
		this.isOnline = isOnline;
	}
}
