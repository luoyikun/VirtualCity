package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.sociality.entity.ProxyUser;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.RSP_UPDATEPROXY)
public class RspUpdateProxyMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private ProxyUser proxyUser;
    @Protobuf(order = 2)
    private int proxyLeve;

    public RspUpdateProxyMessage() {
    }

    public RspUpdateProxyMessage(ProxyUser proxyUser, int proxyLeve) {
        this.proxyUser = proxyUser;
        this.proxyLeve = proxyLeve;
    }

    public int getProxyLeve() {
        return proxyLeve;
    }

    public void setProxyLeve(int proxyLeve) {
        this.proxyLeve = proxyLeve;
    }

    public ProxyUser getProxyUser() {
        return proxyUser;
    }

    public void setProxyUser(ProxyUser proxyUser) {
        this.proxyUser = proxyUser;
    }
}
