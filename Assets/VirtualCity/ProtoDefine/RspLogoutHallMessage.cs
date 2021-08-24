using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspLogoutHallMessage {
[ProtoMember(1)]
    public  long? accountId;

    public RspLogoutHallMessage() {
    }

    public RspLogoutHallMessage(long? accountId) {
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