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

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_REMOVEHOUSEPART)
public class RspRemoveHousePartMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private List<HouseParts> parts;
    @Protobuf(order = 2)
    private int code;
    @Protobuf(order = 3)
    private String tip;



    public List<HouseParts> getParts() {
        return parts;
    }

    public void setParts(List<HouseParts> parts) {
        this.parts = parts;
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

    public RspRemoveHousePartMessage() {
    }

    public RspRemoveHousePartMessage(int code, String tip) {
        this.code = code;
        this.tip = tip;
    }

    public RspRemoveHousePartMessage(List<HouseParts> parts, int code, String tip) {
        this.parts = parts;

        this.code = code;
        this.tip = tip;
    }
}
