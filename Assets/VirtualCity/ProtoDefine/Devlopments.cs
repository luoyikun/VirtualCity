using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class Devlopments{
[ProtoMember(1)]
    public string id;
    /**模型ID*/
[ProtoMember(2)]
    public long? modelId;
    /**开发状态：0正在开发,1收益中,2没有收益*/
[ProtoMember(3)]
    public int status;
    /**开始建造时间*/
[ProtoMember(4)]
    public string buildDate;
    /**开始回报时间*/
[ProtoMember(5)]
    public string rewardDate;
    /**剩余收益时间*/
[ProtoMember(6)]
    public long? rewardTime;
    /**当前单位时间收益*/
[ProtoMember(7)]
    public int? rewardUnit;
    /**当前收益*/
[ProtoMember(8)]
    public int? rewardNum;
    /**好友帮忙次数*/
[ProtoMember(9)]
    public int? friendHelp;
    /**加速次数*/
[ProtoMember(10)]
    public int? speedUpTimes;
    /**好友收取次数*/
[ProtoMember(11)]
    public int? stoneTimes;
    /**好友收取记录*/
[ProtoMember(12)]
    public Dictionary<string,Recode> stoneRecod;
    /**好友帮忙记录*/
[ProtoMember(13)]
    public Dictionary<string,Recode> helpRecod;
[ProtoMember(14)]
    public string landCode;

    public string getLandCode() {
        return landCode;
    }

    public void setLandCode(string landCode) {
        this.landCode = landCode;
    }

    public string getId() {
        return id;
    }

    public void setId(string id) {
        this.id = id;
    }

    public long? getModelId() {
        return modelId;
    }

    public void setModelId(long? modelId) {
        this.modelId = modelId;
    }

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    public string getBuildDate() {
        return buildDate;
    }

    public void setBuildDate(string buildDate) {
        this.buildDate = buildDate;
    }

    public string getRewardDate() {
        return rewardDate;
    }

    public void setRewardDate(string rewardDate) {
        this.rewardDate = rewardDate;
    }

    public long? getRewardTime() {
        return rewardTime;
    }

    public void setRewardTime(long? rewardTime) {
        this.rewardTime = rewardTime;
    }

    public int? getRewardUnit() {
        return rewardUnit;
    }

    public void setRewardUnit(int? rewardUnit) {
        this.rewardUnit = rewardUnit;
    }

    public int? getRewardNum() {
        return rewardNum;
    }

    public void setRewardNum(int? rewardNum) {
        this.rewardNum = rewardNum;
    }

    public int? getFriendHelp() {
        return friendHelp;
    }

    public void setFriendHelp(int? friendHelp) {
        this.friendHelp = friendHelp;
    }

    public int? getSpeedUpTimes() {
        return speedUpTimes;
    }

    public void setSpeedUpTimes(int? speedUpTimes) {
        this.speedUpTimes = speedUpTimes;
    }

    public int? getStoneTimes() {
        return stoneTimes;
    }

    public void setStoneTimes(int? stoneTimes) {
        this.stoneTimes = stoneTimes;
    }

    public Dictionary<string, Recode> getStoneRecod() {
        return stoneRecod;
    }

    public void setStoneRecod(Dictionary<string, Recode> stoneRecod) {
        this.stoneRecod = stoneRecod;
    }

    public Dictionary<string, Recode> getHelpRecod() {
        return helpRecod;
    }

    public void setHelpRecod(Dictionary<string, Recode> helpRecod) {
        this.helpRecod = helpRecod;
    }
}
}