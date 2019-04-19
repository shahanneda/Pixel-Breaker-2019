using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS CLASS IS HERE JUST FOR THE BUTTON CLICKS, BUT TILES THEMSELVES SHOULD NOT HANDLE ANY ACTIONS, ISNTEAD
//THEY SHOULD REPORT THAT MESSAGE TO THEIR MANAGER!
public class Tile : MonoBehaviour
{
    public static TileManager manager;

    [HideInInspector] public int X;
    [HideInInspector] public int Y;

    public float speed = 1f;

    public bool inAnimation = false;
    public bool isDead = false;

    [HideInInspector] public Vector2 shouldMoveTo = Vector2.positiveInfinity;
    [HideInInspector] public Color color;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public bool IsFalling { get; set; }

    void OnMouseDown()
    {
        manager.HandleTileClick(this);
    }

    public void Start()
    {
        if (!isDead)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = manager.colors[Random.Range(0, manager.colors.Length - 1)];
            color = spriteRenderer.color;
            animator = GetComponent<Animator>();
        }
    }

    public void Update()
    {
        if (inAnimation)
        {
            transform.position = Vector2.Lerp(transform.position, shouldMoveTo, speed * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, shouldMoveTo) < 0.01f)
        {
            inAnimation = false;
            IsFalling = false;
        }
    }

    public void SetIsDead()
    {
        isDead = true;
        spriteRenderer.enabled = false;
    }

    public void OnMouseOver()
    {
        if (isDead)
            return;

        animator.SetBool("isHover", true);
        spriteRenderer.sortingOrder = 999;

    }

    public void OnMouseExit()
    {
        if (isDead)
            return;

        animator.SetBool("isHover", false);
        spriteRenderer.sortingOrder = 0;
    }
}
