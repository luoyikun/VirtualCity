using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqPutHousePartMessage {
[ProtoMember(1)]
    public string houseId;
[ProtoMember(2)]
    public PutStatus putStatus;

    public ReqPutHousePartMessage() {
    }

    public ReqPutHousePartMessage(string houseId, PutStatus putStatus) {
        this.houseId = houseId;
        this.putStatus = putStatus;
    }

    public string getHouseId() {
        return houseId;
    }

    public void setHouseId(string houseId) {
        this.houseId = houseId;
    }

    public PutStatus getPutStatus() {
        return putStatus;
    }

    public void setPutStatus(PutStatus putStatus) {
        this.putStatus = putStatus;
    }
}
}