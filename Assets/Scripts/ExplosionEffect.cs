using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
