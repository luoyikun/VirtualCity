package com.kingston.jforgame.server.game.login.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.engineer.*;
import com.kingston.jforgame.server.game.login.LoginDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.LOGIN,cmd = LoginDataPool.RSP_LOAD_DATE)
public class RspGameDataMessage extends Message {
    /**
     * 系统配置数据
     */
    @Protobuf(fieldType = FieldType.OBJECT,order = 1)
    private List<SysProperties> userProperties;

    /**
     * 房屋配置数据
     */
    @Protobuf(fieldType = FieldType.OBJECT,order = 2)
    private List<HomeProperties> homeProperties;
    /**
     * 家具配置数据
     */
    @Protobuf(fieldType = FieldType.OBJECT,order = 3)
    private List<PartProperties> partProperties;
    /**
     * 建筑配置数据
     */
    @Protobuf(fieldType = FieldType.OBJECT,order = 4)
    private List<DevlopmentProperties> devlopmentProperties;
    /**
     * 商业模式配置数据
     */
    @Protobuf(fieldType = FieldType.OBJECT,order = 5)
    private List<BusinessModelProperties> businessModelProperties;
    /**
     * 角色配置数据
     */
    @Protobuf(fieldType = FieldType.OBJECT,order = 6)
    private List<RoleProperties> roleProperties;

    /**
     * 商铺配置数据
     */
    @Protobuf(fieldType = FieldType.OBJECT,order = 7)
    private List<ShopsProperties> shopsProperties;

    @Protobuf(order = 8)
    private int code;
    @Protobuf(order = 9)
    private String tip;

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getTip() {
        return tip;
    }

    public void setTip(String tip) {
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
