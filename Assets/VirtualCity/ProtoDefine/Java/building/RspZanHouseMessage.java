package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_ZANHOUSE)
public class RspZanHouseMessage extends Message {
    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String tips;
    @Protobuf(order = 3)
    private long playerId;
    @Protobuf(order = 4)
    private String houseId;

    public RspZanHouseMessage(int code, String tips, long playerId, String houseId) {
        this.code = code;
        this.tips = tips;
        this.playerId = playerId;
        this.houseId = houseId;
    }

    public RspZanHouseMessage(int code, String tips) {
        this.code = code;
        this.tips = tips;
    }

    public RspZanHouseMessage() {
    }

    public String getHouseId() {
        return houseId;
    }

    public void setHouseId(String houseId) {
        this.houseId = houseId;
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

    public long getPlayerId() {
        return playerId;
    }

    public void setPlayerId(long playerId) {
        this.playerId = playerId;
    }
}
