package com.kingston.jforgame.server.game.building.entity;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;

public class Recode {
    /**用户名*/
    @Protobuf(order = 1)
    private String name;
    /**时间*/
    @Protobuf(order = 2)
    private String time;
    /**操作类型：0,帮忙,1,收取*/
    @Protobuf(order = 3)
    private int handleType;
    @Protobuf(order = 4)
    /**帮忙：获得的钻石,收取：收取金币利润*/
    private long number;
        @Protobuf(order = 5)
    /**减少时间*/
    private long reduceTime;

    public Recode() {
    }

    public Recode(String name, String time, int handleType, long number, long reduceTime) {
        this.name = name;
        this.time = time;
        this.handleType = handleType;
        this.number = number;
        this.reduceTime = reduceTime;
    }

    public long getReduceTime() {
        return reduceTime;
    }

    public void setReduceTime(long reduceTime) {
        this.reduceTime = reduceTime;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getTime() {
        return time;
    }

    public void setTime(String time) {
        this.time = time;
    }

    public int getHandleType() {
        return handleType;
    }

    public void setHandleType(int handleType) {
        this.handleType = handleType;
    }

    public long getNumber() {
        return number;
    }

    public void setNumber(long number) {
        this.number = number;
    }
}
