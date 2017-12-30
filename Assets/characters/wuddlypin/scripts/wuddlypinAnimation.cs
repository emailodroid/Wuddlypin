using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wuddlypinAnimation : MonoBehaviour {

    private Animator wuddlyAnimator;
    private wuddlypinMovement wuddly;
    private string animation;

    // Use this for initialization
    void Start () {
        wuddlyAnimator = GetComponent<Animator>();
        wuddly = GetComponent<wuddlypinMovement>();

        animation = "idle";
    }
	
	// Update is called once per frame
	void Update () {

        animation = "idle";

        if (wuddly.get_velocity_x() != 0)
        {
            animation = "run";
        }

        Debug.Log(wuddly.get_velocity_y());

        if(wuddly.is_grounded() == false)
        {
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

    private void animate(string a)
    {
        this.wuddlyAnimator.Play(a);
    }

    public void set_anim(string a)
    {
        this.animation = a;
    }
}
