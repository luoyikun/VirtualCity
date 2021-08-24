package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_RUNOUTPLAYER)
public class RspRunOutScenceMessage extends Message {
    @Protobuf(order = 1)
    private long playerId;

    public RspRunOutScenceMessage() {
    }

    public RspRunOutScenceMessage(long playerId) {
        this.playerId = playerId;
    }

    public long getPlayerId() {
        return playerId;
    }

    public void setPlayerId(long playerId) {
        this.playerId = playerId;
    }
}
