using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffDevlopments : IEqualityComparer<Devlopments>
{
    public bool Equals(Devlopments x, Devlopments y)
    {
        return x.id == y.id;
    }

    public int GetHashCode(Devlopments obj)
    {
        if (obj == null)
        {
            return 0;
        }
        else
        {
            return obj.ToString().GetHashCode();
        }
    }
}

public class DiffDevlopmentsStatus : IEqualityComparer<Devlopments>
{
    public bool Equals(Devlopments x, Devlopments y)
    {
        if (x.id == y.id)
        {
            return x.status == y.status;
        }
        else {
            return true;
        }
        //return x.id == y.id;
    }

    public int GetHashCode(Devlopments obj)
    {
        if (obj == null)
        {
            return 0;
        }
        else
        {
            return obj.ToString().GetHashCode();
        }
    }
}

