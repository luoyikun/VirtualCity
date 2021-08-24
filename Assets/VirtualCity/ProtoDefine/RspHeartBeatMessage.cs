using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]




public class RspHeartBeatMessage {
[ProtoMember(1)]
    public int code;

    public RspHeartBeatMessage(int code) {
        this.code = code;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }


}
}