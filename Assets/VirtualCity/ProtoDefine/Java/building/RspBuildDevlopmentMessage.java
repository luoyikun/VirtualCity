package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.Devlopments;
import com.kingston.jforgame.server.game.building.entity.Land;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_BUILTEDEVLOPMENT)
public class RspBuildDevlopmentMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private Devlopments devlopment;
    @Protobuf(order = 2)
    private int gold;
    @Protobuf(order = 3)
    private int diament;
    @Protobuf(order = 4)
    private int code;
    @Protobuf(order = 5)
    private String tip;


    public RspBuildDevlopmentMessage() {
    }

    public RspBuildDevlopmentMessage(Devlopments devlopment, int gold, int diament, int code, String tip) {
        this.devlopment = devlopment;
        this.gold = gold;
        this.diament = diament;
        this.code = code;
        this.tip = tip;
    }

    public RspBuildDevlopmentMessage(int code, String tip) {
        this.code = code;
        this.tip = tip;
    }

    public Devlopments getDevlopment() {
        return devlopment;
    }

    public void setDevlopment(Devlopments devlopment) {
        this.devlopment = devlopment;
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
