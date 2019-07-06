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
        if (amount == -90)
        {
            tileManager.SetOption((int)GlobalEnums.Options.Rotate3x3Left90Degrees);
        }
    }
}
