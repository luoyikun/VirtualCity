package com.kingston.jforgame.server.game.mall.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.mall.MallDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SHOPPING,cmd = MallDataPool.RSP_MANAGEORDER)
public class RspManageOrderMessage extends Message {
    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String tips;

    public RspManageOrderMessage(int code, String tips) {
        this.code = code;
        this.tips = tips;
    }

    public RspManageOrderMessage() {
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
