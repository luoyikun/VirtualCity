using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class Land {
[ProtoMember(1)]
    public string code;
    //建筑类型：0没有建筑，611住宅，613开发区
[ProtoMember(2)]
    public int buildType;
[ProtoMember(3)]
    public string buildId;
    //模型id
[ProtoMember(4)]
    public long? Model;

    public Land(string code, int buildType, string buildId, long? model) {
        this.code = code;
        this.buildType = buildType;
        this.buildId = buildId;
        Model = model;
    }

    public Land() {
    }

    public string getCode() {
        return code;
    }

    public void setCode(string code) {
        this.code = code;
    }

    public int getBuildType() {
        return buildType;
    }

    public void setBuildType(int buildType) {
        this.buildType = buildType;
    }

    public string getBuildId() {
        return buildId;
    }

    public void setBuildId(string buildId) {
        this.buildId = buildId;
    }

    public long? getModel() {
        return Model;
    }

    public void setModel(long? model) {
        Model = model;
    }
}
}