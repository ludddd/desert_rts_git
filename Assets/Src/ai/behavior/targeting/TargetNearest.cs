using ai.target;
using faction;

namespace ai.behavior.targeting
{
    public class TargetNearest: BaseWithShooter
    {
        private readonly FactionData.FactionId factionId;

        public TargetNearest(IShooter shooter, FactionData.FactionId factionId) : base(shooter)
        {
            this.factionId = factionId;
        }

        public override void Update()
        {
            if (shooter.Target == null || !shooter.Target.IsValid)
            {
                shooter.Target = FindNearest.Get(shooter.Position, factionId, shooter.Range, shooter.CanHit);
            }
        }
    }
}
