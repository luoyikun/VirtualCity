package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BASE,cmd = SystemDataPool.RSP_CONNECT)
public class RspConnectOtherServerMessage extends Message {
    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String serverName;


    public RspConnectOtherServerMessage() {
    }


    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public RspConnectOtherServerMessage(int code, String serverName) {
        this.code = code;
        this.serverName = serverName;
    }

    public String getServerName() {
        return serverName;
    }

    public void setServerName(String serverName) {
        this.serverName = serverName;
    }
}
