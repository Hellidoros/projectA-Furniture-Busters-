using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XNode;

public class DialogueNode : BaseNode {

    [Input] public int entry;
    [Output] public int exit;
    public string speakerName;
    public string dialogueLine;
    public string eventName;
    public Sprite sprite;
    public string color;

    public override string GetString()
    {
        return "DialogueNode/" + speakerName + "/" + dialogueLine + "/" + eventName;
    }

    public override Sprite GetSprite()
    {
        return sprite;
    }

    public override string GetColor()
    {
        return color;
    }
}