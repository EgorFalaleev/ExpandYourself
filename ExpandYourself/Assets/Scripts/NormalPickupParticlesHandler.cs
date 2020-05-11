using UnityEngine;

public class NormalPickupParticlesHandler : MonoBehaviour
{
    // state variables
    private bool needToIncrease = false;

    void Start()
    {
        // if multiplier is incremented increase number of particles generated
        if (needToIncrease)
        {
            ParticleSystem particles = GetComponent<ParticleSystem>();
            var particlesEmission = particles.emission;
            particlesEmission.SetBurst(0, new ParticleSystem.Burst(0, 100));
        }
    }

    public void IncreaseNumberOfParticles()
    {
        needToIncrease = true;
    }
}
