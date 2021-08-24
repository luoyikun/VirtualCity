using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ShopsProperties {


[ProtoMember(1)]
	public long? id;

	/**
	 商家名称
	 */
[ProtoMember(2)]
	public string name;

	/**
	 模型ID
	 */
[ProtoMember(3)]
	public string moduleId;

	/**
	 坐标X
	 */
[ProtoMember(4)]
	public float x;

	/**
	 坐标X
	 */
[ProtoMember(5)]
	public float y;

	/**
	 坐标X
	 */
[ProtoMember(6)]
	public float z;

	/**
	 商家ID
	 */
[ProtoMember(7)]
	public long businessId;

	/**
	 是否在商业街里面
	 */
[ProtoMember(8)]
	public int isInOut;

	/**
	 店铺出生点
	 */
[ProtoMember(9)]
	public string bornPos;

	/**
	 npc地点
	 */
[ProtoMember(10)]
	public string npcPos;

	/**
	 相机地点
	 */
[ProtoMember(11)]
	public string cameraPos;

	/**
	 npc模型
	 */
[ProtoMember(12)]
	public string npcModel;

[ProtoMember(13)]
	public string createtime;

[ProtoMember(14)]
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

	public string getModuleId() {
		return moduleId;
	}

	public void setModuleId(string moduleId) {
		this.moduleId = moduleId;
	}

	public float getX() {
		return x;
	}

	public void setX(float x) {
		this.x = x;
	}

	public float getY() {
		return y;
	}

	public void setY(float y) {
		this.y = y;
	}

	public float getZ() {
		return z;
	}

	public void setZ(float z) {
		this.z = z;
	}

	public long getBusinessId() {
		return businessId;
	}

	public void setBusinessId(long businessId) {
		this.businessId = businessId;
	}

	public int getIsInOut() {
		return isInOut;
	}

	public void setIsInOut(int isInOut) {
		this.isInOut = isInOut;
	}

	public string getBornPos() {
		return bornPos;
	}

	public void setBornPos(string bornPos) {
		this.bornPos = bornPos;
	}

	public string getNpcPos() {
		return npcPos;
	}

	public void setNpcPos(string npcPos) {
		this.npcPos = npcPos;
	}

	public string getCameraPos() {
		return cameraPos;
	}

	public void setCameraPos(string cameraPos) {
		this.cameraPos = cameraPos;
	}

	public string getNpcModel() {
		return npcModel;
	}

	public void setNpcModel(string npcModel) {
		this.npcModel = npcModel;
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