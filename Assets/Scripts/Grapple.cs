using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grapple : MonoBehaviour
{
    public Camera cam;

    public float grappleSpeed = 10;
    public float grappleStrength = 10;
    public float grappleDistance = 10;

    public Image indictor;
    public float indicatorLerpSpeed = 10;

    private Vector3 grapplePos;
    private bool grappling = false;

    private Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 hitPointSP = new Vector2(Screen.width / 2, Screen.height / 2);
        RaycastHit hit;
        indictor.color = Color.red;
        if (Physics.Raycast(transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, grappleDistance))
        {
            hitPointSP = cam.WorldToScreenPoint(hit.point);
            indictor.color = Color.green;
        }

        if (Input.GetMouseButtonDown(0)) //Left mouse button
        {
            if (hit.transform != null)
            {
                grapplePos = hit.point;
                grappling = true;
                rig.useGravity = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

        if(grappling)
        {
            indictor.color = Color.yellow;

            hitPointSP = (Vector2)cam.WorldToScreenPoint(grapplePos);

            Vector3 moveVec = ((grapplePos - transform.position)).normalized;
            moveVec += cam.transform.TransformDirection(Vector3.forward) * grappleSpeed;
            rig.AddForce(moveVec * grappleStrength * Time.deltaTime, ForceMode.VelocityChange);

            if(Vector3.Distance(transform.position, grapplePos) > grappleDistance)
            {
                StopGrapple(); 
            }
        }

        indictor.rectTransform.position = Vector2.Lerp(indictor.rectTransform.position, hitPointSP, Time.deltaTime * indicatorLerpSpeed);
    }

    void StopGrapple()
    {
        grappling = false;
        rig.useGravity = true;
    }
}
