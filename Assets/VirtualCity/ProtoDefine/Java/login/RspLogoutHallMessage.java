package com.kingston.jforgame.server.game.login.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.login.LoginDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module= Modules.LOGIN, cmd= LoginDataPool.RSP_LOGINOUT)
public class RspLogoutHallMessage extends Message {
    @Protobuf(order = 1)
    private  Long accountId;

    public RspLogoutHallMessage() {
    }

    public RspLogoutHallMessage(Long accountId) {
        this.accountId = accountId;
    }

    public Long getAccountId() {
        return accountId;
    }

    public void setAccountId(Long accountId) {
        this.accountId = accountId;
    }
}
