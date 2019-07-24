using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipBoard : Card
{
    public GlobalEnums.FlipOptions flipOption;

    public override void Start()
    {
        base.Start();
    }

    public void Flip()
    {
        tileManager.FlipBoard(flipOption);
    }
}
