package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_SPEEDUP)
public class ReqSpeedUpMessage extends Message {

    /**建筑Id*/
    @Protobuf(order = 1)
    private String devId;
    /**玩家Id*/
    @Protobuf(order = 2)
    private Long playerId;
    /**玩家昵称*/
    @Protobuf(order = 3)
    private String playerName;

    public ReqSpeedUpMessage() {
    }

    public ReqSpeedUpMessage(String devId, Long playerId, String playerName) {
        this.devId = devId;
        this.playerId = playerId;
        this.playerName = playerName;
    }

    public String getDevId() {
        return devId;
    }

    public void setDevId(String devId) {
        this.devId = devId;
    }

    public Long getPlayerId() {
        return playerId;
    }

    public void setPlayerId(Long playerId) {
        this.playerId = playerId;
    }

    public String getPlayerName() {
        return playerName;
    }

    public void setPlayerName(String playerName) {
        this.playerName = playerName;
    }

}
