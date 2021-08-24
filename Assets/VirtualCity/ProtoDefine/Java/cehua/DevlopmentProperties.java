package com.kingston.jforgame.server.game.entity.engineer;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.db.BaseEntity;
import com.kingston.jforgame.orm.utils.DbName;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;


@Entity
@DbName(name = "proxool.data")
public class DevlopmentProperties extends BaseEntity {



	@Id
	@Column
	@Protobuf(order = 1)
	private Long id;

	/**
	 中文名称
	 */
	@Column
	@Protobuf(order = 2)
	private String cnName;

	/**
	 模型数据josn
	 */
	@Column
	@Protobuf(order = 3)
	private String modleData;

	/**
	 所需金币
	 */
	@Column
	@Protobuf(order = 4)
	private int gold;

	/**
	 建造所需的评分
	 */
	@Column
	@Protobuf(order = 5)
	private int scoreNeed;

	/**
	 加速一次消耗的钻石数量
	 */
	@Column
	@Protobuf(order = 6)
	private int oncespeedupCost;

	/**
	 所需钻石
	 */
	@Column
	@Protobuf(order = 7)
	private int diamond;

	/**
	 建成所需时间
	 */
	@Column
	@Protobuf(order = 8)
	private long builtTime;

	/**
	 收益时间
	 */
	@Column
	@Protobuf(order = 9)
	private long maxIncomeTime;

	/**
	 总利润
	 */
	@Column
	@Protobuf(order = 10)
	private int income;

	/**
	 每秒钟收入
	 */
	@Column
	@Protobuf(order = 11)
	private float rewardUnit;

	/**
	 红包大小
	 */
	@Column
	@Protobuf(order = 12)
	private float redPacket;

	/**
	 显示的红包大小
	 */
	@Column
	@Protobuf(order = 13)
	private float redPacketShow;

	@Column
	@Protobuf(order = 14)
	private String createtime;

	@Column
	@Protobuf(order = 15)
	private String updatetime;

	public Long getId() {
		return id;
	}

	public void setId(Long id) {
		this.id = id;
	}

	public String getCnName() {
		return cnName;
	}

	public void setCnName(String cnName) {
		this.cnName = cnName;
	}

	public String getModleData() {
		return modleData;
	}

	public void setModleData(String modleData) {
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

	public String getCreatetime() {
		return createtime;
	}

	public void setCreatetime(String createtime) {
		this.createtime = createtime;
	}

	public String getUpdatetime() {
		return updatetime;
	}

	public void setUpdatetime(String updatetime) {
		this.updatetime = updatetime;
	}


}


