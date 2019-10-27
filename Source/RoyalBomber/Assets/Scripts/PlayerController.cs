using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 5.0f;
    public float shotPower = 500f;
    public float rechargeSpeed = 1f;

    private Rigidbody playerRigidbody;
    public GameObject bombPrefab;
    public Transform gun;

    private bool readyToShot = true;

    private bool playable = true;
    public bool Playable { get => playable; }

    public GameObject quadExplosionPrefab;
    public GameObject sparksColissionPrefab;
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void MoveCharacter(Vector3 direction)
    {
        if (playable)
        {
            var tempDirection = new Vector3(direction.x, 0, direction.z);
            playerRigidbody.AddForce(tempDirection * moveSpeed * Time.deltaTime);
            if (playerRigidbody.velocity != Vector3.zero)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(tempDirection), Time.deltaTime * rotateSpeed);
            }
        }
    }
    public void RotateCharacter(Vector3 direction)
    {
        if (playable)
        {
            transform.Rotate(direction);
        }
    }

    public void Fire()
    {
        if (readyToShot && playable)
        {
            Transform bomb = Instantiate(bombPrefab, gun.position, gun.rotation).transform;
            bomb.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, shotPower));
            readyToShot = false;
            StartCoroutine(Recharge());
        }
    }

    IEnumerator Recharge()
    {
        yield return new WaitForSeconds(rechargeSpeed);
        readyToShot = true;
    }

    public EventHandler<PlayerController> OnCharacterLeaveField;
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "GameField")
        {
            playable = false;
            transform.GetComponent<Rigidbody>().freezeRotation = false;
            OnCharacterLeaveField?.Invoke(this, this);
            StartCoroutine(DestroyTimer());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Instantiate(sparksColissionPrefab, contact.point, Quaternion.identity);
            }
        }
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(5f);
        Instantiate(quadExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
