package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.server.game.sociality.entity.ChatUser;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.RSP_ADDFRIEND)
public class RspAddFriendMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private ChatUser chatUser;
    @Protobuf(order = 2)
    private long fromAccountId;


    public RspAddFriendMessage() {
    }

    public RspAddFriendMessage(ChatUser chatUser, long fromAccountId) {
        this.chatUser = chatUser;
        this.fromAccountId = fromAccountId;
    }

    public ChatUser getChatUser() {
        return chatUser;
    }

    public void setChatUser(ChatUser chatUser) {
        this.chatUser = chatUser;
    }

    public long getFromAccountId() {
        return fromAccountId;
    }

    public void setFromAccountId(long fromAccountId) {
        this.fromAccountId = fromAccountId;
    }
}
