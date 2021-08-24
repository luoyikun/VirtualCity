package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.House;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_BUYHOUSEPART)
public class RspBuyHousePartMessage extends Message {
    @Protobuf(order = 1)
    private int gold;
    @Protobuf(order = 2)
    private int diament;
    @Protobuf(order = 3)
    private int code;
    @Protobuf(order = 4)
    private String tip;

    public RspBuyHousePartMessage() {
    }

    public RspBuyHousePartMessage(int code, String tip) {
        this.code = code;
        this.tip = tip;
    }

    public RspBuyHousePartMessage(int gold, int diament, int code, String tip) {
        this.gold = gold;
        this.diament = diament;
        this.code = code;
        this.tip = tip;
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
