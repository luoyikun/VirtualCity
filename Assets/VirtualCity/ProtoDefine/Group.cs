using ProtoBuf;
namespace ProtoDefine
{
 [ProtoContract]
 public class Group
{
 [ProtoMember(1)]
private  long? id;

/**
 群名称
*/
 [ProtoMember(2)]
private  string name;

/**
 群成员Id列表
*/
 [ProtoMember(3)]
private  string accountIdList;

/**
 所属用户iD
*/
 [ProtoMember(4)]
private  long? accountId;

 [ProtoMember(5)]
private  string createtime;

 [ProtoMember(6)]
private  string updatetime;

public long? Id
{

get {	 return id==-999?null:id ;	}

 set {	id = value;	}

}

public string Name
{

get {	 return name ;	}

 set {	name = value;	}

}

public string AccountIdList
{

get {	 return accountIdList ;	}

 set {	accountIdList = value;	}

}

public long? AccountId
{

get {	 return accountId==-999?null:accountId ;	}

 set {	accountId = value;	}

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

