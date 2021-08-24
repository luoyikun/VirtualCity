package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.House;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_GETHOUSEINFO)
public class RspSyncHouseMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private House house;
    @Protobuf(order = 2)
    private int code;
    @Protobuf(order = 3)
    private String tip;

    public RspSyncHouseMessage(int code, String tip) {
        this.code = code;
        this.tip = tip;
    }

    public House getHouse() {
        return house;
    }

    public void setHouse(House house) {
        this.house = house;
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

    public RspSyncHouseMessage() {
    }

    public RspSyncHouseMessage(House house, int code, String tip) {
        this.house = house;
        this.code = code;
        this.tip = tip;
    }
}
