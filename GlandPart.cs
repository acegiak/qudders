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
            
			BodyPartType.Make("Glands", null, "glands", null, null, "DefaultGland", null, null, null, null, null, null, false, false, false, true);
            
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
		}

		public override bool FireEvent(Event E)
		{
            if(E.ID =="ObjectCreated"){
                AddBodyPart();
            }

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
