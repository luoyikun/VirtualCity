using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RankInfo {
[ProtoMember(1)]
    public long? accountId;
[ProtoMember(2)]
    public double info;
[ProtoMember(3)]
    public long? modelId;
[ProtoMember(4)]
    public string name;

    public long? getAccountId() {
        return accountId;
    }

    public void setAccountId(long? accountId) {
        this.accountId = accountId;
    }

    public double getInfo() {
        return info;
    }

    public void setInfo(double info) {
        this.info = info;
    }

    public long? getModelId() {
        return modelId;
    }

    public void setModelId(long? modelId) {
        this.modelId = modelId;
    }

    public string getName() {
        return name;
    }

    public void setName(string name) {
        this.name = name;
    }
}
}