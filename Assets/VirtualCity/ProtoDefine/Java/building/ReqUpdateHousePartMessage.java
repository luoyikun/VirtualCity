package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.PutStatus;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;
import java.util.Map;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_UPDATEHOUSEPART)
public class ReqUpdateHousePartMessage extends Message {
    @Protobuf(order = 1)
    private String houseId;
    @Protobuf(order = 2,fieldType = FieldType.MAP)
    private Map<String, PutStatus> putStatusMap;

    public String getHouseId() {
        return houseId;
    }

    public void setHouseId(String houseId) {
        this.houseId = houseId;
    }

    public Map<String, PutStatus> getPutStatusMap() {
        return putStatusMap;
    }

    public void setPutStatusMap(Map<String, PutStatus> putStatusMap) {
        this.putStatusMap = putStatusMap;
    }
}
