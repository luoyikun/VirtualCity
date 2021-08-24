package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_GETFISH)
public class RspGetFishMessage extends Message {
    @Protobuf(order = 1)
    private long fishId;

    public long getFishId() {
        return fishId;
    }

    public void setFishId(long fishId) {
        this.fishId = fishId;
    }

    public RspGetFishMessage() {
    }

    public RspGetFishMessage(long fishId) {
        this.fishId = fishId;
    }
}
