﻿using UnityEngine;

public class RopeController : MonoBehaviour
{
    [SerializeField] private float shootRange = 100;
    [SerializeField] private GameObject anchorGO;
    private Rigidbody anchorRB;
    private ConfigurableJoint joint;

    private void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        anchorRB = anchorGO.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && joint == null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity /*shootRange*/ ) && hit.collider)
            {
                anchorGO.transform.position = hit.point;
                anchorGO.GetComponentInChildren<RopeMeshController>().SetPlayerStartPos();

                if (hit.transform.GetComponent<MovingObject>())
                {
                    anchorGO.transform.parent = hit.transform; //TODO: dont let the rope get longer when object pulls away from player
                }
                else
                {
                    anchorGO.transform.parent = null;
                }

                anchorGO.SetActive(true);
                gameObject.AddComponent<ConfigurableJoint>();
                joint = GetComponent<ConfigurableJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.axis = Vector3.zero;
                joint.anchor = Vector3.zero;
                joint.secondaryAxis = Vector3.zero;
                joint.connectedAnchor = Vector3.zero;
                joint.xMotion = ConfigurableJointMotion.Limited;
                joint.yMotion = ConfigurableJointMotion.Limited;
                joint.zMotion = ConfigurableJointMotion.Limited;
                SoftJointLimit softJointLimit = new SoftJointLimit();
                softJointLimit.limit = Vector3.Distance(anchorGO.transform.position, transform.position);
                joint.linearLimit = softJointLimit;
                joint.connectedBody = anchorRB;
            }
        }

        if (Input.GetMouseButton(0) && joint != null) //While held down the rope can get shorter, but not longer 
        {
            SoftJointLimit softJointLimit = new SoftJointLimit();
            softJointLimit.limit = Vector3.Distance(anchorGO.transform.position, transform.position);
            joint.linearLimit = softJointLimit;

            //TODO: Maybe shrink over time or with mouse wheel manually?
        }

        if (Input.GetMouseButtonUp(0))
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        Destroy(joint);
        anchorGO.SetActive(false);
    }
}