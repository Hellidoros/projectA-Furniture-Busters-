using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndNode : BaseNode
{
    [Input] public int entry;
    public string eventName;

    public override string GetString()
    {
        return "End/" + eventName;
    }
}
