package com.kingston.jforgame.server.game.entity.mall;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.db.BaseEntity;
import com.kingston.jforgame.orm.utils.DbName;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;


@Entity
@DbName(name = "proxool.mall")
public class Comment extends BaseEntity {

    @Id
    @Column
    @Protobuf(order = 1)
    private Long id;

    /**
     商品id
     */
    @Column
    @Protobuf(order = 2)
    private long goodsId;

    /**
     商品评价内容
     */
    @Column
    @Protobuf(order = 3)
    private String text;

    /**
     评价星级
     */
    @Column
    @Protobuf(order = 4)
    private int star;

    /**
     评价账户id
     */
    @Column
    @Protobuf(order = 5)
    private long accountId;

    /**
     评论用户昵称
     */
    @Column
    @Protobuf(order = 6)
    private String accountName;

    /**
     评论用户头像
     */
    @Column
    @Protobuf(order = 7)
    private long moudleId;

    /**
     商品种类
     */
    @Column
    @Protobuf(order = 8)
    private long goodsKindId;

    /**
     确认收货时间
     */
    @Column
    @Protobuf(order = 9)
    private String confirmTime;

    @Column
    @Protobuf(order = 10)
    private String createtime;

    @Column
    @Protobuf(order = 11)
    private String updatetime;

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public long getGoodsId() {
        return goodsId;
    }

    public void setGoodsId(long goodsId) {
        this.goodsId = goodsId;
    }

    public String getText() {
        return text;
    }

    public void setText(String text) {
        this.text = text;
    }

    public int getStar() {
        return star;
    }

    public void setStar(int star) {
        this.star = star;
    }

    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }

    public String getAccountName() {
        return accountName;
    }

    public void setAccountName(String accountName) {
        this.accountName = accountName;
    }

    public long getMoudleId() {
        return moudleId;
    }

    public void setMoudleId(long moudleId) {
        this.moudleId = moudleId;
    }

    public long getGoodsKindId() {
        return goodsKindId;
    }

    public void setGoodsKindId(long goodsKindId) {
        this.goodsKindId = goodsKindId;
    }

    public String getConfirmTime() {
        return confirmTime;
    }

    public void setConfirmTime(String confirmTime) {
        this.confirmTime = confirmTime;
    }

    public String getCreatetime() {
        return createtime;
    }

    public void setCreatetime(String createtime) {
        this.createtime = createtime;
    }

    public String getUpdatetime() {
        return updatetime;
    }

    public void setUpdatetime(String updatetime) {
        this.updatetime = updatetime;
    }

}


