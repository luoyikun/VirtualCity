using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class FatherProxyInfo {
[ProtoMember(1)]
    public int? level;

    public FatherProxyInfo(int level, long? fatherId) {
        this.level = level;
        this.fatherId = fatherId;
    }

[ProtoMember(2)]
    public long? fatherId;

    public FatherProxyInfo() {
    }



    public long? getFatherId() {
        return fatherId;
    }

    public void setFatherId(long? fatherId) {
        this.fatherId = fatherId;
    }


    public void setLevel(int level) {
        this.level = level;
    }
}
}