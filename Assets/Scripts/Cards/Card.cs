using UnityEngine;
using System.Collections;
/// <summary>
/// All cards should inherit from this class which sets up the click animation.
/// </summary>
public class Card : MonoBehaviour
{

    public Animator animator;

    public virtual void Start(){
        animator = GetComponent<Animator>();
    }
    public void SetSelect(bool state){
        animator.SetBool("isSelected", state);
    }

}
