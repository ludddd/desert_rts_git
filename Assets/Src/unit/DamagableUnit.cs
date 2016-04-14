namespace unit
{
    class DamagableUnit: Damagable
    {
        protected override void OnUnitDestroyed()
        {
            DestroyedUnit.Create(gameObject);
            Destroy(gameObject);
        }
    }
}
