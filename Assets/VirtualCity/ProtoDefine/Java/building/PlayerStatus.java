package com.kingston.jforgame.server.game.building.entity;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;

public class PlayerStatus {
    //x坐标
    @Protobuf(order = 1,fieldType = FieldType.FLOAT)
    private float x;
    //y坐标
    @Protobuf(order = 2,fieldType = FieldType.FLOAT)
    private float y;
    //z坐标
    @Protobuf(order = 3,fieldType = FieldType.FLOAT)
    private float z;
    //动作
    @Protobuf(order = 4)
    private int acation;

    //方向x坐标
    @Protobuf(order = 5,fieldType = FieldType.FLOAT)
    private float px;
    //方向y坐标
    @Protobuf(order = 6,fieldType = FieldType.FLOAT)
    private float py;
    //方向z坐标
    @Protobuf(order = 7,fieldType = FieldType.FLOAT)
    private float pz;

    public PlayerStatus() {
    }



    public PlayerStatus( float x, float y, float z, int acation) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.acation = acation;
    }

    public PlayerStatus(Long accountId, float x, float y, float z, int acation, float px, float py, float pz) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.acation = acation;
        this.px = px;
        this.py = py;
        this.pz = pz;
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

    public int getAcation() {
        return acation;
    }

    public void setAcation(int acation) {
        this.acation = acation;
    }

    public float getPx() {
        return px;
    }

    public void setPx(float px) {
        this.px = px;
    }

    public float getPy() {
        return py;
    }

    public void setPy(float py) {
        this.py = py;
    }

    public float getPz() {
        return pz;
    }

    public void setPz(float pz) {
        this.pz = pz;
    }
}
