package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.PutStatus;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_REMOVEHOUSEPART)
public class ReqRemoveHousePartMessage extends Message {
    @Protobuf(order = 1)
    private String houseId;
    @Protobuf(order = 2,fieldType = FieldType.STRING)
    private List<String> putStatusId;

    public ReqRemoveHousePartMessage() {
    }

    public ReqRemoveHousePartMessage(String houseId, List<String> putStatusId) {
        this.houseId = houseId;
        this.putStatusId = putStatusId;
    }

    public List<String> getPutStatusId() {
        return putStatusId;
    }

    public void setPutStatusId(List<String> putStatusId) {
        this.putStatusId = putStatusId;
    }

    public String getHouseId() {
        return houseId;
    }

    public void setHouseId(String houseId) {
        this.houseId = houseId;
    }


}
