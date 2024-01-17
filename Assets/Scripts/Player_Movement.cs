using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float speed = 10f;
    public float speedJump = 20f;
    Vector2 move;
    Vector2 jump;

    private void Update()
    {

        Movement();
    }

    void Movement()
    {
        move = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        jump = new Vector2(0, Input.GetAxisRaw("Vertical"));

        transform.Translate(move * speed * Time.deltaTime );
        transform.Translate(jump * speedJump * Time.deltaTime);
    }
}
