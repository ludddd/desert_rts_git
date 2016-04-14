using ai;
using faction;
using UnityEngine;
using wpn;

public class Turret : MonoBehaviour, IShooter
{
    [editor.attr.LinkTo("turret")]
    public GameObject turretObj;
    [editor.attr.LinkTo("MuzzlePoint*.")]
    public utils.MultipleObjectLink muzzlePoints;
    public Weapon weapon;

    private RepeatedTimedEvent shootEvent;
    private int nextWeaponIdx;

    private void Start()
    {
        //TODO: make this more beautifull
        if (Target == null)
        {
            Target = InvalidTarget.Invalid;
        }
        if (!turretObj || muzzlePoints.objects.Count == 0) {
            enabled = false;
        }
        if (muzzlePoints.objects.Count > 0) {
            Debug.Assert(weapon.fireRate > 0);
            shootEvent = new RepeatedTimedEvent(1.0f / weapon.fireRate / muzzlePoints.objects.Count);
            shootEvent.Event += OnShootEvent;
        }
    }

    private void Update()
    {
        //TODO: support attack mode when unit should move to target and attack it
        if (Target.IsValid) {
            RotateTurretTo(Target.Pos);
        }
        shootEvent.Update(Time.deltaTime);
    }

    private bool CanShoot
    {
        get { return Target.IsValid && CanHit(Target); }
    }

    private void RotateTurretTo(Vector3 pos)
    {
        Vector3 targetDir = pos - turretObj.transform.position;
        targetDir = turretObj.transform.InverseTransformVector(targetDir);
        targetDir.y = 0;    //project on local xz plane
        if (targetDir.magnitude > 0) {
            targetDir.Normalize();
            turretObj.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, targetDir) * turretObj.transform.localRotation;
        }
    }

    private void OnShootEvent()
    {
        if (CanShoot) {
            weapon.Shoot(CurrentMuzzlePoint.transform.position, Target.Pos, FactionId, Target);
            var anim = GetComponentInParent<Animator>();
            if (anim) {
                anim.Play("Shoot");
            }
            SwitchNextMuzzlePoint();
        }
    }

    private FactionData.FactionId FactionId
    {
        get { return Faction.GetFactionId(gameObject); }
    }

    private void SwitchNextMuzzlePoint()
    {
        nextWeaponIdx = (nextWeaponIdx + 1) % muzzlePoints.objects.Count;
    }

    private GameObject CurrentMuzzlePoint
    {
        get
        {
            return muzzlePoints.objects[nextWeaponIdx];
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (turretObj && weapon) {
            dbg.GizmoUtils.DrawCircleXZ(turretObj.transform.position, weapon.ShootDist, Color.red);
        }
    }

    public float ShootDist()
    {
        return weapon.ShootDist;
    }

    public GameObject GetShootingObstacle()
    {
        if (!Target.IsValid) return null;
        var hitInfo = weapon.ShootCast(muzzlePoints.objects[0].transform.position, Target.Pos);
        return hitInfo.HasValue ? hitInfo.Value.collider.gameObject : null;
    }

    public ITarget Target { get; set; }

    public bool CanHit(ITarget target)
    {
        return weapon.CanHit(DefaultMuzzlePoint, target.Pos);
    }

    public Vector3 DefaultMuzzlePoint
    {
        get
        {
            return muzzlePoints.objects[0].transform.position;
        }
    }

    public Vector3 Position
    {
        get { return gameObject.transform.position; }
    }

    public float Range
    {
        get { return weapon.ShootDist; }
    }
}


