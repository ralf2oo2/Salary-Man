using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GunData gunData;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected GameObject bullet;

    protected AudioSource audioSource;

    protected float timeSinceLastShot;

    // Start is called before the first frame update
    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
        audioSource = GetComponentInChildren<AudioSource>();
        gunData.reloading = false;
    }

    public void StartReload()
    {
        Debug.Log("tried to reload");
        if (!gunData.reloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        Debug.Log("Reloading");
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;
    }

    protected bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public virtual void Shoot()
    {
        Debug.Log("tried to shoot");
        if (gunData.currentAmmo > 0)
        {
            Debug.Log("has ammo");
            if (CanShoot()) 
            {
                Debug.Log("can shoot");
                Vector3 targetPoint;
                if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, gunData.maxDistance)) 
                {
                    targetPoint = hitInfo.point;
                    /*IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(gunData.damage);*/
                }
                else
                {
                    targetPoint = muzzle.position + muzzle.forward * 75;
                }

                Vector3 directionWithoutSpread = targetPoint - muzzle.position;

                float xSpread = Random.Range(-gunData.spread, gunData.spread);
                float ySpread = Random.Range(-gunData.spread, gunData.spread);

                Vector3 directionWithSpread = directionWithoutSpread + new Vector3(xSpread, ySpread, 0);

                GameObject currentBullet = Instantiate(bullet, muzzle.position, Quaternion.identity);

                currentBullet.transform.forward = directionWithSpread.normalized;

                currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * gunData.shootForce, ForceMode.Impulse);
                currentBullet.GetComponent<BulletData>().isBase = false;
                currentBullet.GetComponent<BulletData>().SetShootDirection(muzzle.transform.forward);
                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                audioSource.PlayOneShot(audioSource.clip);
                OnGunShot();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        Debug.DrawRay(muzzle.position, muzzle.forward);
    }

    protected void OnGunShot()
    {

    }
}
