using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeslasuitAPI;
using TsSDK;
using UnityEngine;

public class FogCollisionHaptic : MonoBehaviour {
    private ParticleSystem particleSystem;
    [SerializeField] public TsHapticMaterialAsset materialNew;
    private List<ParticleCollisionEvent> collisionEvents;
    private int layer;

    public float impact = 0.2f;
    private void Awake() {
        particleSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        layer = LayerMask.NameToLayer("HapticReceiver");
    }
    private List<TriggerHit> triggerHits = new List<TriggerHit>();
    public int MaxEventsHitperFrame = 1;

    public void TurnCollisionsEvents(bool state) {
        var collis= particleSystem.collision;
            collis.sendCollisionMessages = state;
        
    }


    void OnParticleCollision(GameObject other)
    {
         //Debug.Log(other.name);
        if(!other.CompareTag("Player"))
            return;
       
        
        int numCollisionEvents = particleSystem.GetCollisionEvents(other, collisionEvents);
        
        
        //return;
        int j = -1;
        int kHits = 0;
        while (j < numCollisionEvents) {
            j++;

            
            //continue;
            var collider = collisionEvents[j].colliderComponent as Collider;
            if (collider == null) 
                continue;
            //Debug.Log(collisionEvents[j].colliderComponent.gameObject.name);
            int k = -1;
            for (int i = 0; i < triggerHits.Count; i++) {
                if (triggerHits[i].col == collider) {
                    if (Time.time - triggerHits[i].timePrevHit < 0.2f) {
                        k = -2;
                        break;

                    }

                    k = i;
                    break;
                }
            }
            if(k==-2)
                continue;
            var from = collisionEvents[j].intersection+ collisionEvents[j].normal/ 100;
            var toThisVector = from - collisionEvents[j].intersection;
            var ray = new Ray(from, toThisVector);
            if (Physics.Raycast(ray, out var hit, 1.0f))
            {
                var handler = hit.transform.GetComponent<TsHapticCollisionHandler>();

                if (handler && materialNew && handler.HapticPlayer)
                {
                    Debug.Log("[FogCollisionHaptic]Hit! " + collider.gameObject.name);
                    if (handler.HapticPlayer.PlayerHandle.GetPlayable(materialNew.Instance) is
                        IHapticMaterialPlayable materialPlayable)
                    {
                        Debug.Log("[FogCollisionHaptic]Hit! with material " + collider.gameObject.name);
                        handler.AddImpact(materialPlayable, impact, 100);
                    }
                    if (k != -1) {
                        triggerHits[k].timePrevHit = Time.time;
                    } else {
                        triggerHits.Add(new TriggerHit() { col = collider, timePrevHit = Time.time });
                    }
                    kHits++;
                    if(kHits>= MaxEventsHitperFrame)
                        return;
                }
            }

        }
        
    }
}
