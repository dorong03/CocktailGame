using System;
using UnityEngine;

public class Droplet : MonoBehaviour
{
    private Vector2 velocity;
    private float lifetime;
    private float gravity = 20f;
    
    private string ingredientId;
    private float amount;
    private IPourTarget target;
    private Action<string, float> onPour;
    
    public void Launch(Vector2 velocity, float lifetime, string ingredientId, float amount , IPourTarget target, Action<string, float> onPour)
    {
        this.velocity = velocity;
        this.lifetime = lifetime;
        this.ingredientId = ingredientId;
        this.amount = amount;
        this.onPour = onPour;
        this.target = target;
    }
    
    void Update()
    {
        velocity.y -= gravity * Time.deltaTime;
        transform.position += (Vector3)(velocity * Time.deltaTime);

        if (target != null && target.IsInsideMouth(transform.position))
        {
            onPour?.Invoke(ingredientId, amount);
            Destroy(gameObject);
            return;
        }
        
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}