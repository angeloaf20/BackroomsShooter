using UnityEngine;

public interface IBullet
{
    public void DetectAndDoDamage(Transform origin, GunData gunData, float radiusStrength);
}
