package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.server.game.sociality.entity.ProxyUser;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.RSP_GETPROXYUSER)
public class RspGetProxyUserMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private List<ProxyUser> proxyUsers;

    public RspGetProxyUserMessage() {
    }

    public RspGetProxyUserMessage(List<ProxyUser> proxyUsers) {
        this.proxyUsers = proxyUsers;
    }

    public List<ProxyUser> getProxyUsers() {
        return proxyUsers;
    }

    public void setProxyUsers(List<ProxyUser> proxyUsers) {
        this.proxyUsers = proxyUsers;
    }
}
