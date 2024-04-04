using UnityEngine;

public class NpcGun : Gun
{
    public override void Shoot()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                Vector3 raycastDir = player.transform.position - muzzle.transform.position;;
                Vector3 targetPoint;
                if (Physics.Raycast(muzzle.position, raycastDir, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    targetPoint = hitInfo.point;
                    /*IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(gunData.damage);*/
                }
                else
                {
                    targetPoint = muzzle.position + raycastDir * 75;
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
}
