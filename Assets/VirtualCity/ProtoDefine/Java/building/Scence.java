package com.kingston.jforgame.server.game.building.entity;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;

import java.util.Map;

public  class Scence {
    @Protobuf(order = 91,fieldType = FieldType.MAP)
    private Map<Long, PlayerStatus> playerStatuses;
    @Protobuf(order = 92)
    private String scenceId;

    public Map<Long, PlayerStatus> getPlayerStatuses() {
        return playerStatuses;
    }

    public void setPlayerStatuses(Map<Long, PlayerStatus> playerStatuses) {
        this.playerStatuses = playerStatuses;
    }

    public String getScenceId() {
        return scenceId;
    }

    public void setScenceId(String scenceId) {
        this.scenceId = scenceId;
    }
}
