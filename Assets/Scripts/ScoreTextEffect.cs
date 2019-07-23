using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextEffect : MonoBehaviour
{
    public Text text;
    public Animator animator;

    public void DisplayScore(int score)
    {
        transform.position = Input.mousePosition;

        text.text = "+" + score;
        animator.Play("DisplayScore");
    }
}
