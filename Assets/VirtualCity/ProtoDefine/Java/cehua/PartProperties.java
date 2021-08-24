package com.kingston.jforgame.server.game.entity.engineer;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.db.BaseEntity;
import com.kingston.jforgame.orm.utils.DbName;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;


@Entity
@DbName(name = "proxool.data")
public class PartProperties extends BaseEntity {



	@Id
	@Column
	@Protobuf(order = 1)
	private Long id;

	/**
	 中文名
	 */
	@Column
	@Protobuf(order = 2)
	private String cnName;

	/**
	 金币
	 */
	@Column
	@Protobuf(order = 3)
	private int gold;

	/**
	 钻石
	 */
	@Column
	@Protobuf(order = 4)
	private int diamond;

	/**
	 icon名称
	 */
	@Column
	@Protobuf(order = 5)
	private String iconName;

	/**
	 0 代表 室内 1 室外
	 */
	@Column
	@Protobuf(order = 6)
	private int inOutType;

	/**
	 0：室内普通，  1：室内桌子  2：室内桌上   3：室外
	 0：室内普通，  1：室内桌子  2：室内桌上   3：室外
	 0：室内普通，  1：室内桌子  2：室内桌上   3：室外
	 */
	@Column
	@Protobuf(order = 7)
	private int selftype;

	/**
	 模型数据
	 */
	@Column
	@Protobuf(order = 8)
	private String modleData;

	/**
	 评分
	 */
	@Column
	@Protobuf(order = 9)
	private int score;

	/**
	 类别code
	 */
	@Column
	@Protobuf(order = 10)
	private int type;

	/**
	 容器类别code
	 */
	@Column
	@Protobuf(order = 11)
	private String fatherTypeId;

	/**
	 最小缩放级别
	 */
	@Column
	@Protobuf(order = 12)
	private double minScale;

	/**
	 最大缩放级别
	 */
	@Column
	@Protobuf(order = 13)
	private double maxScale;

	/**
	 对应商品类别id
	 */
	@Column
	@Protobuf(order = 14)
	private long goodsKindId;

	/**
	 红包金额
	 */
	@Column
	@Protobuf(order = 15)
	private float redPacket;

	/**
	 显示红包金额
	 */
	@Column
	@Protobuf(order = 16)
	private float redPacketShow;

	@Column
	@Protobuf(order = 17)
	private String createtime;

	@Column
	@Protobuf(order = 18)
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

	public String getIconName() {
		return iconName;
	}

	public void setIconName(String iconName) {
		this.iconName = iconName;
	}

	public int getInOutType() {
		return inOutType;
	}

	public void setInOutType(int inOutType) {
		this.inOutType = inOutType;
	}

	public int getSelftype() {
		return selftype;
	}

	public void setSelftype(int selftype) {
		this.selftype = selftype;
	}

	public String getModleData() {
		return modleData;
	}

	public void setModleData(String modleData) {
		this.modleData = modleData;
	}

	public int getScore() {
		return score;
	}

	public void setScore(int score) {
		this.score = score;
	}

	public int getType() {
		return type;
	}

	public void setType(int type) {
		this.type = type;
	}

	public String getFatherTypeId() {
		return fatherTypeId;
	}

	public void setFatherTypeId(String fatherTypeId) {
		this.fatherTypeId = fatherTypeId;
	}

	public double getMinScale() {
		return minScale;
	}

	public void setMinScale(double minScale) {
		this.minScale = minScale;
	}

	public double getMaxScale() {
		return maxScale;
	}

	public void setMaxScale(double maxScale) {
		this.maxScale = maxScale;
	}

	public long getGoodsKindId() {
		return goodsKindId;
	}

	public void setGoodsKindId(long goodsKindId) {
		this.goodsKindId = goodsKindId;
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


