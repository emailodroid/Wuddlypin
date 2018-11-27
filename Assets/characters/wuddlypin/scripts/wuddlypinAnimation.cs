using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wuddlypinAnimation : MonoBehaviour {

    private Animator wuddlyAnimator;
    private wuddlypinMovement wuddly;
    private string animation;

    private bool idled;

    // Use this for initialization
    void Start () {
        wuddlyAnimator = GetComponent<Animator>();
        wuddly = GetComponent<wuddlypinMovement>();

        animation = "idle";
        idled = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (wuddly.is_grounded() == true)
        {
            if(idled == false)
            {
                animation = "landed";
            } else
            {
                animation = "idle";
            }

            if(this.wuddlyAnimator.GetCurrentAnimatorStateInfo(0).IsName("landed") && (this.wuddlyAnimator.GetCurrentAnimatorStateInfo(0).length - this.wuddlyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime) < -1 )
            {
                idled = true;
                animation = "idle";
            }
        }

        if (Mathf.Abs(wuddly.get_velocity_x()) > 0.1)
        {
            if(Mathf.Abs(wuddly.get_velocity_x()) > 7.5)
            {
                idled = false;
            }
            animation = "run";
        }

        if(wuddly.is_grounded() == false)
        {
            idled = false;

            if (wuddly.get_velocity_y() > 0)
            {
                animation = "jumpStart";
            }
            else
            {
                animation = "jumpFalling";
            }
        }


        animate(animation);
    }

    IEnumerator onComplete()
    {

        while (this.wuddlyAnimator.GetCurrentAnimatorStateInfo(0).IsName("landed"))
        {
            Debug.Log("Animation in progress!");
            yield return null;
        }

        // this will get here when kickedInHead is false
        Debug.Log("Animation complete!");
    }

    private IEnumerator WaitForAnimation(Animation animation)
    {
        do
        {
            yield return null;
        } while (animation.isPlaying);
    }

    private void animate(string a)
    {
        this.wuddlyAnimator.Play(a);
    }

    public void set_anim(string a)
    {
        this.animation = a;
    }
}
