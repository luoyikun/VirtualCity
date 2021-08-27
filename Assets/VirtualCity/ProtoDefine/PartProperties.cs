using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class PartProperties {



[ProtoMember(1)]
	public long? id =1;

	/**
	 中文名
	 */
[ProtoMember(2)]
	public string cnName;

	/**
	 金币
	 */
[ProtoMember(3)]
	public int gold;

	/**
	 钻石
	 */
[ProtoMember(4)]
	public int diamond;

	/**
	 icon名称
	 */
[ProtoMember(5)]
	public string iconName = "jj_ty_chuang01";

	/**
	 0 代表 室内 1 室外
	 */
[ProtoMember(6)]
	public int inOutType = 0;

	/**
	 0：室内普通，  1：室内桌子  2：室内桌上   3：室外
	 0：室内普通，  1：室内桌子  2：室内桌上   3：室外
	 0：室内普通，  1：室内桌子  2：室内桌上   3：室外
	 */
[ProtoMember(7)]
	public int selftype;

	/**
	 模型数据
	 */
[ProtoMember(8)]
	public string modleData = "jj_ty_chuang01";

	/**
	 评分
	 */
[ProtoMember(9)]
	public int score;

	/**
	 类别code
	 */
[ProtoMember(10)]
	public int type = 0;

	/**
	 容器类别code
	 */
[ProtoMember(11)]
	public string fatherTypeId;

	/**
	 最小缩放级别
	 */
[ProtoMember(12)]
	public double minScale;

	/**
	 最大缩放级别
	 */
[ProtoMember(13)]
	public double maxScale;

	/**
	 对应商品类别id
	 */
[ProtoMember(14)]
	public long goodsKindId;

	/**
	 红包金额
	 */
[ProtoMember(15)]
	public float redPacket;

	/**
	 显示红包金额
	 */
[ProtoMember(16)]
	public float redPacketShow;

[ProtoMember(17)]
	public string createtime;

[ProtoMember(18)]
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

	public string getIconName() {
		return iconName;
	}

	public void setIconName(string iconName) {
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

	public string getModleData() {
		return modleData;
	}

	public void setModleData(string modleData) {
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

	public string getFatherTypeId() {
		return fatherTypeId;
	}

	public void setFatherTypeId(string fatherTypeId) {
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