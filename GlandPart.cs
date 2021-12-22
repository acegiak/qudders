using System;
using XRL.Language;

namespace XRL.World.Parts.Mutation
{
	[Serializable]
	public class acegiak_Glands  : BaseMutation
	{
		public string GlandType = "milk";

		public acegiak_Glands()
		{
			DisplayName = "Milk Glands";
			Type = "Physical";
		}
		public override bool SameAs(IPart p)
		{
			return false;
		}
		public static int GetMaxVolume(int Level)
		{
			return 6 + Level * 2;
		}
		public int GetMaxVolume()
		{
			return GetMaxVolume(base.Level);
		}
		public static int GetProductionRate(int Level)
		{
			return (int) (500.0d * Math.Pow(0.9, Level-1));
		} 
		public int GetProductionRate()
		{
			return GetProductionRate(base.Level);
		}
		/// Used to update max volume and production rate based on mutation level
		public void SyncStats()
		{
			GameObject GlandsObject = GetGlands();
			if(GlandsObject == null)
			{
				return;
			}
			GlandsObject.GetPart<LiquidProducer>().Rate = GetProductionRate();
			GlandsObject.GetPart<LiquidVolume>().MaxVolume = GetMaxVolume();
		}
		public void AddBodyPart(){

			string gtype = ParentObject.GetTag("GlandLiquid");
			if(gtype != null){
				//IPart.AddPlayerMessage("glandtype:"+gtype);
				this.GlandType = gtype;
			}

			Body part = ParentObject.GetPart<Body>();
			if (part != null)
			{
				BodyPart body = part.GetBody();

				if(body.GetFirstPart("Glands")!= null){
					return;
				}

				BodyPart firstPart = body.AddPart(new BodyPart("Glands",part),"Back");
				GameObject GlandsObject = GameObjectFactory.Factory.CreateObject("DefaultGlands");
				GlandsObject.DisplayName = GlandType+" glands";
				SyncStats();
				LiquidProducer liquidProducer = GlandsObject.GetPart<LiquidProducer>();
				LiquidVolume liquidVolume = GlandsObject.GetPart<LiquidVolume>();
				liquidProducer.Liquid=GlandType;
				liquidVolume.Empty();
				liquidVolume.StartVolume = "1-"+liquidVolume.MaxVolume;
				liquidVolume.InitialLiquid = GlandType;
				liquidVolume.Volume = liquidVolume.StartVolume.RollCached();


				Event @event = Event.New("CommandForceEquipObject");
				@event.AddParameter("Object", GlandsObject);
				@event.AddParameter("BodyPart", firstPart);
				@event.SetSilent(true);
				ParentObject.FireEvent(@event);

			}


		}

		public override void Register(GameObject Object)
		{
			Object.RegisterPartEvent(this, "ObjectCreated");
			Object.RegisterPartEvent(this, "GetInventoryActions");
			Object.RegisterPartEvent(this, "InvCommandMilk");
			Object.RegisterPartEvent(this, "GetShortDescription");
			base.Register(Object);
		}

		public GameObject GetGlands()
		{
			Body part = ParentObject.GetPart<Body>();
			BodyPart bp = part.GetFirstPart("Glands");

			return bp.Equipped;
		}
		public override bool FireEvent(Event E)
		{
			if(E.ID =="ObjectCreated"){
				AddBodyPart();
			}


			if (E.ID == "GetInventoryActions")
			{
				//if(Volume > 0){
						// E.AddInventoryAction("Drink", 'k',  false, "drin&Wk&y", "InvCommandDrinkObject");
						E.AddInventoryAction("Milk", 'm',  false, "&Wm&yilk", "InvCommandMilk");
						// E.AddInventoryAction("Collect", 'c',  false, "&Wc&yollect", "InvCommandCollectObject");
				//}
			}

			if (E.ID == "InvCommandMilk")
			{
				GameObject GO = GetGlands();
				if(GO != null)
				{
					if(ParentObject.pBrain.IsHostileTowards(E.GetGameObjectParameter("Owner")) ){

						if (ParentObject.MakeSave("Strength", 18, E.GetGameObjectParameter("Owner"), null, "Milking"))
						{
							if (IPart.Visible(ParentObject))
							{
								ParentObject.ParticleText(IPart.ConsequentialColor(ParentObject) + "*resisted*");
							}
							if (E.GetGameObjectParameter("Owner").IsPlayer())
							{
								IPart.AddPlayerMessage("&r" + ParentObject.The + ParentObject.ShortDisplayName + "&r" + ParentObject.GetVerb("resist") + " your milking attempt.");
							}
							else if (ParentObject.IsPlayer())
							{
								IPart.AddPlayerMessage("&gYou resist " + Grammar.MakePossessive(E.GetGameObjectParameter("Owner").the + E.GetGameObjectParameter("Owner").ShortDisplayName) + "&g milking attempt.");
							}
							E.RequestInterfaceExit();
							return false;
						}
					}
					GO.FireEvent(E.Copy("InvCommandPourObject"));
				}
			}


			// if (E.ID == "GetShortDescription")
			// {
			// 	string text = "\n&CEndowed with pendulous milk glands.";
			// 	E.SetParameter("Postfix", E.GetStringParameter("Postfix") + text);
			// 	return true;
			// }

			return true;
		}


		public override string GetDescription()
		{
			return "You have glands that produce " + GlandType + " over time.";
		}

		public override string GetLevelText(int Level)
		{
			string glandname = DisplayName.ToLower();
			return "You can produce and store " + GetMaxVolume(Level) + " drams of " + GlandType + " in your " + GlandType + " glands at a rate of " + GetProductionRate(Level) + " rounds per dram.\n";
		}


		public override bool ChangeLevel(int NewLevel)
		{
			SyncStats();
			return base.ChangeLevel(NewLevel);
		}

		public override bool Mutate(GameObject GO, int Level)
		{
			Unmutate(GO);
			Body part = ParentObject.GetPart<Body>();
			if (part != null)
			{
				BodyPart body = part.GetBody();

				int glandcount = body.GetPartCount("Glands");
				if(glandcount <= 0){
					AddBodyPart();
				}
			}
			return base.Mutate(GO, Level);
		}

		public override bool Unmutate(GameObject GO)
		{
			Body part = ParentObject.GetPart<Body>();
			if (part != null)
			{
				BodyPart body = part.GetBody();

				int glandcount = body.GetPartCount("Glands");
				if(glandcount > 0){
					body.RemovePart(body.GetFirstPart("Glands"));
				}
			}
			return base.Unmutate(GO);
		}
	}
}
