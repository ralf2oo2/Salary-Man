using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData : MonoBehaviour
{
    [SerializeField] GameObject bulletHoleContainer;
    [SerializeField] GameObject bulletHolePrefab;
    [SerializeField] Rigidbody rb;
    [SerializeField] float maxLifetime;
    [SerializeField] float damage;

    private Vector3 shootDirection = Vector3.zero;

    [SerializeField] LayerMask obstructionMask;
    public bool isBase = true;

    public void SetShootDirection(Vector3 direction)
    {
        shootDirection = direction;
    }
    
    private void Setup()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        maxLifetime -= Time.deltaTime;
        if (maxLifetime < 0)
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        if(!isBase)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.collider.CompareTag("Bullet")) return;
        if(collision.gameObject.GetComponent<Health>() != null)
        {
            Health health = collision.gameObject.GetComponent<Health>();
            health.health -= damage;
            Debug.Log("hit someone " + health);
        }
        else
        {

            /*Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100);*/
            Vector3 collisionNormal = collision.GetContact(0).normal;
            Vector3 collisionPoint = collision.GetContact(0).point;

            GameObject bulletHole = Instantiate(bulletHolePrefab, collisionPoint - (collisionNormal * 0.2f), Quaternion.LookRotation(collisionNormal, Vector3.up) * bulletHolePrefab.transform.rotation);

            bulletHole.transform.SetParent(collision.gameObject.transform);
            bulletHole.transform.Rotate(Vector3.forward, Random.Range(0f, 360f));

            Destroy(bulletHole, 30);
        }
        DestroyBullet();
    }
}
