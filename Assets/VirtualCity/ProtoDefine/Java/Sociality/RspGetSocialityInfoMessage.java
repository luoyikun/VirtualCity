package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.entity.Land;
import com.kingston.jforgame.server.game.entity.user.ChatGroup;
import com.kingston.jforgame.server.game.entity.user.SystemNotify;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.server.game.sociality.entity.ChatUser;
import com.kingston.jforgame.server.game.sociality.entity.ProxyUser;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.RSP_GETFRIENDINFO)
public class RspGetSocialityInfoMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private List<ChatUser> friendList;
    @Protobuf(order = 2,fieldType = FieldType.OBJECT)
    private List<ChatGroup> inChatGroup;
    @Protobuf(order = 3,fieldType = FieldType.OBJECT)
    private List<SystemNotify> systemNotifies;
    @Protobuf(order = 4,fieldType = FieldType.OBJECT)
    private List<ProxyUser> proxyUsers;

    public RspGetSocialityInfoMessage() {
    }

    public RspGetSocialityInfoMessage(List<ChatUser> friendList, List<ChatGroup> inChatGroup, List<SystemNotify> systemNotifies, List<ProxyUser> proxyUsers) {
        this.friendList = friendList;
        this.inChatGroup = inChatGroup;
        this.systemNotifies = systemNotifies;
        this.proxyUsers = proxyUsers;
    }

    public List<ProxyUser> getProxyUsers() {
        return proxyUsers;
    }

    public void setProxyUsers(List<ProxyUser> proxyUsers) {
        this.proxyUsers = proxyUsers;
    }

    public List<ChatUser> getFriendList() {
        return friendList;
    }

    public void setFriendList(List<ChatUser> friendList) {
        this.friendList = friendList;
    }

    public List<ChatGroup> getInChatGroup() {
        return inChatGroup;
    }

    public void setInChatGroup(List<ChatGroup> inChatGroup) {
        this.inChatGroup = inChatGroup;
    }

    public List<SystemNotify> getSystemNotifies() {
        return systemNotifies;
    }

    public void setSystemNotifies(List<SystemNotify> systemNotifies) {
        this.systemNotifies = systemNotifies;
    }
}
