using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ReqRemoveHousePartMessage {
[ProtoMember(1)]
    public string houseId;
[ProtoMember(2)]
    public List<string> putStatusId;

    public ReqRemoveHousePartMessage() {
    }

    public ReqRemoveHousePartMessage(string houseId, List<string> putStatusId) {
        this.houseId = houseId;
        this.putStatusId = putStatusId;
    }

    public List<string> getPutStatusId() {
        return putStatusId;
    }

    public void setPutStatusId(List<string> putStatusId) {
        this.putStatusId = putStatusId;
    }

    public string getHouseId() {
        return houseId;
    }

    public void setHouseId(string houseId) {
        this.houseId = houseId;
    }


}
}