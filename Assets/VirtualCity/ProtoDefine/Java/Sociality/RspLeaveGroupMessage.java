package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.RSP_LEAVEGROUP)
public class RspLeaveGroupMessage extends Message {
    @Protobuf(order = 1)
    private long groupId;
    @Protobuf(order = 2)
    private long accounId;
    public RspLeaveGroupMessage() {
    }

    public RspLeaveGroupMessage(long groupId, long accounId) {
        this.groupId = groupId;
        this.accounId = accounId;
    }

    public long getAccounId() {
        return accounId;
    }

    public void setAccounId(long accounId) {
        this.accounId = accounId;
    }

    public long getGroupId() {
        return groupId;
    }

    public void setGroupId(long groupId) {
        this.groupId = groupId;
    }
}


