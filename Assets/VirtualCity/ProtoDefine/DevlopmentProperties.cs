using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class DevlopmentProperties {



[ProtoMember(1)]
	public long? id;

	/**
	 中文名称
	 */
[ProtoMember(2)]
	public string cnName;

	/**
	 模型数据josn
	 */
[ProtoMember(3)]
	public string modleData;

	/**
	 所需金币
	 */
[ProtoMember(4)]
	public int gold;

	/**
	 建造所需的评分
	 */
[ProtoMember(5)]
	public int scoreNeed;

	/**
	 加速一次消耗的钻石数量
	 */
[ProtoMember(6)]
	public int oncespeedupCost;

	/**
	 所需钻石
	 */
[ProtoMember(7)]
	public int diamond;

	/**
	 建成所需时间
	 */
[ProtoMember(8)]
	public long builtTime;

	/**
	 收益时间
	 */
[ProtoMember(9)]
	public long maxIncomeTime;

	/**
	 总利润
	 */
[ProtoMember(10)]
	public int income;

	/**
	 每秒钟收入
	 */
[ProtoMember(11)]
	public float rewardUnit;

	/**
	 红包大小
	 */
[ProtoMember(12)]
	public float redPacket;

	/**
	 显示的红包大小
	 */
[ProtoMember(13)]
	public float redPacketShow;

[ProtoMember(14)]
	public string createtime;

[ProtoMember(15)]
	public string updatetime;

	public long? getId() {
		return id;
	}

	public void setId(long? id) {
		this.id = id;
	}

	public string getCnName() {
		return cnName;
	}

	public void setCnName(string cnName) {
		this.cnName = cnName;
	}

	public string getModleData() {
		return modleData;
	}

	public void setModleData(string modleData) {
		this.modleData = modleData;
	}

	public int getGold() {
		return gold;
	}

	public void setGold(int gold) {
		this.gold = gold;
	}

	public int getScoreNeed() {
		return scoreNeed;
	}

	public void setScoreNeed(int scoreNeed) {
		this.scoreNeed = scoreNeed;
	}

	public int getOncespeedupCost() {
		return oncespeedupCost;
	}

	public void setOncespeedupCost(int oncespeedupCost) {
		this.oncespeedupCost = oncespeedupCost;
	}

	public int getDiamond() {
		return diamond;
	}

	public void setDiamond(int diamond) {
		this.diamond = diamond;
	}

	public long getBuiltTime() {
		return builtTime;
	}

	public void setBuiltTime(long builtTime) {
		this.builtTime = builtTime;
	}

	public long getMaxIncomeTime() {
		return maxIncomeTime;
	}

	public void setMaxIncomeTime(long maxIncomeTime) {
		this.maxIncomeTime = maxIncomeTime;
	}

	public int getIncome() {
		return income;
	}

	public void setIncome(int income) {
		this.income = income;
	}

	public float getRewardUnit() {
		return rewardUnit;
	}

	public void setRewardUnit(float rewardUnit) {
		this.rewardUnit = rewardUnit;
	}

	public float getRedPacket() {
		return redPacket;
	}

	public void setRedPacket(float redPacket) {
		this.redPacket = redPacket;
	}

	public float getRedPacketShow() {
		return redPacketShow;
	}

	public void setRedPacketShow(float redPacketShow) {
		this.redPacketShow = redPacketShow;
	}

	public string getCreatetime() {
		return createtime;
	}

	public void setCreatetime(string createtime) {
		this.createtime = createtime;
	}

	public string getUpdatetime() {
		return updatetime;
	}

	public void setUpdatetime(string updatetime) {
		this.updatetime = updatetime;
	}


}


}