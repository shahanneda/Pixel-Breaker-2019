using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : Card
{
    [Tooltip("In degrees")] public int amount;

    public override void Start()
    {
        base.Start();
    }

    public void SetRotation()
    {
        switch (amount)
        {
            case -90:
                tileManager.SetOption((int)GlobalEnums.Options.Rotate3x3Left90Degrees);
                break;

            case -180:
                tileManager.SetOption((int)GlobalEnums.Options.Rotate3x3Left180Degrees);
                break;

            case 90:
                tileManager.SetOption((int)GlobalEnums.Options.Rotate3x3Right90Degrees);
                break;

            case 180:
                tileManager.SetOption((int)GlobalEnums.Options.Rotate3x3Right180Degrees);
                break;
        }
    }
}
