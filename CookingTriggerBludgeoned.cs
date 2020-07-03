using XRL.Rules;
using XRL.World.Parts;

namespace XRL.World.Effects
{
	public class CookingDomainStrongBones_OnBludgeon : ProceduralCookingEffectWithTrigger
	{
		public int Tier;

		public override void Init(GameObject target)
		{
			Tier = Stat.Random(1, 3);
			base.Init(target);
		}

		public override string GetTriggerDescription()
		{
			return "whenever @thisCreature take@s cudgel, falling, or concussive damage, there's a " + Tier + "% chance";
		}

		public override string GetTemplatedTriggerDescription()
		{
			return "whenever @thisCreature take@s cudgel, falling, or concussive damage, there's a 10-30% chance";
		}

		public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "TookDamage");
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "TookDamage");
			base.Unregister(Object);
		}

		public override bool FireEvent(Event E)
		{
			if (E.ID == "TookDamage" && Stat.Random(1, 100) <= Tier*10)
			{
                Damage damage = E.GetParameter("Damage") as Damage;
				if (damage.HasAttribute("Crushing") || damage.HasAttribute("Cudgel") || damage.HasAttribute("Concussion"))
				{
				    Trigger();
                }
			}
			return base.FireEvent(E);
		}
	}
}
