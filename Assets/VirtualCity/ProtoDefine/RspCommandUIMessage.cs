using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspCommandUIMessage {
[ProtoMember(1)]
    public string ui;

    public string getUi() {
        return ui;
    }

    public void setUi(string ui) {
        this.ui = ui;
    }

    public RspCommandUIMessage(string ui) {
        this.ui = ui;
    }

    public RspCommandUIMessage() {
    }
}
}