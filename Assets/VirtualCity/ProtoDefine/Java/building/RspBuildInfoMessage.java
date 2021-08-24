package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.Devlopments;
import com.kingston.jforgame.server.game.building.entity.House;
import com.kingston.jforgame.server.game.building.entity.HouseParts;
import com.kingston.jforgame.server.game.building.entity.Land;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;
import java.util.Map;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_GETHOMETONE)
public class RspBuildInfoMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.MAP)
    private Map<String,Land> landMap;
    @Protobuf(order = 2,fieldType = FieldType.MAP)
    private Map<String,Devlopments> devlopmentsMap;
    @Protobuf(order = 3,fieldType = FieldType.MAP)
    private Map<String,House> housesMap;
    @Protobuf(order = 4)
    private int code;
    @Protobuf(order = 5)
    private String tips;
    @Protobuf(order = 6,fieldType = FieldType.INT64)
    private long zan;


    public Map<String, Land> getLandMap() {
        return landMap;
    }

    public void setLandMap(Map<String, Land> landMap) {
        this.landMap = landMap;
    }

    public Map<String, Devlopments> getDevlopmentsMap() {
        return devlopmentsMap;
    }

    public void setDevlopmentsMap(Map<String, Devlopments> devlopmentsMap) {
        this.devlopmentsMap = devlopmentsMap;
    }

    public Map<String, House> getHousesMap() {
        return housesMap;
    }

    public void setHousesMap(Map<String, House> housesMap) {
        this.housesMap = housesMap;
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

    public long getZan() {
        return zan;
    }

    public void setZan(long zan) {
        this.zan = zan;
    }
}
