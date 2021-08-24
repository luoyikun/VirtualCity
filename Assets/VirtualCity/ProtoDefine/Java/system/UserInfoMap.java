package com.kingston.jforgame.server.game.system.entity;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;

public class UserInfoMap {
    @Protobuf(order = 1)
    private int infoType;
    @Protobuf(order = 2)
    private String info;

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

    public UserInfoMap() {
    }

    public UserInfoMap(short infoType, String info) {
        this.infoType = infoType;
        this.info = info;
    }
}
