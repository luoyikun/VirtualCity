package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.REQ_GETUSERINFO)
public class ReqGetUserInfoMessage extends Message {
    @Protobuf(order = 1)
    private List<Long> accountId;

    public ReqGetUserInfoMessage() {
    }

    public List<Long> getAccountId() {
        return accountId;
    }

    public void setAccountId(List<Long> accountId) {
        this.accountId = accountId;
    }
}
