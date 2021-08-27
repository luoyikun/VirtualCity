using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class HomeProperties {


[ProtoMember(1)]
	public long? id = 1;

	/**
	 名称
	 */
[ProtoMember(2)]
	public string name = "xxx";

	/**
	 评分
	 */
[ProtoMember(3)]
	public int score;

	/**
	 模型数据
	 */
[ProtoMember(4)]
	public string modleData = "zhuzhai_haozhai01";

	/**
	 简模
	 */
[ProtoMember(5)]
	public string modleDataGoodJianMo = "zhuzhai_haozhai01";

	/**
	 图标
	 */
[ProtoMember(6)]
	public string icon;

	/**
	 室内起始点
	 */
[ProtoMember(7)]
	public string lookIn = "0,0,0";

	/**
	 室外起始点
	 */
[ProtoMember(8)]
	public string lookOut = "0,0,0";

        /**
         楼层
         */
        [ProtoMember(9)]
	public int floor = 3;

	/**
	 购买所需金币
	 */
[ProtoMember(10)]
	public int gold;

	/**
	 购买所需钻石
	 */
[ProtoMember(11)]
	public int diamond;

[ProtoMember(12)]
	public string createtime;

[ProtoMember(13)]
	public string updatetime;

	public long? getId() {
		return id;
	}

	public void setId(long? id) {
		this.id = id;
	}

	public string getName() {
		return name;
	}

	public void setName(string name) {
		this.name = name;
	}

	public int getScore() {
		return score;
	}

	public void setScore(int score) {
		this.score = score;
	}

	public string getModleData() {
		return modleData;
	}

	public void setModleData(string modleData) {
		this.modleData = modleData;
	}

	public string getModleDataGoodJianMo() {
		return modleDataGoodJianMo;
	}

	public void setModleDataGoodJianMo(string modleDataGoodJianMo) {
		this.modleDataGoodJianMo = modleDataGoodJianMo;
	}

	public string getIcon() {
		return icon;
	}

	public void setIcon(string icon) {
		this.icon = icon;
	}

	public string getLookIn() {
		return lookIn;
	}

	public void setLookIn(string lookIn) {
		this.lookIn = lookIn;
	}

	public string getLookOut() {
		return lookOut;
	}

	public void setLookOut(string lookOut) {
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