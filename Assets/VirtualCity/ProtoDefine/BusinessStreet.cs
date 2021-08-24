using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class BusinessStreet {
[ProtoMember(1)]
    public  Dictionary<long?,Scence> shops;

    public Dictionary<long?, Scence> getShops() {
        return shops;
    }

    public void setShops(Dictionary<long?, Scence> shops) {
        this.shops = shops;
    }
}
}