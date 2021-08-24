using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqUpdateOtherDataMessage {
[ProtoMember(1)]
    public string fieldName;
[ProtoMember(2)]
    public string value;

    public string getFieldName() {
        return fieldName;
    }

    public void setFieldName(string fieldName) {
        this.fieldName = fieldName;
    }

    public string getValue() {
        return value;
    }

    public void setValue(string value) {
        this.value = value;
    }
}
}