package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.REQ_SEARCHUSER)
public class ReqSearchUserMessage extends Message {
    //如果群内搜索没有输入用户名传null即显示所有成员
    @Protobuf(order = 1)
    private String userName;
    //如果不在群内搜索传null
    @Protobuf(order = 2)
    private Long groupId;
    /**页码,搜索用户不填*/
    @Protobuf(order = 3)
    private int pageIndex;

    public ReqSearchUserMessage() {
    }

    public ReqSearchUserMessage(String userName, Long groupId) {
        this.userName = userName;
        this.groupId = groupId;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public Long getGroupId() {
        return groupId;
    }

    public void setGroupId(Long groupId) {
        this.groupId = groupId;
    }

    public int getPageIndex() {
        return pageIndex;
    }

    public void setPageIndex(int pageIndex) {
        this.pageIndex = pageIndex;
    }
}