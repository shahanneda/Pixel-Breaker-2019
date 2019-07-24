using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translation : Card
{
    public int amount;

    public override void Start()
    {
        base.Start();
    }

    public void Translate()
    {
        if (amount == 1)
        {
            tileManager.SetOption((int)GlobalEnums.Options.TranslateOneTile);
        }
        else if (amount == 3)
        {
            tileManager.SetOption((int)GlobalEnums.Options.ThreeByThreeSwitch);
        }
        else if (amount == -1)
        {
            tileManager.SetOption((int)GlobalEnums.Options.SwitchRows);
        }
        else if (amount == -2)
        {
            tileManager.SetOption((int)GlobalEnums.Options.SwitchColumns);
        }
    }
}
