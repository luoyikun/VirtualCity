using ProtoBuf;
namespace ProtoDefine
{
 [ProtoContract]
 public class ProxyRelation
{
 [ProtoMember(1)]
private  long? id;

/**
 客户
*/
 [ProtoMember(2)]
private  long? accountId;

/**
 一层代理数
*/
 [ProtoMember(3)]
private  int? proxyNum;

/**
 父级代理账户ID
*/
 [ProtoMember(4)]
private  long? fatherId;

/**
 产生关系的总代理数
*/
 [ProtoMember(5)]
private  int? proxyNumTotle;

 [ProtoMember(6)]
private  string createtime;

 [ProtoMember(7)]
private  string updatetime;

public long? Id
{

get {	 return id==-999?null:id ;	}

 set {	id = value;	}

}

public long? AccountId
{

get {	 return accountId==-999?null:accountId ;	}

 set {	accountId = value;	}

}

public int? ProxyNum
{

get {	 return proxyNum==-999?null:proxyNum ;	}

 set {	proxyNum = value;	}

}

public long? FatherId
{

get {	 return fatherId==-999?null:fatherId ;	}

 set {	fatherId = value;	}

}

public int? ProxyNumTotle
{

get {	 return proxyNumTotle==-999?null:proxyNumTotle ;	}

 set {	proxyNumTotle = value;	}

}

public string Createtime
{

get {	 return createtime ;	}

 set {	createtime = value;	}

}

public string Updatetime
{

get {	 return updatetime ;	}

 set {	updatetime = value;	}

}

}

}

