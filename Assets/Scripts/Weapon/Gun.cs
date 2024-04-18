using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GunData gunData;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected GameObject bullet;

    [SerializeField] AudioSource gunAudioSource;

    [SerializeField] AudioClip shootClip;
    [SerializeField] AudioClip reloadStartClip;
    [SerializeField] AudioClip reloadEndClip;
    private int currentAmmo;
    private int ammoReserve = 30;

    private bool reloading = false;

    private bool triggerUp = false;

    protected float timeSinceLastShot;

    // Start is called before the first frame update
    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
        PlayerShoot.triggerUpInput += TriggerUp;
    }

    private void OnDestroy()
    {
        PlayerShoot.shootInput -= Shoot;
        PlayerShoot.reloadInput -= StartReload;
        PlayerShoot.triggerUpInput -= TriggerUp;
    }

    public int CurrentAmmo => currentAmmo;
    public int AmmoReserve => ammoReserve;

    public void TriggerUp()
    {
        triggerUp = true;
    }

    public void StartReload()
    {
        if (!reloading && this != null)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        if(this == null)
        {
            yield return null;
        }
        else
        {
            reloading = true;
            gunAudioSource.PlayOneShot(reloadStartClip);

            yield return new WaitForSeconds(gunData.reloadTime);

            int bulletsToFill = gunData.magSize - currentAmmo;

            if (ammoReserve - bulletsToFill < 0)
            {
                currentAmmo = ammoReserve;
                ammoReserve = 0;
            }
            else
            {
                ammoReserve -= bulletsToFill;
                currentAmmo = gunData.magSize;
            }

            reloading = false;
            gunAudioSource.PlayOneShot(reloadEndClip);
        }
    }

    protected bool CanShoot() => !reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public virtual void Shoot()
    {
        if (this == null || muzzle == null || gunData == null || bullet == null) return;
        if(!triggerUp && !gunData.automatic)
        {
            return;
        }
        if (currentAmmo > 0)
        {
            if (CanShoot()) 
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                Vector3 targetPoint;
                if (Physics.Raycast(ray, out RaycastHit hitInfo, gunData.maxDistance)) 
                {
                    targetPoint = hitInfo.point;
                    /*IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(gunData.damage);*/
                }
                else
                {
                    targetPoint = ray.GetPoint(75);
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
                currentAmmo--;
                timeSinceLastShot = 0;
                gunAudioSource.PlayOneShot(shootClip);
                OnGunShot();
                Debug.Log("shot callewd" + currentAmmo);
                triggerUp = false;
                if (gunData.loud)
                {
                    LoudGunShot();
                }
            }
        }
    }

    private void LoudGunShot()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(muzzle.position, 30);
        foreach (Collider collider in rangeChecks)
        {
            if (collider.gameObject.GetComponentInChildren<EnemyAwareness>() != null)
            {
                EnemyAwareness awareness = collider.gameObject.GetComponentInChildren<EnemyAwareness>();
                awareness.MakeSuspicious();
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
