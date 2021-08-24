package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.RSP_DELETEFRIEND)
public class RspDeleteFriendMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private long accountId;
    @Protobuf(order = 2)
    private long fromAccountId;


    public RspDeleteFriendMessage() {
    }

    public RspDeleteFriendMessage(long accountId, long fromAccountId) {
        this.accountId = accountId;
        this.fromAccountId = fromAccountId;
    }

    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }

    public long getFromAccountId() {
        return fromAccountId;
    }

    public void setFromAccountId(long fromAccountId) {
        this.fromAccountId = fromAccountId;
    }
}