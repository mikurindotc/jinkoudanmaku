using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleHitbox : MonoBehaviour{
    private List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    private List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    [SerializeField] private Player playerHandler;
    void OnEnable(){
        playerHandler = GameObject.Find("PlayerPrefab").GetComponent<Player>();
    }

    void OnParticleTrigger()
    {
        ParticleSystem system = GetComponent<ParticleSystem>();
        int numInside = system.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside, out var insideData);

        for (int i = 0; i < numInside; i++)
        {
            if(insideData.GetCollider(i, 0) == system.trigger.GetCollider(0))
            {
                Debug.Log("I'm inside!");
            }
        }

        if (insideData.GetCollider(0, 0) == system.trigger.GetCollider(1) && numInside > 1)
        {
            Debug.Log("I'm dead!");
        }
    }
}
