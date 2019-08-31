using System;
using XRL.Rules;

namespace XRL.World.Parts.Effects
{
	[Serializable]
	public class CookingDomainStrongBones_BludgeonDamageReduction : ProceduralCookingEffectUnit
	{
		public int Tier;

		public override string GetDescription()
		{
			return "-" + Tier + "0% damage from cudgel, falling, and concussive damage.";
		}

		public override string GetTemplatedDescription()
		{
			return "Reduced damage from cudgel, falling, and concussive damage.";
		}

		public override void Init(GameObject target)
		{
			Tier = Stat.Random(1, 4);
		}

		public override void Apply(GameObject Object, Effect parent)
		{
			Object.RegisterEffectEvent(parent, "BeforeApplyDamage");
		}

		public override void Remove(GameObject Object, Effect parent)
		{
			Object.UnregisterEffectEvent(parent, "BeforeApplyDamage");
		}

		public override void FireEvent(Event E)
		{
			if (E.ID == "BeforeApplyDamage")
			{
				Damage damage = E.GetParameter("Damage") as Damage;
				if (damage.HasAttribute("Crushing") || damage.HasAttribute("Cudgel") || damage.HasAttribute("Concussion"))
				{
					// if (damage.Amount > 0 && ParentObject.IsPlayer())
					// {
					// 	MessageQueue.AddPlayerMessage("&rYou feel your bones fracture.");
					// }
                    float multiplier = 1f-(Tier*0.1f);
					damage.Amount = (int)((float)damage.Amount * multiplier);
					E.AddParameter("Damage", damage);
				}
			}
		}
	}
}
