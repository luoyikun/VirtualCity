package com.kingston.jforgame.server.game.login.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.login.LoginDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.LOGIN,cmd = LoginDataPool.RSP_CONNECTION)
public class RspGetConnectionMessage extends Message {
    /**
     * 用户对象
     */
    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 3)
    private String tip;

    public RspGetConnectionMessage() {
    }

    public RspGetConnectionMessage(int code, String tip) {
        this.code = code;
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
