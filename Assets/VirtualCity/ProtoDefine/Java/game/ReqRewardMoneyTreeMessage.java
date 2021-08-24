package com.kingston.jforgame.server.game.activity.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.activity.ActivityDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.ACTIVITY,cmd = ActivityDataPool.RSP_REWARDMONEYTREE)
public class ReqRewardMoneyTreeMessage extends Message {
    @Protobuf(order = 1)
    private long accountId;


    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }
}
