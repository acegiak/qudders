using System;
using XRL.Language;

namespace XRL.World.Parts.Mutation
{
	[Serializable]
	public class acegiak_Glands  : BaseMutation
	{
		public int Bonus;

		public int FeetID;

		public int OldFeetID;

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
		public override bool CanLevel()
		{
			return false;
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
				GlandsObject.GetPart<LiquidProducer>().Liquid=GlandType;
				GlandsObject.GetPart<LiquidVolume>().Empty();
				GlandsObject.GetPart<LiquidVolume>().InitialLiquid=GlandType+"-1000";
				GlandsObject.GetPart<LiquidVolume>().Volume = GlandsObject.GetPart<LiquidVolume>().StartVolume.RollCached();
				//GlandsObject.GetPart<acegiak_NoPour>().ProcessInitialLiquid(GlandType-"1000");


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

		public GameObject CheckGlands()
		{
			GameObject result = null;
			int num = 0;
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
				GameObject GO = CheckGlands();
				if(GO != null){
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
			return "You have glands that produce milk over time.";
		}

		// public override string GetLevelText(int Level)
		// {
		// 	string empty = string.Empty;
		// 	return empty + "Your \n";
		// }


		public override bool ChangeLevel(int NewLevel)
		{
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
