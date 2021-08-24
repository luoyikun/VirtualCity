using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspGameDataMessage {
    /**
     * 系统配置数据
     */
[ProtoMember(1)]
    public List<SysProperties> userProperties;

    /**
     * 房屋配置数据
     */
[ProtoMember(2)]
    public List<HomeProperties> homeProperties;
    /**
     * 家具配置数据
     */
[ProtoMember(3)]
    public List<PartProperties> partProperties;
    /**
     * 建筑配置数据
     */
[ProtoMember(4)]
    public List<DevlopmentProperties> devlopmentProperties;
    /**
     * 商业模式配置数据
     */
[ProtoMember(5)]
    public List<BusinessModelProperties> businessModelProperties;
    /**
     * 角色配置数据
     */
[ProtoMember(6)]
    public List<RoleProperties> roleProperties;

    /**
     * 商铺配置数据
     */
[ProtoMember(7)]
    public List<ShopsProperties> shopsProperties;

[ProtoMember(8)]
    public int code;
[ProtoMember(9)]
    public string tip;

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

    public List<SysProperties> getUserProperties() {
        return userProperties;
    }

    public void setUserProperties(List<SysProperties> userProperties) {
        this.userProperties = userProperties;
    }

    public List<HomeProperties> getHomeProperties() {
        return homeProperties;
    }

    public void setHomeProperties(List<HomeProperties> homeProperties) {
        this.homeProperties = homeProperties;
    }

    public List<PartProperties> getPartProperties() {
        return partProperties;
    }

    public void setPartProperties(List<PartProperties> partProperties) {
        this.partProperties = partProperties;
    }

    public List<DevlopmentProperties> getDevlopmentProperties() {
        return devlopmentProperties;
    }

    public void setDevlopmentProperties(List<DevlopmentProperties> devlopmentProperties) {
        this.devlopmentProperties = devlopmentProperties;
    }

    public List<BusinessModelProperties> getBusinessModelProperties() {
        return businessModelProperties;
    }

    public void setBusinessModelProperties(List<BusinessModelProperties> businessModelProperties) {
        this.businessModelProperties = businessModelProperties;
    }

    public List<RoleProperties> getRoleProperties() {
        return roleProperties;
    }

    public void setRoleProperties(List<RoleProperties> roleProperties) {
        this.roleProperties = roleProperties;
    }

    public List<ShopsProperties> getShopsProperties() {
        return shopsProperties;
    }

    public void setShopsProperties(List<ShopsProperties> shopsProperties) {
        this.shopsProperties = shopsProperties;
    }
}
}