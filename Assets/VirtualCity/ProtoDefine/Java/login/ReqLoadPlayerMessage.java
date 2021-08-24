package com.kingston.jforgame.server.game.login.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.master.Account;
import com.kingston.jforgame.server.game.login.LoginDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.LOGIN,cmd = LoginDataPool.REQ_LOAD_PLAYER)
public class ReqLoadPlayerMessage extends Message {
    /**
     * 用户对象
     */
    @Protobuf(order = 1)
    private long playerId;

    public long getPlayerId() {
        return playerId;
    }

    public void setPlayerId(long playerId) {
        this.playerId = playerId;
    }
}
