using UnityEngine;

//TODO: add interface IDamagable
public class Damagable : MonoBehaviour
{
    public float MaxHealth;

    // Use this for initialization
    void Start()
    {
        Health = MaxHealth;
    }

    public float Health { get; private set; }

    public virtual void Damage(float value)
    {
        if (!IsAlive) return;
        Health = Mathf.Max(0, Health - value);
        if (Health <= 0)
        {
            OnUnitDestroyed();
        }
    }

    public virtual bool IsAlive { get { return Health > 0; } }

    protected virtual void OnUnitDestroyed()
    {
    }
}
