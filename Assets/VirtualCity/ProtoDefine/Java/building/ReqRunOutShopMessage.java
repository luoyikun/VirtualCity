package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.PlayerStatus;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_RUNOUTSHOP)
public class ReqRunOutShopMessage extends Message {

    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private PlayerStatus playerStatus;
    @Protobuf(order = 2)
    private String streetScenceId;
    @Protobuf(order = 3)
    private long shopId;

    public PlayerStatus getPlayerStatus() {
        return playerStatus;
    }

    public void setPlayerStatus(PlayerStatus playerStatus) {
        this.playerStatus = playerStatus;
    }

    public String getStreetScenceId() {
        return streetScenceId;
    }

    public void setStreetScenceId(String streetScenceId) {
        this.streetScenceId = streetScenceId;
    }

    public long getShopId() {
        return shopId;
    }

    public void setShopId(long shopId) {
        this.shopId = shopId;
    }
}