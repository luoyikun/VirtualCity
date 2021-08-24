package com.kingston.jforgame.server.game.building.entity;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;

import java.util.List;
import java.util.Map;

public class House{
    @Protobuf(order = 1)
    private String id;
    @Protobuf(order = 2)
    private Long modelId;
    //摆放状态
    @Protobuf(order = 3,fieldType = FieldType.MAP)
    private Map<String,PutStatus> putStatusList;
    @Protobuf(order = 4)
    private String landCode;
    //点赞数量
    @Protobuf(order = 5,fieldType = FieldType.INT64)
    private long zan;

    public House() {
    }

    public House(String id, Long modelId, Map<String, PutStatus> putStatusList, String landCode) {
        this.id = id;
        this.modelId = modelId;
        this.putStatusList = putStatusList;
        this.landCode = landCode;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public Long getModelId() {
        return modelId;
    }

    public void setModelId(Long modelId) {
        this.modelId = modelId;
    }

    public Map<String, PutStatus> getPutStatusList() {
        return putStatusList;
    }

    public void setPutStatusList(Map<String, PutStatus> putStatusList) {
        this.putStatusList = putStatusList;
    }

    public String getLandCode() {
        return landCode;
    }

    public void setLandCode(String landCode) {
        this.landCode = landCode;
    }

    public long getZan() {
        return zan;
    }

    public void setZan(long zan) {
        this.zan = zan;
    }
}
