package com.kingston.jforgame.server.game.activity.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.activity.ActivityDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.ACTIVITY,cmd = ActivityDataPool.RSP_REWARDMONEYTREE)
public class RspRewardMoneyTreeMessage extends Message {
    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String tips;

    public RspRewardMoneyTreeMessage() {
    }

    public RspRewardMoneyTreeMessage(int code, String tips) {
        this.code = code;
        this.tips = tips;
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
}
