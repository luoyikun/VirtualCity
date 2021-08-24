package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_REMOVEDEVLOPMENT)
public class ReqRemoveDevMessage extends Message {
    @Protobuf(order = 1)
    private String devId;

    public ReqRemoveDevMessage() {
    }

    public ReqRemoveDevMessage(String devId) {
        this.devId = devId;
    }

    public String getDevId() {
        return devId;
    }

    public void setDevId(String devId) {
        this.devId = devId;
    }
}