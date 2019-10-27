using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float timeToExplosion = 5f;
    public float radius = 5.0F;
    public float power = 10.0F;
    public GameObject explosionParticlePrefab;
    void Start()
    {
        StartCoroutine(ExplosionTimer());
    }

    IEnumerator ExplosionTimer()
    {
        yield return new WaitForSeconds(timeToExplosion);
        Explosion();
    }

    void Explosion()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 0F, ForceMode.Force);
        }
        Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);// Анимация взрыва
        Destroy(gameObject);
    }
}
