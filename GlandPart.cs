using System;

namespace XRL.World.Parts.Mutation
{
	[Serializable]
	public class acegiak_Glands  : BaseMutation
	{
		public int Bonus;

		public int FeetID;

		public int OldFeetID;

        public string glandtype;

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
            
			BodyPartType.Make("Glands", null, "glands", null, null, "DefaultGlands", null, null, null, null, null, null, false, false, false, true);
            
            Body part = ParentObject.GetPart<Body>();
			if (part != null)
			{
				BodyPart body = part.GetBody();
				
				BodyPart firstPart = body.AddPart(new BodyPart("Glands",part),"Back");
                GameObject GlandsObject = GameObjectFactory.Factory.CreateObject("DefaultGlands");
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
