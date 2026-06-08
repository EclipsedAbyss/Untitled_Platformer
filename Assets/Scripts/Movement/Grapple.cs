using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private GameObject playerCamPosition;
    [SerializeField] private BaseMovement playerMovementScript;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private float range;
    [SerializeField] private float grappleDelay;
    [SerializeField] private float grappleSpeed;//stores the grapples speed
    [SerializeField] private LineRenderer cable;
    [SerializeField] private float coolDown;
    [HideInInspector] public float coolDownStored;
    private Vector3 endPoint;//stores the players ending position
    private float step;//used for grapple calculations
    public KeyCode grapple = KeyCode.Mouse2;
    public float gapValue;
    public bool grapplingNow;
    private bool clinging = false;
    private RaycastHit hit;

    private void Start()
    {
        cable.positionCount = 1;
        coolDownStored = coolDown;
    }

    private void Update()
    {
        cable.SetPosition(0, transform.position);

        step = grappleSpeed * Time.deltaTime;

        if (Input.GetKeyDown(grapple) && clinging == false)
        {
            GrappleStart();
        }

        if (coolDownStored < coolDown)
        {
            coolDownStored += Time.deltaTime;
        }

    }
    private void GrappleCling()
    {
        if(Vector3.Distance(player.transform.position, endPoint) < 3)
        {
            Invoke(nameof(GrappleCling), 0.01f);
            
            
        }
        else
        {
            if (hit.transform.GetComponent<SlamPad>() == null)
            {
                coolDownStored = 0;
            }
            Debug.Log("playerMoved");
            clinging = false;
            cable.positionCount = 1;
            playerRB.useGravity = true;
        }

    }
    private void Grappling()
    {
        if (Vector3.Distance(player.transform.position, endPoint) > 2 && grapplingNow)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, endPoint, step);//moves the Player towards the end point.
            playerCam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z);

            Invoke(nameof(Grappling), 0f);
        }
        else if (!grapplingNow && Vector3.Distance(player.transform.position, endPoint) > 3)
        {
            cable.positionCount = 1;
            playerCam.transform.position = playerCamPosition.transform.position;
            playerRB.useGravity = true;
            coolDownStored = 0;
            playerRB.AddForce(transform.forward * 30f, ForceMode.Impulse);
            grapplingNow = false;
            playerMovementScript.freeMove = true;
        }
        else
        {
            playerRB.linearVelocity = Vector3.zero;
            playerCam.transform.position = playerCamPosition.transform.position;
            grapplingNow = false;
            playerMovementScript.freeMove = true;
            if (hit.transform.GetComponent<SlamPad>() == null)
            {
                clinging = true;
            }
            GrappleCling();
        }
    }

    private void GrappleStart()
    {

        if (Physics.Raycast(playerCam.gameObject.transform.position + playerCam.transform.forward, playerCam.transform.forward, out hit, range) && !grapplingNow && coolDownStored >= coolDown)
        {
            cable.positionCount = 2;
            endPoint = hit.point;
            cable.SetPosition(1, endPoint);
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
