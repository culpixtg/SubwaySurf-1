using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float swipeAmount = 200f;

    public float speed;
    public float jumpSpeed;
    public float swipeSpeed = 15f;
    public float moveXPos = 3f;

    private Rigidbody rb;
    private Vector3 touchStartPos;
    private Vector3 touchEndPos;
    private bool swipeLeft = false;
    private bool swipeRight = false;
    private bool isJumping = false;
    private bool swipeDown = false;
    private Vector3 targetPos;
    private float tempYPos;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        tempYPos = transform.position.y;
    }


    void Update()
    {
        Move();
        Swipe();
        
        Vector3 temp = transform.position;
        temp.x = Mathf.Clamp(temp.x, moveXPos * -1, moveXPos);
        transform.position = temp;

        if (transform.position.y > tempYPos) isJumping = true;


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) touchStartPos = touch.position;
            if (touch.phase == TouchPhase.Ended)
            {
                touchEndPos = touch.position;


                
                if (!swipeRight && touchEndPos.x < touchStartPos.x && Mathf.Abs(touchEndPos.x - touchStartPos.x) > swipeAmount) // swiping left
                {
                    if (transform.position.x > moveXPos * -1)
                    {
                        swipeLeft = true;
                        targetPos = new Vector3(transform.position.x - moveXPos, transform.position.y, transform.position.z);
                    }
                }
                if (!swipeLeft && touchEndPos.x > touchStartPos.x && Mathf.Abs(touchEndPos.x - touchStartPos.x) > swipeAmount) // swiping right
                {
                    if (transform.position.x < moveXPos)
                    {
                        swipeRight = true;
                        targetPos = new Vector3(transform.position.x + moveXPos, transform.position.y, transform.position.z);
                    }
                }

                if (touchEndPos.y > touchStartPos.y && Mathf.Abs(touchEndPos.y - touchStartPos.y) > swipeAmount) // Swiping up
                {
                    if (transform.position.y < 1)
                    {
                        rb.AddForce(Vector2.up * jumpSpeed, ForceMode.Impulse);
                    }
                }
                if (touchEndPos.y < touchStartPos.y && Mathf.Abs(touchEndPos.y - touchStartPos.y) > swipeAmount) // Swiping down
                {
                    swipeDown = true;
                }

            }
        }

    }

    private void Move()
    {
        transform.position += new Vector3(0, 0, speed * Time.deltaTime);
    }

    private void Swipe()
    {
        if (swipeLeft)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, targetPos.y, transform.position.z), swipeSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, new Vector3(targetPos.x, targetPos.y, transform.position.z)) < 0.01f) swipeLeft = false;
        }
        if (swipeRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, targetPos.y, transform.position.z), swipeSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, new Vector3(targetPos.x, targetPos.y, transform.position.z)) < 0.01f) swipeRight = false;
        }
    }

}
