using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class House{
[ProtoMember(1)]
    public string id;
[ProtoMember(2)]
    public long? modelId = 1;
    //摆放状态
[ProtoMember(3)]
    public Dictionary<string,PutStatus> putStatusList = new Dictionary<string, PutStatus>();
[ProtoMember(4)]
    public string landCode = "1";
    //点赞数量
[ProtoMember(5)]
    public long zan;

    public House() {
    }

    public House(string id, long? modelId, Dictionary<string, PutStatus> putStatusList, string landCode) {
        this.id = id;
        this.modelId = modelId;
        this.putStatusList = putStatusList;
        this.landCode = landCode;
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

    public Dictionary<string, PutStatus> getPutStatusList() {
        return putStatusList;
    }

    public void setPutStatusList(Dictionary<string, PutStatus> putStatusList) {
        this.putStatusList = putStatusList;
    }

    public string getLandCode() {
        return landCode;
    }

    public void setLandCode(string landCode) {
        this.landCode = landCode;
    }

    public long getZan() {
        return zan;
    }

    public void setZan(long zan) {
        this.zan = zan;
    }
}
}