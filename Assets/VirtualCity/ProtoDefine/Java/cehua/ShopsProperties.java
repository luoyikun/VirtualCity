package com.kingston.jforgame.server.game.entity.engineer;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.db.BaseEntity;
import com.kingston.jforgame.orm.utils.DbName;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;


@Entity
@DbName(name = "proxool.engineer_test")
public class ShopsProperties extends BaseEntity {


	@Id
	@Column
	@Protobuf(order = 1)
	private Long id;

	/**
	 商家名称
	 */
	@Column
	@Protobuf(order = 2)
	private String name;

	/**
	 模型ID
	 */
	@Column
	@Protobuf(order = 3)
	private String moduleId;

	/**
	 坐标X
	 */
	@Column
	@Protobuf(order = 4)
	private float x;

	/**
	 坐标X
	 */
	@Column
	@Protobuf(order = 5)
	private float y;

	/**
	 坐标X
	 */
	@Column
	@Protobuf(order = 6)
	private float z;

	/**
	 商家ID
	 */
	@Column
	@Protobuf(order = 7)
	private long businessId;

	/**
	 是否在商业街里面
	 */
	@Column
	@Protobuf(order = 8)
	private int isInOut;

	/**
	 店铺出生点
	 */
	@Column
	@Protobuf(order = 9)
	private String bornPos;

	/**
	 npc地点
	 */
	@Column
	@Protobuf(order = 10)
	private String npcPos;

	/**
	 相机地点
	 */
	@Column
	@Protobuf(order = 11)
	private String cameraPos;

	/**
	 npc模型
	 */
	@Column
	@Protobuf(order = 12)
	private String npcModel;

	@Column
	@Protobuf(order = 13)
	private String createtime;

	@Column
	@Protobuf(order = 14)
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

	public String getModuleId() {
		return moduleId;
	}

	public void setModuleId(String moduleId) {
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

	public String getBornPos() {
		return bornPos;
	}

	public void setBornPos(String bornPos) {
		this.bornPos = bornPos;
	}

	public String getNpcPos() {
		return npcPos;
	}

	public void setNpcPos(String npcPos) {
		this.npcPos = npcPos;
	}

	public String getCameraPos() {
		return cameraPos;
	}

	public void setCameraPos(String cameraPos) {
		this.cameraPos = cameraPos;
	}

	public String getNpcModel() {
		return npcModel;
	}

	public void setNpcModel(String npcModel) {
		this.npcModel = npcModel;
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


