package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.user.ChatGroup;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.RSP_CREATEGROUP)
public class RspCreateGroupMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private ChatGroup group;

    public RspCreateGroupMessage() {
    }

    public RspCreateGroupMessage(ChatGroup group) {
        this.group = group;
    }

    public ChatGroup getGroup() {
        return group;
    }

    public void setGroup(ChatGroup group) {
        this.group = group;
    }
}
