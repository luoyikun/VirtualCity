package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.RSP_SENDMESSAGE)
public class RspSendMessage extends Message {
    @Protobuf(order = 1)
    private int code = SocialityDataPool.FAIL;
    @Protobuf(order = 2)
    private String tip;

    public RspSendMessage() {
    }

    public RspSendMessage(String tip) {
        this.tip = tip;
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
