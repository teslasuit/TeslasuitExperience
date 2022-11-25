using UnityEngine;

public interface IDestroyable {
    void TakeDamage(int amount, Vector3 hitPoint);
}
