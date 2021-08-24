package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

/**
 * 通用ResponseMessage,需另外设置cmd
 */
@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.RSP_COMMENT)
public class RspCommentMessage extends Message {
    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String tip;
    @Protobuf(order = 3)
    private int rspcmd;

    public RspCommentMessage() {
    }

    public RspCommentMessage(int code, String tip, int cmd) {
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

    public String getTip() {
        return tip;
    }

    public void setTip(String tip) {
        this.tip = tip;
    }

    public int getRspcmd() {
        return rspcmd;
    }

    public void setRspcmd(int rspcmd) {
        this.rspcmd = rspcmd;
    }

    @Override
    public String toString() {
        return "RspCommentMessage{" +
                "code=" + code +
                ", tip='" + tip + '\'' +
                '}';
    }
}
