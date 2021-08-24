using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspCloseServerMessage  {
[ProtoMember(1)]
    public int code = -1;
}
}