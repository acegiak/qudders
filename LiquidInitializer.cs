using System;
using System.Collections.Generic;
using System.Text;
using XRL.Core;
using XRL.Messages;
using XRL.Rules;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Parts.Effects;
using UnityEngine;
using XRL.Liquids;
using XRL.World.Tinkering;

namespace XRL.World.Parts
{
    public class acegiak_MilkLiquidInitializer : IPart
    {
        //this code runs early during game boot - the game creates a temporary instance of every object from the blueprints
        public acegiak_MilkLiquidInitializer ()
        {
            Debug.Log("Initializing Milk Liquids.");
            

			LiquidVolume.ComponentLiquidTypes.Add(Convert.ToByte(acegiak_LiquidMilk.ID), new acegiak_LiquidMilk());
			LiquidVolume.ComponentLiquidNameMap.Add("milk", LiquidVolume.ComponentLiquidTypes[Convert.ToByte(acegiak_LiquidMilk.ID)]);


            Debug.Log("Initializing Milk Liquids - COMPLETE.");
			


			// LiquidVolume.ComponentLiquidTypes.Add(Convert.ToByte(acegiak_LiquidGrapeJuice.ID), new acegiak_LiquidGrapeJuice());
			// LiquidVolume.ComponentLiquidNameMap.Add("grapejuice", LiquidVolume.ComponentLiquidTypes[Convert.ToByte(acegiak_LiquidGrapeJuice.ID)]);


			// LiquidVolume.ComponentLiquidTypes.Add(Convert.ToByte(acegiak_LiquidAppleJuice.ID), new acegiak_LiquidAppleJuice());
			// LiquidVolume.ComponentLiquidNameMap.Add("applejuice", LiquidVolume.ComponentLiquidTypes[Convert.ToByte(acegiak_LiquidAppleJuice.ID)]);



			// BitType item = new BitType(113, 'w', "&wwood scrap");
			// BitType.BitTypes.Add(item);
            // BitType.BitMap.Add(item.Color, item);
            // if (!BitType.LevelMap.ContainsKey(item.Level))
            // {
            //     BitType.LevelMap.Add(item.Level, new List<BitType>());
            // }
            // BitType.LevelMap[item.Level].Add(item);
            // BitType.BitSortOrder.Add('w',133);
			
        }
    }
}