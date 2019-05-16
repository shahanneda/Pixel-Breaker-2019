using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public static TileManager manager;

    [HideInInspector]
    public int X;
    [HideInInspector]
    public int Y;

    public float speed = 1f;

    public bool inAnimation = false;
    public bool isDead = false;

    [HideInInspector]
    public Vector2 shouldMoveTo = Vector2.positiveInfinity;
    [HideInInspector]
    public Sprite sprite;

    private Animator anim;
    public SpriteRenderer spriteRenderer;


    private bool _isFalling;
    public bool isFalling
    {

        get
        {
            return _isFalling;
        }
        set
        {
            _isFalling = value;
            //if(_isFalling)
            //{
            //    speed = 1f;
            //}else{
            //    speed = 1f;
            //}
        }

    }
    //THIS CLASS IS HERE JUST FOR THE BUTTON CLICKS, BUT TILES THEMSELVES SHOULD NOT HANDLE ANY ACTIONS, ISNTEAD
    //THEY SHOULD REPORT THAT MESSAGE TO THEIR MANAGER!
    void OnMouseDown()
    {
        manager.HandleTileClick(this);
    }
    public void Start()
    {
        if (!isDead)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            //spriteRenderer.color = manager.colorToSprite[Random.Range(0, manager.colorToSprite.Length - 1)].color;
            sprite = manager.sprites[Random.Range(0, manager.sprites.Length - 1)];
            spriteRenderer.sprite = sprite;
            anim = GetComponent<Animator>();
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
            isFalling = false;
        }
    }
    public void setIsDead()
    {
        isDead = true;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void OnMouseOver()
    {
        manager.HandleTileMouseOver(this);
    }

    public void OnMouseExit()
    {
        manager.HandleTileMouseExit(this);
    }

    public void setHover(bool state)
    {
        if (isDead)
            return;
        //anim.SetBool("isHover", state);

        if (state)
        {
            spriteRenderer.color = Color.gray;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public void setSelect(bool state)
    {
        anim.SetBool("Selected", state);
        print("Setting selection");
    }

    public bool getInSelect()
    {
        return !isDead && anim.GetBool("isHover");
    }
}
