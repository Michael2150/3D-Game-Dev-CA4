using UnityEngine;

public interface IHittable
{
    void Hit(GameObject hittingObject, IHitter hitter);
}