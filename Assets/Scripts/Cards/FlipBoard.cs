using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipBoard : Card
{
    public GlobalEnums.FlipOptions flipOption;
    private TileManager tileManager;

    public override void Start()
    {
        base.Start();
        tileManager = FindObjectOfType<TileManager>();
    }

    public void Flip()
    {
        tileManager.FlipBoard(flipOption);
    }
}
