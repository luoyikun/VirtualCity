package com.kingston.jforgame.server.game.activity.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.activity.ActivityDataPool;
import com.kingston.jforgame.server.game.entity.master.MoneyTree;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.ACTIVITY,cmd = ActivityDataPool.RSP_GETMONEYTREE)
public class RspGetMoneyTreeMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private MoneyTree moneyTree;

    public RspGetMoneyTreeMessage() {
    }

    public RspGetMoneyTreeMessage(MoneyTree moneyTree) {
        this.moneyTree = moneyTree;
    }

    public MoneyTree getMoneyTree() {
        return moneyTree;
    }

    public void setMoneyTree(MoneyTree moneyTree) {
        this.moneyTree = moneyTree;
    }
}
