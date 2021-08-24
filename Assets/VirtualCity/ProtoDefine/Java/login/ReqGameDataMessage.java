package com.kingston.jforgame.server.game.login.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.login.LoginDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.LOGIN,cmd = LoginDataPool.REQ_LOAD_DATE)
public class ReqGameDataMessage extends Message {
    @Protobuf(order = 1)
    private String clientVersion;

    public String getVersion() {
        return clientVersion;
    }

    public void setVersion(String version) {
        this.clientVersion = version;
    }
}
