package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.PlayerStatus;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_SYNCSTREET)
public class ReqSyncStreetMessage extends Message {

    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private PlayerStatus playerStatus;
    @Protobuf(order = 2)
    private String scenceId;

    public PlayerStatus getPlayerStatus() {
        return playerStatus;
    }

    public void setPlayerStatus(PlayerStatus playerStatus) {
        this.playerStatus = playerStatus;
    }

    public String getScenceId() {
        return scenceId;
    }

    public void setScenceId(String scenceId) {
        this.scenceId = scenceId;
    }

}