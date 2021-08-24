package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.Devlopments;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_GETREWARD)
public class RspGetRewardMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private Devlopments devlopment;
    @Protobuf(order = 2)
    private int gold;
    @Protobuf(order = 3)
    private int code;
    @Protobuf(order = 4)
    private String tips;

    public RspGetRewardMessage() {
    }

    public RspGetRewardMessage(Devlopments devlopment, int gold, int code, String tips) {
        this.devlopment = devlopment;
        this.gold = gold;
        this.code = code;
        this.tips = tips;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getTips() {
        return tips;
    }

    public void setTips(String tips) {
        this.tips = tips;
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
}
