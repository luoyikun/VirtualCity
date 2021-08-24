package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.PutStatus;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_PUTHOUSEPART)
public class ReqPutHousePartMessage extends Message {
    @Protobuf(order = 1)
    private String houseId;
    @Protobuf(order = 2,fieldType = FieldType.OBJECT)
    private PutStatus putStatus;

    public ReqPutHousePartMessage() {
    }

    public ReqPutHousePartMessage(String houseId, PutStatus putStatus) {
        this.houseId = houseId;
        this.putStatus = putStatus;
    }

    public String getHouseId() {
        return houseId;
    }

    public void setHouseId(String houseId) {
        this.houseId = houseId;
    }

    public PutStatus getPutStatus() {
        return putStatus;
    }

    public void setPutStatus(PutStatus putStatus) {
        this.putStatus = putStatus;
    }
}
