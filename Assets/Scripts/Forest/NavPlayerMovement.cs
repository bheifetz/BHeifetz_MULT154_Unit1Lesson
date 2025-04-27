using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public float rotationSpeed = 60.0f;
    private Rigidbody playerRb = null;
    private float translationValue = 0;
    private float rotateValue = 0;
    private Animator animator;
    public bool isDead = false;
    private bool isAlert = false;
    private HivePickUp hivePickUp;
    public GameObject lookTarget;
    private Coroutine smoothLookCoroutine;
    private GameObject currentHazard;

    public delegate void DropHive(Vector3 position);
    public static event DropHive DroppedHive;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isDead)
            return;
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal");

        animator.SetFloat("speed", translation);

        translationValue = translation;
        rotateValue = rotation;

        if (Input.GetKeyDown(KeyCode.Space) && HivePickUp.pickedUp && !HivePickUp.dropped)
            DroppedHive.Invoke(transform.position);
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;
        // rotates the player
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y += rotateValue * rotationSpeed * Time.deltaTime;
        playerRb.MoveRotation(Quaternion.Euler(rot));
        
        // simply moves the player by however much the player is pressing with respect
        // to the speed parameter. Does not affect gravity.
        Vector3 move = transform.forward * translationValue;
        if (translationValue < 0)
            move = Vector3.zero;
        playerRb.velocity = new Vector3(move.x * speed, playerRb.velocity.y, move.z * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Hazard") && isDead == false)
        {
            isDead = true;
            animator.SetTrigger("died");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Hazard"))
        {
            if(smoothLookCoroutine != null)
                StopCoroutine(smoothLookCoroutine);
            isAlert = true;
            animator.SetBool("leftEarStandUp", isAlert);
            if (currentHazard == null)
                currentHazard = other.gameObject;
            lookTarget.transform.position = Vector3.Lerp(lookTarget.transform.position, currentHazard.transform.position, 0.08f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            currentHazard = null;
            smoothLookCoroutine = StartCoroutine("SmoothLookForward");
            isAlert = false;
            animator.SetBool("leftEarStandUp", isAlert);
        }
    }

    IEnumerator SmoothLookForward()
    {
        Vector3 targetPosition = transform.position + transform.forward * 10;
        while (Vector3.Distance(lookTarget.transform.position, targetPosition) > 0.1f)
        {
            targetPosition = transform.position + transform.forward * 10;
            lookTarget.transform.position = Vector3.Lerp(lookTarget.transform.position, targetPosition, 0.08f);
            yield return new WaitForSeconds(0.05f);
        }
        lookTarget.transform.position = targetPosition;
    }
}
