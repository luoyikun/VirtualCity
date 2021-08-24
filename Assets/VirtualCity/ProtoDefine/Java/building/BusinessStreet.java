package com.kingston.jforgame.server.game.building.entity;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;

import java.util.Map;

public class BusinessStreet extends Scence {
    @Protobuf(order = 1,fieldType = FieldType.MAP)
    private  Map<Long,Scence> shops;

    public Map<Long, Scence> getShops() {
        return shops;
    }

    public void setShops(Map<Long, Scence> shops) {
        this.shops = shops;
    }
}
