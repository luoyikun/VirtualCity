package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.RSP_FRINEDLOGIN)
public class RspFriendLoginMessage extends Message {
    @Protobuf(order = 1)
    private long accountId;
    /**
     * 0：下线
     * 1：上线
     */
    @Protobuf(order = 2)
    private int online;

    @Protobuf(order = 3)

    private long friendId;

    public RspFriendLoginMessage() {
    }

    public RspFriendLoginMessage(long accountId, int online, long friendId) {
        this.accountId = accountId;
        this.online = online;
        this.friendId = friendId;
    }

    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }

    public int getOnline() {
        return online;
    }

    public void setOnline(int online) {
        this.online = online;
    }

    public long getFriendId() {
        return friendId;
    }

    public void setFriendId(long friendId) {
        this.friendId = friendId;
    }
}
