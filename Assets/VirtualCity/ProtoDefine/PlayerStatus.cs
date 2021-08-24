using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class PlayerStatus {
    //x坐标
[ProtoMember(1)]
    public float x;
    //y坐标
[ProtoMember(2)]
    public float y;
    //z坐标
[ProtoMember(3)]
    public float z;
    //动作
[ProtoMember(4)]
    public int acation;

    //方向x坐标
[ProtoMember(5)]
    public float px;
    //方向y坐标
[ProtoMember(6)]
    public float py;
    //方向z坐标
[ProtoMember(7)]
    public float pz;

    public PlayerStatus() {
    }



    public PlayerStatus( float x, float y, float z, int acation) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.acation = acation;
    }

    public PlayerStatus(long? accountId, float x, float y, float z, int acation, float px, float py, float pz) {
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
}