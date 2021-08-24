package com.kingston.jforgame.server.game.building.entity;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;

import java.util.Objects;

public class  PutStatus {
    @Protobuf(order = 1)
    private String id;
    /**模型ID*/
    @Protobuf(order = 2)
    private Long modelId;
    /**摆放楼层*/
    @Protobuf(order = 3)
    private int hasPut;
    /**摆放位置--x*/
    @Protobuf(order = 4,fieldType = FieldType.FLOAT)
    private float x;
    /**摆放位置--y*/
    @Protobuf(order = 5,fieldType = FieldType.FLOAT)
    private float y;
    /**摆放位置--z*/
    @Protobuf(order = 6,fieldType = FieldType.FLOAT)
    private float z;
    /**摆放方向--x*/
    @Protobuf(order = 7,fieldType = FieldType.FLOAT)
    private float dirX;
    /**摆放方向--y*/
    @Protobuf(order = 8,fieldType = FieldType.FLOAT)
    private float dirY;
    /**摆放方向--z*/
    @Protobuf(order = 9,fieldType = FieldType.FLOAT)
    private float dirZ;
    /**摆放方向--w*/
    @Protobuf(order = 10,fieldType = FieldType.FLOAT)
    private float dirW;
    @Protobuf(order = 11)
    private int hasPar;

    public int getHasPar() {
        return hasPar;
    }

    public void setHasPar(int hasPar) {
        this.hasPar = hasPar;
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

    public int getHasPut() {
        return hasPut;
    }

    public void setHasPut(int hasPut) {
        this.hasPut = hasPut;
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

    public float getDirX() {
        return dirX;
    }

    public void setDirX(float dirX) {
        this.dirX = dirX;
    }

    public float getDirY() {
        return dirY;
    }

    public void setDirY(float dirY) {
        this.dirY = dirY;
    }

    public float getDirZ() {
        return dirZ;
    }

    public void setDirZ(float dirZ) {
        this.dirZ = dirZ;
    }

    public float getDirW() {
        return dirW;
    }

    public void setDirW(float dirW) {
        this.dirW = dirW;
    }
}
