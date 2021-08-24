package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.RSP_CREATEPAYINFO)
public class RspCreatePayInfoMessage extends Message {
    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String tip;
    @Protobuf(order = 3)
    private String payInfo;

    public RspCreatePayInfoMessage() {
    }

    public RspCreatePayInfoMessage(int code, String tip, String payInfo) {
        this.code = code;
        this.tip = tip;
        this.payInfo = payInfo;
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

    public String getPayInfo() {
        return payInfo;
    }

    public void setPayInfo(String payInfo) {
        this.payInfo = payInfo;
    }
}
