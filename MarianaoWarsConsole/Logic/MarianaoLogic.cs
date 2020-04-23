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
            int knowledgeStress = int.Parse(systemResources[0].Sleep.Split(',')[computer.Resource.KnowledgeLevel - 1]);
            int ingenyousStress = int.Parse(systemResources[1].Sleep.Split(',')[computer.Resource.IngenyousLevel - 1]);
            int coffeStress = int.Parse(systemResources[2].Sleep.Split(',')[computer.Resource.CoffeLevel - 1]);
            int currentStress = int.Parse(systemResources[3].Increment.Split(',')[computer.Resource.StressLevel - 1]);
            int finalStress = currentStress - knowledgeStress - ingenyousStress - coffeStress;

            //modificador por estrés
            int modStress = 1;
            if (finalStress < 0) modStress = 0;

            //conocimiento
            computer.Resource.Knowledge += int.Parse(systemResources[0].Increment.Split(',')[computer.Resource.KnowledgeLevel - 1]) * modStress;

            //ingenio
            computer.Resource.Ingenyous += int.Parse(systemResources[1].Increment.Split(',')[computer.Resource.IngenyousLevel - 1]) * modStress;

            //café
            computer.Resource.Coffe += int.Parse(systemResources[2].Increment.Split(',')[computer.Resource.CoffeLevel - 1]) * modStress;

            //sleep
            computer.Resource.Stress = finalStress;
        }

    }
}
