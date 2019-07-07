using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translation : MonoBehaviour
{
    public int amount;

    private TileManager tileManager;

    private void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
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
    }
}
