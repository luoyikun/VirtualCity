package com.kingston.jforgame.server.game.entity.engineer;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.db.BaseEntity;
import com.kingston.jforgame.orm.utils.DbName;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;


@Entity
@DbName(name = "proxool.engineer_test")
public class HomeProperties extends BaseEntity {


	@Id
	@Column
	@Protobuf(order = 1)
	private Long id;

	/**
	 名称
	 */
	@Column
	@Protobuf(order = 2)
	private String name;

	/**
	 评分
	 */
	@Column
	@Protobuf(order = 3)
	private int score;

	/**
	 模型数据
	 */
	@Column
	@Protobuf(order = 4)
	private String modleData;

	/**
	 简模
	 */
	@Column
	@Protobuf(order = 5)
	private String modleDataGoodJianMo;

	/**
	 图标
	 */
	@Column
	@Protobuf(order = 6)
	private String icon;

	/**
	 室内起始点
	 */
	@Column
	@Protobuf(order = 7)
	private String lookIn;

	/**
	 室外起始点
	 */
	@Column
	@Protobuf(order = 8)
	private String lookOut;

	/**
	 楼层
	 */
	@Column
	@Protobuf(order = 9)
	private int floor;

	/**
	 购买所需金币
	 */
	@Column
	@Protobuf(order = 10)
	private int gold;

	/**
	 购买所需钻石
	 */
	@Column
	@Protobuf(order = 11)
	private int diamond;

	@Column
	@Protobuf(order = 12)
	private String createtime;

	@Column
	@Protobuf(order = 13)
	private String updatetime;

	public Long getId() {
		return id;
	}

	public void setId(Long id) {
		this.id = id;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public int getScore() {
		return score;
	}

	public void setScore(int score) {
		this.score = score;
	}

	public String getModleData() {
		return modleData;
	}

	public void setModleData(String modleData) {
		this.modleData = modleData;
	}

	public String getModleDataGoodJianMo() {
		return modleDataGoodJianMo;
	}

	public void setModleDataGoodJianMo(String modleDataGoodJianMo) {
		this.modleDataGoodJianMo = modleDataGoodJianMo;
	}

	public String getIcon() {
		return icon;
	}

	public void setIcon(String icon) {
		this.icon = icon;
	}

	public String getLookIn() {
		return lookIn;
	}

	public void setLookIn(String lookIn) {
		this.lookIn = lookIn;
	}

	public String getLookOut() {
		return lookOut;
	}

	public void setLookOut(String lookOut) {
		this.lookOut = lookOut;
	}

	public int getFloor() {
		return floor;
	}

	public void setFloor(int floor) {
		this.floor = floor;
	}

	public int getGold() {
		return gold;
	}

	public void setGold(int gold) {
		this.gold = gold;
	}

	public int getDiamond() {
		return diamond;
	}

	public void setDiamond(int diamond) {
		this.diamond = diamond;
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


