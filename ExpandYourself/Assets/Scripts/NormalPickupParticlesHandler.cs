using UnityEngine;

public class NormalPickupParticlesHandler : MonoBehaviour
{
    // state variables
    private bool needToIncrease = false;

    void Start()
    {
        // if multiplier is incremented increase number of particles generated and their size
        if (needToIncrease)
        {
            ParticleSystem particles = GetComponent<ParticleSystem>();
            var particlesEmission = particles.emission;
            var mainModule = particles.main;
            mainModule.startSize = Random.Range(0.25f, 0.35f);
            mainModule.startLifetime = Random.Range(0.5f, 1f);
            particlesEmission.SetBurst(0, new ParticleSystem.Burst(0, 150));
        }
    }

    public void IncreaseNumberOfParticles()
    {
        needToIncrease = true;
    }
}
