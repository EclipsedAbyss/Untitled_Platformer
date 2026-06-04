using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private BaseMovement playerMovementScript;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private float range;
    [SerializeField] private float grappleSpeed;//stores the grapples speed
    private Vector3 position;//stores the players current position
    private Vector3 endPoint;//stores the players ending position
    private float step;//used for grapple calculations
    public KeyCode grapple = KeyCode.Mouse2;
    public float coolDown;
    public float gapValue;
    private bool grapplingNow;
    private RaycastHit hit;
    [HideInInspector] public float coolDownStored;

    private void Start()
    {

    }

    private void Update()
    {
        step = grappleSpeed * Time.deltaTime;

        if (Input.GetKeyDown(grapple))
        {
            if (Physics.Raycast(playerCam.gameObject.transform.position, playerCam.transform.forward, out hit, range) && !grapplingNow)
            {
                endPoint = hit.point;
                grapplingNow = true;
                playerMovementScript.freeMove = false;
                playerRB.useGravity = false;
                playerRB.linearVelocity = Vector3.zero;
                Grappling();
            }
            else if (grapplingNow)
            {
                grapplingNow = false;
            }
            else
            {
                Debug.Log("you missed");
            }


      
        }
    }

    private void Grappling()
    {
        if (Vector3.Distance(player.transform.position, endPoint) > 2 && grapplingNow)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, endPoint, step);//moves the Player towards the end point.
            Invoke(nameof(Grappling), 0.01f);
        }
        else if (!grapplingNow && Vector3.Distance(player.transform.position, endPoint) < 1)
        {
            playerRB.useGravity = true;
            grapplingNow = false;
            playerRB.AddForce(transform.forward * 50, ForceMode.Impulse);
            playerMovementScript.freeMove = true;
        }
        else
        {
            playerRB.useGravity = true;
            grapplingNow = false;
            playerMovementScript.freeMove = true;
        }





    }


        //private IEnumerator WallGrapple()
        //{
        //    if (targetPos != null && player.transform.position != targetPos.position)
        //    {
        //        player.transform.position += targetPos.position / (gapValue * Vector3.Distance(player.transform.position, targetPos.position));

        //        if (Input.GetKeyUp (grapple))
        //        {
        //            targetPos = null;
        //            StopCoroutine(WallGrapple());
        //        }


        //    }
        //    else
        //    {
        //        StopCoroutine(WallGrapple());
        //    }

        //        yield return targetPos = null;
        //}

        //private void EnemyGrapple()
        //{

        //}
        //This is stupid dumb and bad. I'm leaving this here as a reminder for myself to never code this bullshit again, theres much better ways to do this.
    
}
