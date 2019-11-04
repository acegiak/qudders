using System;
using System.Collections.Generic;
using XRL;
using XRL.World;
using XRL.World.Parts;
using Qud.API;
using XRL.Rules;


namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_ChanceToMutate  : IPart
	{
        public string Mutation = "Milk Glands";
        public int Chance = 95;
        public int Level = 1;

        public void domutate(){
            int roll = Stat.Random(1,100);
           Log("Titroll:"+roll.ToString()+"/"+Chance.ToString());
            if(roll<=Chance){
                Mutations mutations = ParentObject.GetPart("Mutations") as Mutations;
                if (mutations == null )
                {
                   Log("Can mutate, no mutations part");
                    return ;
                }
                if( ! MutationFactory.MutationsByName.ContainsKey(Mutation)){
                   Log("Mutation "+Mutation+" isn't recognised");
                }
                mutations.AddMutation(MutationFactory.MutationsByName[Mutation].CreateInstance(), Level);
               Log("mutated");
            }
        }

		public override void Register(GameObject Object)
		{
			Object.RegisterPartEvent(this, "ObjectCreated");
			base.Register(Object);
		}

		public override bool FireEvent(Event E)
		{
			if (E.ID == "ObjectCreated")
			{
                domutate();
            }
            return base.FireEvent(E);
        }
    }

}