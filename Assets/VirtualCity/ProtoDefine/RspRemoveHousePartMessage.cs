using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspRemoveHousePartMessage {
[ProtoMember(1)]
    public List<HouseParts> parts;
[ProtoMember(2)]
    public int code;
[ProtoMember(3)]
    public string tip;



    public List<HouseParts> getParts() {
        return parts;
    }

    public void setParts(List<HouseParts> parts) {
        this.parts = parts;
    }


    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public string getTip() {
        return tip;
    }

    public void setTip(string tip) {
        this.tip = tip;
    }

    public RspRemoveHousePartMessage() {
    }

    public RspRemoveHousePartMessage(int code, string tip) {
        this.code = code;
        this.tip = tip;
    }

    public RspRemoveHousePartMessage(List<HouseParts> parts, int code, string tip) {
        this.parts = parts;

        this.code = code;
        this.tip = tip;
    }
}
}