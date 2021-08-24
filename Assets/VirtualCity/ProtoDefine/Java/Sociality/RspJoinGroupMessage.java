package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.user.ChatGroup;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.RSP_JOINGROUP)
public class RspJoinGroupMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private ChatGroup group;
    @Protobuf(order = 2)
    private int code;
    @Protobuf(order = 3)
    private String tips;

    public RspJoinGroupMessage() {
    }

    public RspJoinGroupMessage(ChatGroup group, int code, String tips) {
        this.group = group;
        this.code = code;
        this.tips = tips;
    }

    public ChatGroup getGroup() {
        return group;
    }

    public void setGroup(ChatGroup group) {
        this.group = group;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getTips() {
        return tips;
    }

    public void setTips(String tips) {
        this.tips = tips;
    }
}
