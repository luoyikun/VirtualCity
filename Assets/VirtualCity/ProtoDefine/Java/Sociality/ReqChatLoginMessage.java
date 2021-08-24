package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.server.game.sociality.entity.ChatUser;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.REQ_LOGIN)
public class ReqChatLoginMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private ChatUser chatUser;
    @Protobuf(order = 2,fieldType = FieldType.INT64)
    private List<Long> friendList;
    @Protobuf(order = 3,fieldType = FieldType.INT64)
    private List<Long> groupList;

    public ChatUser getChatUser() {
        return chatUser;
    }

    public ReqChatLoginMessage() {
    }

    public void setChatUser(ChatUser chatUser) {
        this.chatUser = chatUser;
    }

    public ReqChatLoginMessage(ChatUser chatUser) {
        this.chatUser = chatUser;
    }

    public List<Long> getFriendList() {
        return friendList;
    }

    public void setFriendList(List<Long> friendList) {
        this.friendList = friendList;
    }

    public List<Long> getGroupList() {
        return groupList;
    }

    public void setGroupList(List<Long> groupList) {
        this.groupList = groupList;
    }
}