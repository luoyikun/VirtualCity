package com.kingston.jforgame.server.game.login.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;

import com.kingston.jforgame.server.game.login.LoginDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;


@MessageMeta(module = Modules.LOGIN,cmd = LoginDataPool.RSP_HEARTBATE)
public class RspHeartBeatMessage extends Message {
    @Protobuf(order = 1)
    private int code;

    public RspHeartBeatMessage(int code) {
        this.code = code;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }


}
