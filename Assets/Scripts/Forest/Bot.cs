///
/// Code modified from https://learn.unity.com/tutorial/hide-h1zl/?courseId=5dd851beedbc2a1bf7b72bed&projectId=5e0b9220edbc2a14eb8c9356&tab=materials&uv=2019.3#
/// Author: Penny de Byl
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    public GameObject[] hidingSpots;
    private Rigidbody rbTarget;

    //public Mode aiMode;

    //public enum Mode
    //{
    //    SEEK,
    //    FLEE,
    //    PURSUE,
    //    EVADE,
    //    WANDER
    //}


    float currentSpeed
    {
        get { return agent.velocity.magnitude; }
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        rbTarget = target.GetComponent<Rigidbody>();
    }

    public void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    public void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }

    public void Pursue()
    {
        //Vector3 targetLocation = target.transform.position + rbTarget.velocity;
        //Seek(targetLocation);
        
        Vector3 targetDir = target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, target.transform.forward);

        float toTarget = Vector3.Angle(this.transform.forward, targetDir);

        if ((toTarget > 90 && relativeHeading < 20) || rbTarget.velocity.magnitude < 0.01f)
        {
            Seek(target.transform.position);
            return;
        }

        float lookAhead = targetDir.magnitude / (agent.speed + rbTarget.velocity.magnitude);
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    public void Evade()
    {
        //Vector3 targetLocation = target.transform.position + rbTarget.velocity;
        //Flee(targetLocation);

        Vector3 targetDir = target.transform.position - this.transform.position;
        float lookAhead = targetDir.magnitude / (agent.speed + rbTarget.velocity.magnitude);
        Flee(target.transform.position + target.transform.forward * lookAhead);
    }


    Vector3 wanderTarget = Vector3.zero;
    public void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = Random.Range(-10.0f, 10.0f);
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                    0,
                                    Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        //Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(this.transform.position + targetLocal);
    }

    void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for (int i = 0; i < hidingSpots.Length; i++)
        {
            Vector3 hideDir =hidingSpots[i].transform.position - target.transform.position;
            Vector3 hidePos = hidingSpots[i].transform.position + hideDir.normalized * 10;

            if (Vector3.Distance(this.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(this.transform.position, hidePos);
            }
        }

        Seek(chosenSpot);
    }

    void CleverHide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = hidingSpots[0];

        for (int i = 0; i < hidingSpots.Length; i++)
        {
            Vector3 hideDir =hidingSpots[i].transform.position - target.transform.position;
            Vector3 hidePos = hidingSpots[i].transform.position + hideDir.normalized * 100;

            if (Vector3.Distance(this.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = hidingSpots[i];
                dist = Vector3.Distance(this.transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 250.0f;
        hideCol.Raycast(backRay, out info, distance);


        Seek(info.point + chosenDir.normalized);

    }

    public bool CanSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - this.transform.position;
        if (Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject.tag == "Player")
                return true;
        }
        return false;
    }

    public bool CanTargetSeeMe()
    {
        RaycastHit raycastInfo;
        Vector3 targetFwdWS = target.transform.TransformDirection(target.transform.forward);
        Debug.DrawRay(target.transform.position, targetFwdWS * 10);
        Debug.DrawRay(target.transform.position, target.transform.forward * 10, Color.green);
        if (Physics.Raycast(target.transform.position, target.transform.forward, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject == gameObject)
                return true;
        }
        return false;
    }

    //void Update()
    //{
    //    switch(aiMode)
    //    {
    //        case Mode.SEEK:
    //            Seek(target.transform.position);
    //            break;
    //        case Mode.FLEE:
    //            Flee(target.transform.position);
    //            break;
    //        case Mode.PURSUE:
    //            Pursue();
    //            break;
    //        case Mode.EVADE:
    //            Evade();
    //            break;
    //    }
    //}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && collision.gameObject.GetComponent<NavPlayerMovement>().isDead)
            agent.isStopped = true;
    }
}
