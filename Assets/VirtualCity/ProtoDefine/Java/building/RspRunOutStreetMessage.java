package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_RUNOUTSTREET)
public class RspRunOutStreetMessage extends Message {
    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String tip;

    public RspRunOutStreetMessage() {
    }

    public RspRunOutStreetMessage(int code, String tip) {
        this.code = code;
        this.tip = tip;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getTip() {
        return tip;
    }

    public void setTip(String tip) {
        this.tip = tip;
    }
}
