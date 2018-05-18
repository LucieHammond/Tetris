using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetriminoBehaviour : MonoBehaviour
{

    public Vector3 rotationPoint;

    private bool isActive = true;
    private bool leftMoveActive = true;
    private bool rightMoveActive = true;
    private float timeLeftPressed = 0f;
    private float timeRightPressed = 0f;

    // Use this for initialization
    void Start()
    {
        if (isActive)
        {
            //StartCoroutine(Fall());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
            MoveWithInputs();
    }

    private void MoveWithInputs()
    {
        if (leftMoveActive)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                transform.Translate(-1, 0, 0, Space.World);
            else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                timeLeftPressed += Time.deltaTime;
                if (timeLeftPressed > 0.5)
                {
                    transform.Translate(-1, 0, 0, Space.World);
                    timeLeftPressed -= 0.1f;
                }
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
                timeLeftPressed = 0f;
        }

        if (rightMoveActive)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                transform.Translate(1, 0, 0, Space.World);
            else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                timeRightPressed += Time.deltaTime;
                if (timeRightPressed > 0.5)
                {
                    transform.Translate(1, 0, 0, Space.World);
                    timeRightPressed -= 0.1f;
                }
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
                timeRightPressed = 0f;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
            transform.RotateAround(transform.position + transform.rotation * rotationPoint, Vector3.back, 90f);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            transform.Translate(0, -1, 0, Space.World);
    }

    private IEnumerator Fall()
    {
        WaitForSeconds timeWaiter = new WaitForSeconds(0.5f);
        while (true)
        {
            yield return timeWaiter;
            transform.Translate(0, -1, 0, Space.World);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.name == "Border Left")
            leftMoveActive = false;
        else if (collision.collider.name == "Border Right")
            rightMoveActive = false;
        else if (collision.collider.name == "Border Bottom")
        {
            StopAllCoroutines();
            isActive = false;
        }
        else
        {
            Vector3 selfCollider = collision.otherCollider.transform.position;
            Vector3 obstacleCollider = collision.collider.transform.position;
            Vector3 diff = obstacleCollider - selfCollider;

            if (Vector3.Distance(diff, Vector3.right) <= 0.05)
                rightMoveActive = false;
            else if (Vector3.Distance(diff, Vector3.left) <= 0.05)
                leftMoveActive = false;
            else if (Vector3.Distance(diff, Vector3.down) <= 0.05)
            {
                StopAllCoroutines();
                isActive = false;
            }
			else if (Vector3.Distance(diff, new Vector3(1, 1, 0)) <= 0.05)
				rightMoveActive = true;
			else if (Vector3.Distance(diff, new Vector3(-1, 1, 0)) <= 0.05)
                leftMoveActive = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Border Left")
            leftMoveActive = true;
        else if (collision.collider.name == "Border Right")
            rightMoveActive = true;
        else if (collision.collider.name == "Border Bottom")
            Debug.Log("/!\\ Border Bottom Exit");
        else
        {
            Vector3 selfCollider = collision.otherCollider.transform.position;
            Vector3 obstacleCollider = collision.collider.transform.position;
            Vector3 diff = obstacleCollider - selfCollider;
			Debug.Log("x : " + diff.x + ", y : " + diff.y);
            
			if (diff.x > 1.5)
                rightMoveActive = true;
			else if (diff.x < 1.5)
                leftMoveActive = true;
            else if (diff.y < -1.5)
            {
                Debug.Log("/!\\ Bottom Obstacle Exit");
            }
        }
    }

}
