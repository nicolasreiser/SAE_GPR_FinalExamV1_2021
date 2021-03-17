using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeDropInstance : MonoBehaviour, IDropOwner
{
    private Drop drop;




    public Drop GetDrop()
    {
        return drop;
    }

    public void SetDrop(Drop drop)
    {
        this.drop = drop;
    }
}
