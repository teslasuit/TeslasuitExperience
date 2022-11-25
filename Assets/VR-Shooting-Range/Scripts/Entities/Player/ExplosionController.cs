using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    public void SetColor(Color color)
    {
        if (_particleSystem != null) {
            var module=_particleSystem.main;
            module.startColor = color;
            _particleSystem.Play();
        }
    }
}
