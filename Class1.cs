using ThunderRoad;
using UnityEngine;

namespace OneForAll
{
    public class OneForAllSpell : SpellCastCharge
    {
        public float strengthMult;
        public float speedMult;
        public override void Fire(bool active)
        {
            base.Fire(active);
            if (spellCaster.ragdollHand.creature.gameObject.GetComponent<OneForAllComponent>() == null && active)
            {
                spellCaster.ragdollHand.creature.gameObject.AddComponent<OneForAllComponent>().Entry(strengthMult, speedMult);
            }
            else if (active)
            {
                Object.Destroy(spellCaster.ragdollHand.creature.gameObject.GetComponent<OneForAllComponent>());
            }
        }
    }
    public class OneForAllComponent : MonoBehaviour
    {
        public float strengthMultiplier = 2;
        public float speedMultiplier = 2;
        Creature creature;
        EffectInstance instance;
        bool fallDamage;
        public void Start()
        {
            creature = GetComponent<Creature>();
            instance = Catalog.GetData<EffectData>("OneForAll").Spawn(creature.ragdoll.rootPart.transform, true);
            instance.SetRenderer(creature.GetRendererForVFX(), false);
            instance.SetIntensity(1f);
            instance.Play();
            fallDamage = Player.fallDamage;
            EnhanceStrength();
            EnhanceSpeed();
        }
        public void EnhanceStrength()
        {
            creature.data.forceMaxPosition *= strengthMultiplier;
            creature.data.forceMaxRotation *= strengthMultiplier;
            creature.data.forcePositionSpringDamper.x *= strengthMultiplier;
            creature.data.forceRotationSpringDamper.x *= strengthMultiplier;
            creature.data.climbingForcePositionSpringDamperMult.x *= strengthMultiplier;
            creature.data.gripForceMaxPosition *= strengthMultiplier;
            creature.data.gripForceMaxRotation *= strengthMultiplier;
            creature.data.gripForcePositionSpringDamperMult.x *= strengthMultiplier;
            creature.data.gripForceRotationSpringDamperMult.x *= strengthMultiplier;
            if (creature.handLeft.grabbedHandle != null)
                creature.handLeft.grabbedHandle.RefreshAllJointDrives();
            if (creature.handRight.grabbedHandle != null)
                creature.handRight.grabbedHandle.RefreshAllJointDrives();
        }
        public void EnhanceSpeed()
        {
            if (creature.isPlayer)
            {
                Player.fallDamage = false;
            }
            creature.currentLocomotion.SetSpeedModifier(this, speedMultiplier, speedMultiplier, speedMultiplier, speedMultiplier, speedMultiplier);
        }
        public void RestoreStrength()
        {
            creature.data.forceMaxPosition /= strengthMultiplier;
            creature.data.forceMaxRotation /= strengthMultiplier;
            creature.data.forcePositionSpringDamper.x /= strengthMultiplier;
            creature.data.forceRotationSpringDamper.x /= strengthMultiplier;
            creature.data.climbingForcePositionSpringDamperMult.x /= strengthMultiplier;
            creature.data.gripForceMaxPosition /= strengthMultiplier;
            creature.data.gripForceMaxRotation /= strengthMultiplier;
            creature.data.gripForcePositionSpringDamperMult.x /= strengthMultiplier;
            creature.data.gripForceRotationSpringDamperMult.x /= strengthMultiplier;
            if (creature.handLeft.grabbedHandle != null)
                creature.handLeft.grabbedHandle.RefreshAllJointDrives();
            if (creature.handRight.grabbedHandle != null)
                creature.handRight.grabbedHandle.RefreshAllJointDrives();
        }
        public void RestoreSpeed()
        {
            if (creature.isPlayer)
            {
                Player.fallDamage = fallDamage;
            }
            creature.currentLocomotion.RemoveSpeedModifier(this);
        }
        public void OnDestroy()
        {
            instance.Stop();
            RestoreStrength();
            RestoreSpeed();
        }
        public void Entry(float importStrength, float importSpeed)
        {
            strengthMultiplier = importStrength;
            speedMultiplier = importSpeed;
        }
    }
}
