using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversion : MonoBehaviour
{
    public Sprite cardColor;
    public int amount;

    private TileManager tileManager;

    private void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
    }

    public void Convert()
    {
        if (amount == 1)
        {
            tileManager.SetOption((int)GlobalEnums.Options.SwitchOne);
        }
    }
}
