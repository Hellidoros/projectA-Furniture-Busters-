using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XNode;

public class BaseNode : Node {

    public virtual string GetString()
    {
        return null;
    }

    public virtual Sprite GetSprite()
    {
        return null;
    }

    public virtual string GetColor()
    {
        return null;
    }
}