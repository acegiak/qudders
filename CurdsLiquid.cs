using System;
using System.Collections.Generic;
using System.Text;
using XRL.Core;
using XRL.Rules;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Effects;

namespace XRL.Liquids
{
	[Serializable]
	internal class acegiak_LiquidCurds : BaseLiquid
	{
		public new const string ID = "curd";

		[NonSerialized]
		public static List<string> Colors = new List<string>(2)
		{
			"Y",
			"W"
		};

		public acegiak_LiquidCurds()
			: base(ID)
		{
		}

		public override List<string> GetColors()
		{
			return Colors;
		}

		public override string GetColor()
		{
			return "Y";
		}

		public override string GetName(LiquidVolume Liquid)
		{
			return "&Wcurd";
		}

		public override string GetAdjective(LiquidVolume Liquid)
		{
			return "&Wcurdled";
		}

		public override string GetSmearedName(LiquidVolume Liquid)
		{
			return "&Wcurdy";
		}


		public override string GetPreparedCookingIngredient()
		{
			return "HP";
		}

		public override bool Drank(LiquidVolume Liquid, int Volume, GameObject Target, StringBuilder Message, ref bool ExitInterface)
		{
			
			if (Target.HasPart("Stomach") && !Target.FireEvent(new Event("AddWater", "Amount", 10 * Liquid.ComponentLiquids[ID])))
			{
				return false;
			}
			if (Target.HasPart("Amphibious"))
			{
				Message.Append("You pour the milk on yourself!\n");
			}
			else 
			{
				Message.Append("Ahh, warm and refreshing!\n");
			}
			return true;
		}

		public override void RenderBackground(LiquidVolume Liquid, RenderEvent eRender)
		{
			eRender.ColorString = "^Y" + eRender.ColorString;
		}

		public override void RenderPrimary(LiquidVolume Liquid, RenderEvent eRender)
		{
			Physics physics = Liquid.ParentObject.GetPart("Physics") as Physics;
			if (physics.Temperature <= physics.FreezeTemperature)
			{
				eRender.RenderString = "~";
				eRender.ColorString = "&W^Y";
				return;
			}
			Render render = Liquid.ParentObject.GetPart("Render") as Render;
			int num = (XRLCore.CurrentFrame + Liquid.nFrameOffset) % 60;
			if (Stat.RandomCosmetic(1, 600) == 1)
			{
				eRender.RenderString = string.Empty + '\u000f';
				eRender.ColorString = "&W^Y";
			}
			if (Stat.RandomCosmetic(1, 60) == 1 || render.ColorString == "&b")
			{
				if (num < 15)
				{
					render.RenderString = string.Empty + '÷';
					render.ColorString = "&Y^W";
				}
				else if (num < 30)
				{
					render.RenderString = "~";
					render.ColorString = "&Y^W";
				}
				else if (num < 45)
				{
					render.RenderString = string.Empty + '\t';
					render.ColorString = "&Y^W";
				}
				else
				{
					render.RenderString = "~";
					render.ColorString = "&Y^W";
				}
			}
		}

		public override void RenderSecondary(LiquidVolume Liquid, RenderEvent eRender)
		{
			eRender.ColorString += "&Y";
		}

		public override void RenderSmearPrimary(LiquidVolume Liquid, RenderEvent eRender)
		{
			int num = XRLCore.CurrentFrame % 60;
			if (num > 5 && num < 15)
			{
				eRender.ColorString = "&Y";
			}
			base.RenderSmearPrimary(Liquid, eRender);
		}

		public override void ObjectEnteredCell(LiquidVolume Liquid, GameObject GO)
		{
		}

		public override float GetValuePerDram()
		{
			return 3.8f;
		}

		public override int GetNavigationWeight(LiquidVolume Liquid, GameObject GO, bool Smart, bool Slimewalking, ref bool Uncacheable)
		{
			return 0;
		}
	}
}
