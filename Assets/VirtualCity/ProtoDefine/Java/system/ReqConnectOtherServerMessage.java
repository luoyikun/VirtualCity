package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BASE,cmd = SystemDataPool.REQ_CONNECT)
public class ReqConnectOtherServerMessage  extends Message {
    @Protobuf(order = 1)
    private String connectInfo;
    @Protobuf(order = 2)
    private String fromConnectInfo;

    public ReqConnectOtherServerMessage() {
    }

    public ReqConnectOtherServerMessage(String fromConnectInfo,String connectInfo) {
        this.connectInfo = connectInfo;
        this.fromConnectInfo = fromConnectInfo;
    }

    public String getFromConnectInfo() {
        return fromConnectInfo;
    }

    public void setFromConnectInfo(String fromConnectInfo) {
        this.fromConnectInfo = fromConnectInfo;
    }

    public String getConnectInfo() {
        return connectInfo;
    }

    public void setConnectInfo(String connectInfo) {
        this.connectInfo = connectInfo;
    }
}
