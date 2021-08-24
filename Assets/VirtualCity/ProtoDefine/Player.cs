using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]




public class Player {
[ProtoMember(1)]
    public long accountId;
[ProtoMember(2)]
    public long modelId;
[ProtoMember(3)]
    public string name;
[ProtoMember(4)]
    public string location;


    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }

    public long getModelId() {
        return modelId;
    }

    public void setModelId(long modelId) {
        this.modelId = modelId;
    }

    public string getName() {
        return name;
    }

    public void setName(string name) {
        this.name = name;
    }

    public string getLocation() {
        return location;
    }

    public void setLocation(string location) {
        this.location = location;
    }

}
}