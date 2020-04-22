using MarianaoWars.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarianaoWarsConsole.Logic
{
    public class MarianaoLogic
    {
        public static void UpdateResources(Computer computer, List<SystemResource> systemResources)
        {
            //conocimiento
            computer.Resource.Knowledge += int.Parse(systemResources[0].Increment.Split(',')[computer.Resource.KnowledgeLevel - 1]);

            //ingenio
            computer.Resource.Ingenyous += int.Parse(systemResources[1].Increment.Split(',')[computer.Resource.IngenyousLevel - 1]);

            //café
            //computer.Resource.Coffe += int.Parse(systemResources[2].Increment.Split(',')[computer.Resource.CoffeLevel]);

            //sleep
            //computer.Resource.StressLevel = int.Parse(systemResources[3].Increment.Split(',')[computer.Resource.StressLevel]);
        }

    }
}
