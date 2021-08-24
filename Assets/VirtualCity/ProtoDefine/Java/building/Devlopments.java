package com.kingston.jforgame.server.game.building.entity;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;

import java.util.Map;

public class Devlopments{
    @Protobuf(order = 1)
    private String id;
    /**模型ID*/
    @Protobuf(order = 2)
    private Long modelId;
    /**开发状态：0正在开发,1收益中,2没有收益*/
    @Protobuf(order = 3)
    private int status;
    /**开始建造时间*/
    @Protobuf(order = 4)
    private String buildDate;
    /**开始回报时间*/
    @Protobuf(order = 5)
    private String rewardDate;
    /**剩余收益时间*/
    @Protobuf(order = 6)
    private Long rewardTime;
    /**当前单位时间收益*/
    @Protobuf(order = 7)
    private Integer rewardUnit;
    /**当前收益*/
    @Protobuf(order = 8)
    private Integer rewardNum;
    /**好友帮忙次数*/
    @Protobuf(order = 9)
    private Integer friendHelp;
    /**加速次数*/
    @Protobuf(order = 10)
    private Integer speedUpTimes;
    /**好友收取次数*/
    @Protobuf(order = 11)
    private Integer stoneTimes;
    /**好友收取记录*/
    @Protobuf(order = 12,fieldType = FieldType.MAP)
    private Map<String,Recode> stoneRecod;
    /**好友帮忙记录*/
    @Protobuf(order = 13,fieldType = FieldType.MAP)
    private Map<String,Recode> helpRecod;
    @Protobuf(order = 14)
    private String landCode;

    public String getLandCode() {
        return landCode;
    }

    public void setLandCode(String landCode) {
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

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    public String getBuildDate() {
        return buildDate;
    }

    public void setBuildDate(String buildDate) {
        this.buildDate = buildDate;
    }

    public String getRewardDate() {
        return rewardDate;
    }

    public void setRewardDate(String rewardDate) {
        this.rewardDate = rewardDate;
    }

    public Long getRewardTime() {
        return rewardTime;
    }

    public void setRewardTime(Long rewardTime) {
        this.rewardTime = rewardTime;
    }

    public Integer getRewardUnit() {
        return rewardUnit;
    }

    public void setRewardUnit(Integer rewardUnit) {
        this.rewardUnit = rewardUnit;
    }

    public Integer getRewardNum() {
        return rewardNum;
    }

    public void setRewardNum(Integer rewardNum) {
        this.rewardNum = rewardNum;
    }

    public Integer getFriendHelp() {
        return friendHelp;
    }

    public void setFriendHelp(Integer friendHelp) {
        this.friendHelp = friendHelp;
    }

    public Integer getSpeedUpTimes() {
        return speedUpTimes;
    }

    public void setSpeedUpTimes(Integer speedUpTimes) {
        this.speedUpTimes = speedUpTimes;
    }

    public Integer getStoneTimes() {
        return stoneTimes;
    }

    public void setStoneTimes(Integer stoneTimes) {
        this.stoneTimes = stoneTimes;
    }

    public Map<String, Recode> getStoneRecod() {
        return stoneRecod;
    }

    public void setStoneRecod(Map<String, Recode> stoneRecod) {
        this.stoneRecod = stoneRecod;
    }

    public Map<String, Recode> getHelpRecod() {
        return helpRecod;
    }

    public void setHelpRecod(Map<String, Recode> helpRecod) {
        this.helpRecod = helpRecod;
    }
}
