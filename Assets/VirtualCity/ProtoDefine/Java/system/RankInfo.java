package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;

public class RankInfo {
    @Protobuf(order = 1)
    private Long accountId;
    @Protobuf(order = 2)
    private double info;
    @Protobuf(order = 3)
    private Long modelId;
    @Protobuf(order = 4)
    private String name;

    public Long getAccountId() {
        return accountId;
    }

    public void setAccountId(Long accountId) {
        this.accountId = accountId;
    }

    public double getInfo() {
        return info;
    }

    public void setInfo(double info) {
        this.info = info;
    }

    public Long getModelId() {
        return modelId;
    }

    public void setModelId(Long modelId) {
        this.modelId = modelId;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
