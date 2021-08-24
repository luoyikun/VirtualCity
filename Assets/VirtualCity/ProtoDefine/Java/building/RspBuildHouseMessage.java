package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.Devlopments;
import com.kingston.jforgame.server.game.building.entity.House;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_BUILTEHOUSE)
public class RspBuildHouseMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private House house;
    @Protobuf(order = 2)
    private int gold;
    @Protobuf(order = 3)
    private int diament;
    @Protobuf(order = 4)
    private int code;
    @Protobuf(order = 5)
    private String tip;

    public RspBuildHouseMessage() {
    }

    public RspBuildHouseMessage(int code, String tip) {
        this.code = code;
        this.tip = tip;
    }

    public RspBuildHouseMessage(House house, int gold, int diament, int code, String tip) {
        this.house = house;
        this.gold = gold;
        this.diament = diament;
        this.code = code;
        this.tip = tip;
    }

    public House getHouse() {
        return house;
    }

    public void setHouse(House house) {
        this.house = house;
    }

    public int getGold() {
        return gold;
    }

    public void setGold(int gold) {
        this.gold = gold;
    }

    public int getDiament() {
        return diament;
    }

    public void setDiament(int diament) {
        this.diament = diament;
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
