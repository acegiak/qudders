using System;
using XRL.Rules;
using Qud.API;
using XRL.Rules;

namespace XRL.World.Parts
{
	[Serializable]
	[WantLoadBlueprint]
	[HasGameBasedStaticCache]
	public class acegiak_MilkMerchantHotloader : IPart
	{
        public acegiak_MilkMerchantHotloader(){
                            AddToPopTable("RandomLiquid", new PopulationObject { Blueprint = "milk" });
                            // AddToPopTable("RandomLiquid", new PopulationObject { Blueprint = "curd" });
                            AddToPopTable("StiltTents", new PopulationTable { Name = "DairyTent" });
                            AddToPopTable("VesselLiquid_Standard", new PopulationObject { Blueprint = "milk" });
                            AddToPopTable("Ingredients_EarlyTiers", new PopulationObject { Blueprint = "Milk Bottle" });
                            AddToPopTable("Ingredients_EarlyTiers", new PopulationObject { Blueprint = "Hunk of Cheese" });
                            Log("QUDDERS IS HOTLOADING POP TABLES");

        }
        // Helper method to fudge into the most common/simple pop tables.
        public static bool AddToPopTable(string table, params PopulationItem[] items) {
            PopulationInfo info;
            if (!PopulationManager.Populations.TryGetValue(table, out info))
                return false;
                
            // If this is a single group population, add to that group.
            if (info.Items.Count == 1 && info.Items[0] is PopulationGroup) { 
                var group = info.Items[0] as PopulationGroup;
                group.Items.AddRange(items);
                return true;
            }

            info.Items.AddRange(items);
            return true;
        }



    }

}