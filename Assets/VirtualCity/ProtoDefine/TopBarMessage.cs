using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class TopBarMessage {
[ProtoMember(1)]
    public string message;

    public TopBarMessage() {
    }

    public TopBarMessage(string message) {
        this.message = message;
    }

    public string getMessage() {
        return message;
    }

    public void setMessage(string message) {
        this.message = message;
    }
}
}