package com.kingston.jforgame.server.game.login.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.master.Account;
import com.kingston.jforgame.server.game.login.LoginDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.Objects;

/**
 * 请求－账号登录
 * @author kingston
 */
@MessageMeta(module=Modules.LOGIN, cmd=LoginDataPool.REQ_LOGIN)
public class ReqLoginMessage extends Message {
	
	/** 账号ID */
	@Protobuf(order = 1)
	private Long accountId;
	/**手机号码*/
	@Protobuf(order = 2)
	private  String phone;
	@Protobuf(order = 3)
	private byte[] content;


	public Long getAccountId() {
		return accountId;
	}

	public void setAccountId(Long accounteId) {
		this.accountId = accounteId;
	}


	public String getPhone() {
		return phone;
	}

	public void setPhone(String phone) {
		this.phone = phone;
	}

	public byte[] getContent() {
		return content;
	}

	public void setContent(byte[] content) {
		this.content = content;
	}

	@Override
	public String toString() {
		return "ReqChatLoginMessage{" +
				"accountId=" + accountId +
				", phone='" + phone + '\'' +
				'}';
	}

}
