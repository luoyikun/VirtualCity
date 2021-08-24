using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ReqUpdateHousePartMessage {
[ProtoMember(1)]
    public string houseId;
[ProtoMember(2)]
    public Dictionary<string, PutStatus> putStatusMap;

    public string getHouseId() {
        return houseId;
    }

    public void setHouseId(string houseId) {
        this.houseId = houseId;
    }

    public Dictionary<string, PutStatus> getPutStatusMap() {
        return putStatusMap;
    }

    public void setPutStatusMap(Dictionary<string, PutStatus> putStatusMap) {
        this.putStatusMap = putStatusMap;
    }
}
}