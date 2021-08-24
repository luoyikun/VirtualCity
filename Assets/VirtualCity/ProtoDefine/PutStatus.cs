using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class  PutStatus {
[ProtoMember(1)]
    public string id;
    /**模型ID*/
[ProtoMember(2)]
    public long? modelId;
    /**摆放楼层*/
[ProtoMember(3)]
    public int hasPut;
    /**摆放位置--x*/
[ProtoMember(4)]
    public float x;
    /**摆放位置--y*/
[ProtoMember(5)]
    public float y;
    /**摆放位置--z*/
[ProtoMember(6)]
    public float z;
    /**摆放方向--x*/
[ProtoMember(7)]
    public float dirX;
    /**摆放方向--y*/
[ProtoMember(8)]
    public float dirY;
    /**摆放方向--z*/
[ProtoMember(9)]
    public float dirZ;
    /**摆放方向--w*/
[ProtoMember(10)]
    public float dirW;
[ProtoMember(11)]
    public int hasPar;

    public int getHasPar() {
        return hasPar;
    }

    public void setHasPar(int hasPar) {
        this.hasPar = hasPar;
    }

    public string getId() {
        return id;
    }

    public void setId(string id) {
        this.id = id;
    }

    public long? getModelId() {
        return modelId;
    }

    public void setModelId(long? modelId) {
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
}