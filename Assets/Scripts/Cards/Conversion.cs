using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversion : Card
{
    public Sprite cardColor;
    public int amount;

    public override void Start()
    {
        base.Start();
    }

    public void Convert()
    {
        if (amount == 1)
        {
            tileManager.SetOption((int)GlobalEnums.Options.SwitchColorOfOne);
        }
    }
}
