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

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_PUTHOUSEPART)
public class RspPutHousePartMessage extends Message {
    @Protobuf(order = 1)
    private String putStatusId;
    @Protobuf(order = 2)
    private int gold;
    @Protobuf(order = 3)
    private int diamend;
    @Protobuf(order = 4)
    private int code;
    @Protobuf(order = 5)
    private String tip;

    public RspPutHousePartMessage() {
    }

    public RspPutHousePartMessage(int code, String tip) {
        this.code = code;
        this.tip = tip;
    }

    public RspPutHousePartMessage( String putStatusId, int gold, int diamend, int code, String tip) {
        this.putStatusId = putStatusId;
        this.gold = gold;
        this.diamend = diamend;
        this.code = code;
        this.tip = tip;
    }

    public String getPutStatusId() {
        return putStatusId;
    }

    public void setPutStatusId(String putStatusId) {
        this.putStatusId = putStatusId;
    }

    public int getGold() {
        return gold;
    }

    public void setGold(int gold) {
        this.gold = gold;
    }

    public int getDiamend() {
        return diamend;
    }

    public void setDiamend(int diamend) {
        this.diamend = diamend;
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
