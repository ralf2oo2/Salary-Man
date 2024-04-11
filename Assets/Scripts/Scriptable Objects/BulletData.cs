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

    private AudioSource audioSource;

    private Vector3 shootDirection = Vector3.zero;

    [SerializeField] LayerMask obstructionMask;
    public bool isBase = true;

    public void SetShootDirection(Vector3 direction)
    {
        shootDirection = direction;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponentInChildren<AudioSource>();
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
        if (collision.collider.CompareTag("Bullet")) return;
        if(collision.gameObject.GetComponent<Health>() != null)
        {
            Health health = collision.gameObject.GetComponent<Health>();
            health.health -= damage;
        }
        else
        {

            /*Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100);*/
            Vector3 collisionNormal = collision.GetContact(0).normal;
            Vector3 collisionPoint = collision.GetContact(0).point;

            GameObject bulletHole = Instantiate(bulletHolePrefab, collisionPoint - (collisionNormal * 0.2f), Quaternion.LookRotation(collisionNormal, Vector3.up) * bulletHolePrefab.transform.rotation);

            bulletHole.transform.SetParent(collision.gameObject.transform);
            bulletHole.transform.Rotate(Vector3.forward, Random.Range(0f, 360f));

            AudioSource audioSourceClone = bulletHole.AddComponent<AudioSource>();
            audioSourceClone.transform.parent = bulletHole.transform;
            audioSourceClone.pitch = Random.Range(0.8f, 1.2f);
            audioSourceClone.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
            audioSource.spatialBlend = audioSource.spatialBlend;
            audioSourceClone.Play();

            Destroy(bulletHole, 30);
        }
        DestroyBullet();
    }
}
