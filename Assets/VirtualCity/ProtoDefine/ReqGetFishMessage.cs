using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqGetFishMessage {
[ProtoMember(1)]
    public long accountId;

    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }
}
}