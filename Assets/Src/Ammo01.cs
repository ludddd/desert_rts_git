using UnityEngine;
using faction;

public class Ammo01 : MonoBehaviour
{

    public GameObject explosion;
    public float damage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        _onHit(other);
    }

    void OnColliderEnter(Collider other)
    {
        _onHit(other);
    }

    void _onHit(Collider other)
    {
        Faction faction = other.gameObject.GetComponentInParent<Faction>();
        if (faction == null || faction.FactionId.IsFriend(gameObject)) {
            return;
        }
        if (explosion) {
            GameObject expl = Instantiate(explosion);
            expl.transform.position = transform.position;
        }
        if (other.gameObject.GetComponent<Damagable>()) {
            other.gameObject.GetComponent<Damagable>().Damage(damage);
        }
        Destroy(gameObject);
    }
}
