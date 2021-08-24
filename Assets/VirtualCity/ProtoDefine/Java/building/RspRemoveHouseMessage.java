package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.House;
import com.kingston.jforgame.server.game.building.entity.HouseParts;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_REMOVEHOUSE)
public class RspRemoveHouseMessage extends Message {

    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private House house;

    public RspRemoveHouseMessage() {
    }

    public RspRemoveHouseMessage(House house) {
        this.house = house;
    }

    public House getHouse() {
        return house;
    }

    public void setHouse(House house) {
        this.house = house;
    }

}
