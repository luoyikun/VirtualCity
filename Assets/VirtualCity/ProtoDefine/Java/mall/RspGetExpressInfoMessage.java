package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.mall.ExpressInfo;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.RSP_EXPRESSINFO)
public class RspGetExpressInfoMessage extends Message {
    //快递信息
    @Protobuf(order = 1)
    private ExpressInfo expressInfo;

    @Protobuf(order = 2)
    private int code;

    @Protobuf(order = 3)
    private String tip;

    public RspGetExpressInfoMessage() {
    }

    public RspGetExpressInfoMessage(ExpressInfo expressInfo, int code, String tip) {
        this.expressInfo = expressInfo;
        this.code = code;
        this.tip = tip;
    }

    public ExpressInfo getExpressInfo() {
        return expressInfo;
    }

    public void setExpressInfo(ExpressInfo expressInfo) {
        this.expressInfo = expressInfo;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getTip() {
        return tip;
    }

    public void setTip(String tip) {
        this.tip = tip;
    }
}
