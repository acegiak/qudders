using System;
using XRL.Core;
using XRL.UI;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_NoPour : LiquidVolume
	{
		public override void Register(GameObject Object)
		{
			Object.RegisterPartEvent(this, "GetInventoryActions");
            base.Register(Object);
        }

		public override bool FireEvent(Event E)
		{
            if (E.ID == "GetInventoryActions")
			{
                if(Volume > 0){
						//E.AddInventoryAction("Drink", 'k',  false, "drin&Wk&y", "InvCommandDrinkObject");
						E.AddInventoryAction("Milk", 'm',  false, "&Wm&yilk", "InvCommandPourObject");
                        //E.AddInventoryAction("Collect", 'c',  false, "&Wc&yollect", "InvCommandCollectObject");
                }
                return true;
			}else{

            }
            return base.FireEvent(E);
        }
    }
}