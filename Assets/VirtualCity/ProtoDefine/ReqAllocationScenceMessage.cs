using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqAllocationScenceMessage  {
[ProtoMember(1)]
    public long accountId;
}
}