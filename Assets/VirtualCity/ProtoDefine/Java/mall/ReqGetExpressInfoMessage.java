package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.REQ_EXPRESSINFO)
public class ReqGetExpressInfoMessage extends Message {
    //快递单号
    @Protobuf(order = 1)
    private String expressCode;

    public String getExpressCode() {
        return expressCode;
    }

    public void setExpressCode(String expressCode) {
        this.expressCode = expressCode;
    }
}
