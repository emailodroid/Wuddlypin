﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class wuddlypinMovement : MonoBehaviour {

    private Rigidbody2D RIGID;

    public enum STATE
    {
        ACTIVE,
        LOCKED,
        DEAD
    }

    public enum DIRECTION
    {
        LEFT = -1,
        RIGHT = 1
    }
    private DIRECTION FACING;
    private STATE MY_STATE;

    public float JUMP_FORCE;
    public bool GROUNDED;

    public bool IS_RUNNING = false;
    public bool CAN_RUN    = true;
    private bool IS_JUMPING = false;

    public float MAX_MOVEMENT_SPEED;
    public float VELOCITY_X;
    public float ACCELERATION_X;
    public float DRAG_X;
    public float VELOCITY_Y;

    //CONTROLS
    public static KeyCode LEFT = KeyCode.LeftArrow;
    public static KeyCode RIGHT = KeyCode.RightArrow;
    public static KeyCode JUMP = KeyCode.UpArrow;
    public static KeyCode WHACK = KeyCode.S;

    // Use this for initialization
    void Start ()
    {
        RIGID = GetComponent<Rigidbody2D>();
        FACING = DIRECTION.LEFT;
        ACCELERATION_X = 2f;
        DRAG_X = ACCELERATION_X / 5f;
        MAX_MOVEMENT_SPEED = 8f;
        VELOCITY_X = 0f;
        JUMP_FORCE = 12f;
        GROUNDED = false;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        jump();
    }

    void FixedUpdate()
    {
        RIGID.velocity = new Vector2(VELOCITY_X, RIGID.velocity.y);
    }

    private void jump()
    {
        if (GROUNDED)
        {
            if (Input.GetKeyDown(JUMP))
            {
                IS_JUMPING = true;
                RIGID.velocity = new Vector2(RIGID.velocity.x, JUMP_FORCE);
            }
        }
        else
        {
            if (RIGID.velocity.y > 0)
            {
                if (Input.GetKeyUp(JUMP))
                {
                    RIGID.velocity = new Vector2(RIGID.velocity.x, RIGID.velocity.y / 2f);
                }
            }
        }
    }

    private void movement()
    {
        VELOCITY_X += ACCELERATION_X * controllInput();

        if (Mathf.Abs(VELOCITY_X) > MAX_MOVEMENT_SPEED)
        {
            VELOCITY_X = (int)Mathf.Sign(VELOCITY_X) * MAX_MOVEMENT_SPEED;
        }

        if (VELOCITY_X != 0)
        {
            int TEMP_SIGN = (int)Mathf.Sign(VELOCITY_X);
            VELOCITY_X -= TEMP_SIGN * DRAG_X;
            if ((int)Mathf.Sign(VELOCITY_X) != TEMP_SIGN)
            {
                VELOCITY_X = 0f;
            }
        }
    }

    public void flip()
    {
        Vector2 SCALE = transform.localScale;
        SCALE = new Vector2(SCALE.x * -1, SCALE.y);
        transform.localScale = SCALE;
    }

    private int controllInput()
    {
        int KEY = 0;
        
        if (Input.GetKey(RIGHT))
        {
            KEY = 1;
            if (FACING != DIRECTION.RIGHT)
            {
                FACING = DIRECTION.RIGHT;
                flip();
            }
        }
        if (Input.GetKey(LEFT)) {
            KEY = -1;
            if (FACING != DIRECTION.LEFT)
            {
                FACING = DIRECTION.LEFT;
                flip();
            }
        }

        return KEY;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        string collision_tag = collision.gameObject.tag;

        if (collision_tag.Contains("Wall"))
        {
            foreach (ContactPoint2D p in collision.contacts)
            {
                if (p.normal.x > 0)
                {
                    if (VELOCITY_X < 0)
                    {
                        VELOCITY_X = 0f;
                    }
                }
                if (p.normal.x < 0)
                {
                    if (VELOCITY_X > 0)
                    {
                        VELOCITY_X = 0f;
                    }
                }
            }
        }
        if (collision_tag.Contains("Ground"))
        {
            foreach (ContactPoint2D p in collision.contacts)
            {

                if (p.normal.y > 0)
                {
                    GROUNDED = true;
                    IS_JUMPING = false;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        string collision_tag = collision.gameObject.tag;

        if (collision_tag.Contains("Wall"))
        {

        }
        if (collision_tag.Contains("Ground"))
        {
            GROUNDED = false;
        }

    }

    public STATE get_state()
    {
        return this.MY_STATE;
    }

    public void set_state(STATE st)
    {
        this.MY_STATE = st;
    }

    public float get_velocity_x()
    {
        return this.VELOCITY_X;
    }

    public void set_velocity_x(float val)
    {
        this.VELOCITY_X = val;
    }

    public float get_velocity_y()
    {
        return RIGID.velocity.y;
    }

    public void set_velocity_y(float val)
    {
        this.VELOCITY_Y = val;
    }

    public DIRECTION get_facing()
    {
        return this.FACING;
    }

    public bool is_jumping()
    {
        return this.IS_JUMPING;
    }

    public bool is_grounded()
    {
        return this.GROUNDED;
    }

}
