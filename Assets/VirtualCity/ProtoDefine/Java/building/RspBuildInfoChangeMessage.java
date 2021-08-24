package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;


@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_BUILTEHOUSE)
public class RspBuildInfoChangeMessage extends Message {
    /**消息类型，房屋数据，建筑数据，房屋部件数据和土地数据*/
    @Protobuf(order = 1)
    private int infoType;
    @Protobuf(order = 2)
    /**消息的json*/
    private String info;
    /**金币*/
    @Protobuf(order = 3)
    private int gold;
    /**钻石*/
    @Protobuf(order = 4)
    private int diamond;
    /**成功*/
    @Protobuf(order = 5)
    private int code;
    /**提示*/
    @Protobuf(order = 6)
    private String tip;

    public RspBuildInfoChangeMessage() {
    }



    public int getInfoType() {
        return infoType;
    }

    public void setInfoType(int infoType) {
        this.infoType = infoType;
    }

    public String getInfo() {
        return info;
    }

    public void setInfo(String info) {
        this.info = info;
    }

    public int getGold() {
        return gold;
    }

    public void setGold(int gold) {
        this.gold = gold;
    }

    public int getDiamond() {
        return diamond;
    }

    public void setDiamond(int diamond) {
        this.diamond = diamond;
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

    public RspBuildInfoChangeMessage(int infoType, String info, int gold, int diamond, int code, String tip) {
        this.infoType = infoType;
        this.info = info;
        this.gold = gold;
        this.diamond = diamond;
        this.code = code;
        this.tip = tip;
    }

    @Override
    public String toString() {
        return "RspBuildInfoChangeMessage{" +
                "infoType=" + infoType +
                ", info='" + info + '\'' +
                ", gold=" + gold +
                ", diamond=" + diamond +
                ", code=" + code +
                ", tip='" + tip + '\'' +
                '}';
    }
}