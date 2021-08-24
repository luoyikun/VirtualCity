using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqLogoutHallMessage {
[ProtoMember(1)]
    public  long? accountId;

    public ReqLogoutHallMessage() {
    }

    public ReqLogoutHallMessage(long? accountId) {
        this.accountId = accountId;
    }

    public long? getAccountId() {
        return accountId;
    }

    public void setAccountId(long? accountId) {
        this.accountId = accountId;
    }
}
}