using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [Tooltip("In degrees")] public int amount;

    private TileManager tileManager;

    private void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
    }

    public void SetRotation()
    {
        /*if (amount == -90)
        {
            tileManager.SetOption((int)GlobalEnums.Options.Rotate3x3Left90Degrees);
        }
        else if (amount == -180)
        {
            tileManager.SetOption((int)GlobalEnums.Options.Rotate3x3Left180Degrees);
        }
        else if (amount == 90)
        {
            tileManager.SetOption((int)GlobalEnums.Options.Rotate3x3Right90Degrees);
        }
        else if (amount == 180)
        {
            tileManager.SetOption((int)GlobalEnums.Options.Rotate3x3Right180Degrees);
        }*/

        switch (amount)
        {
            case -90:
                tileManager.SetOption((int)GlobalEnums.Options.Rotate3x3Left90Degrees);
                break;

            case -180:

                break;

            case 90:

                break;

            case 180:

                break;
        }
    }
}
