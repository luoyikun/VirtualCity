package com.kingston.jforgame.server.game.building.entity;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;

public class Land {
    @Protobuf(order = 1)
    private String code;
    //建筑类型：0没有建筑，611住宅，613开发区
    @Protobuf(order = 2)
    private int buildType;
    @Protobuf(order = 3)
    private String buildId;
    //模型id
    @Protobuf(order = 4)
    private Long Model;

    public Land(String code, int buildType, String buildId, Long model) {
        this.code = code;
        this.buildType = buildType;
        this.buildId = buildId;
        Model = model;
    }

    public Land() {
    }

    public String getCode() {
        return code;
    }

    public void setCode(String code) {
        this.code = code;
    }

    public int getBuildType() {
        return buildType;
    }

    public void setBuildType(int buildType) {
        this.buildType = buildType;
    }

    public String getBuildId() {
        return buildId;
    }

    public void setBuildId(String buildId) {
        this.buildId = buildId;
    }

    public Long getModel() {
        return Model;
    }

    public void setModel(Long model) {
        Model = model;
    }
}
