using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    //Max Distance from wall to wall run
    public float wallMaxDist = 0.5f;
    //Speed mult for running on all
    public float wallSpeed = 1.2f;
    //min height to be attached to wall
    public float minHeight = 1.2f;
    //roll camera for running on wall
    public float maxAngleRoll = 20;

    [Range(0.0f, 1.0f)]
    public float normalizedAngleThreashold = 0.1f;

    //Cooldown for reattaching to a wall
    public float jumpDuration = 1;
    //Angle to jump off a wall at
    public float wallBounce = 3;
    //Wall gravity
    public float wallGravity = 20f;

    private Vector3[] directions;
    private RaycastHit[] hits;

    PlayerMovement playerMovement;

    private bool isWallRunning = false;
    private Vector3 lastWallPosition;
    private Vector3 lastWallNormal;
    private float timeSinceJump = 0;
    public float timeSinceWallAttach = 0;
    public float timeSinceWallDetach = 0;
    private bool isJumping;
    bool IsGrounded() => playerMovement.isGrounded;

    public bool IsWallRunning() => isWallRunning;

    public bool CanWallRun()
    {
        return !IsGrounded();
    }

    public bool VerticalCheck()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minHeight);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        directions = new Vector3[]
        {
            Vector3.right,
            Vector3.right + Vector3.forward,
            Vector3.forward,
            Vector3.left,
            Vector3.left + Vector3.forward
        };
    }

    // Update is called once per frame
    void LateUpdate()
    {
        isWallRunning = false;

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        if (CanAttach())
        {
            hits = new RaycastHit[directions.Length];

            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 dir = transform.TransformDirection(directions[i]);
                Physics.Raycast(transform.position, dir, out hits[i], wallMaxDist);
                if (hits[i].collider != null)
                {
                    Debug.DrawRay(transform.position, dir * hits[i].distance, Color.green);
                }
                else
                {
                    Debug.DrawRay(transform.position, dir * wallMaxDist, Color.red);
                }
            }
        }

        if (CanWallRun())
        {
            hits =hits.ToList().Where(h => h.collider != null).OrderBy(h => h.distance).ToArray();
            if(hits.Length > 0)
            {
                if(hits[0].transform.gameObject.layer == LayerMask.NameToLayer("WallRun")){
                    OnWall(hits[0]);
                    lastWallPosition = hits[0].point;
                    lastWallNormal = hits[0].normal;

                }
            }
        }

        if (isWallRunning)
        {
            timeSinceWallDetach = 0;
            timeSinceWallAttach = +Time.deltaTime;
            playerMovement.move += Vector3.down * wallGravity * Time.deltaTime;
        }
        else
        {
            timeSinceWallAttach = 0;
            timeSinceWallDetach += Time.deltaTime;
        }
        
    }

    private void OnWall(RaycastHit hit)
    {
        float d = Vector3.Dot(hit.normal, Vector3.up);
        if(d >= -normalizedAngleThreashold && d <= normalizedAngleThreashold)
        {
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 alongWall = transform.TransformDirection(Vector3.forward);

            playerMovement.move = alongWall * vertical * wallSpeed;
            isWallRunning = true;
        }
    }

    private bool CanAttach()
    {
        if (isJumping)
        {
            timeSinceJump += Time.deltaTime;
            if (timeSinceJump > jumpDuration)
            {
                timeSinceJump = 0;
                isJumping = false;
            }
            return false;
        }
        return true;
    }
}
