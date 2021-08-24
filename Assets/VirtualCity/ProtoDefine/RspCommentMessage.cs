using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


/**
 * 通用ResponseMessage,需另外设置cmd
 */
public class RspCommentMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tip;
[ProtoMember(3)]
    public int rspcmd;

    public RspCommentMessage() {
    }

    public RspCommentMessage(int code, string tip, int cmd) {
        this.code = code;
        this.tip = tip;
        this.rspcmd = cmd;
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

    public int getRspcmd() {
        return rspcmd;
    }

    public void setRspcmd(int rspcmd) {
        this.rspcmd = rspcmd;
    }

    public string tostring() {
        return "RspCommentMessage{" +
                "code=" + code +
                ", tip='" + tip + '\'' +
                '}';
    }
}
}