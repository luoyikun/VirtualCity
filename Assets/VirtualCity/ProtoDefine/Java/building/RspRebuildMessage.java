package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.Devlopments;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_REBUILD)
public class RspRebuildMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private Devlopments devlopments;
    @Protobuf(order = 2)
    private int gold;
    @Protobuf(order = 3)
    private int diamend;
    @Protobuf(order = 4)
    private int code;
    @Protobuf(order = 5)
    private String tip;

    public RspRebuildMessage() {
    }

    public RspRebuildMessage(int code, String tip) {
        this.code = code;
        this.tip = tip;
    }

    public RspRebuildMessage(Devlopments devlopments, int gold, int diamend, int code, String tip) {
        this.devlopments = devlopments;
        this.gold = gold;
        this.diamend = diamend;
        this.code = code;
        this.tip = tip;
    }

    public Devlopments getDevlopments() {
        return devlopments;
    }

    public void setDevlopments(Devlopments devlopments) {
        this.devlopments = devlopments;
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
