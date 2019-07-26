using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipArea : Card
{
    public GlobalEnums.Options option;

    public override void Start()
    {
        base.Start();
    }

    public void SetOption()
    {
        tileManager.SetOption((int)option);
    }
}
