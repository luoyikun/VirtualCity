package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.HouseParts;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_GETHOUSEPART)
public class RspGetHousePartMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private List<HouseParts> housePartsList;

    public RspGetHousePartMessage() {
    }

    public RspGetHousePartMessage(List<HouseParts> housePartsList) {
        this.housePartsList = housePartsList;
    }

    public List<HouseParts> getHousePartsList() {
        return housePartsList;
    }

    public void setHousePartsList(List<HouseParts> housePartsList) {
        this.housePartsList = housePartsList;
    }
}
