using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ReqGetUserInfoMessage {
[ProtoMember(1)]
    public List<long?> accountId;

    public ReqGetUserInfoMessage() {
    }

    public List<long?> getAccountId() {
        return accountId;
    }

    public void setAccountId(List<long?> accountId) {
        this.accountId = accountId;
    }
}
}