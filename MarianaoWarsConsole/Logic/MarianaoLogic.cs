using MarianaoWars.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarianaoWarsConsole.Logic
{
    public class MarianaoLogic
    {
        private const int MINUT = 60;

        public static void UpdateResources(Computer computer, List<SystemResource> systemResources, List<SystemSoftware> systemSoftwares)
        {
            SystemResource knowledge = systemResources[Resource.KNOWLEDGE];
            SystemResource ingenyous = systemResources[Resource.INGENYOUS];
            SystemResource coffee = systemResources[Resource.COFFEE];
            SystemResource stress = systemResources[Resource.STRESS];

            //cálculo estrés
            int knowledgeStress = int.Parse(knowledge.Sleep.Split(',')[computer.Resource.KnowledgeLevel - 1]);
            int ingenyousStress = int.Parse(ingenyous.Sleep.Split(',')[computer.Resource.IngenyousLevel - 1]);
            int coffeStress = int.Parse(coffee.Sleep.Split(',')[computer.Resource.CoffeLevel - 1]);
            int currentStress = int.Parse(stress.Increment.Split(',')[computer.Resource.StressLevel - 1]);
            int finalStress = currentStress - knowledgeStress - ingenyousStress - coffeStress;

            //almacenamiento
            SystemSoftware mySql = systemSoftwares[Software.MYSQL];
            int maxResource = int.Parse(mySql.Action1.Split(',')[computer.Software.MySqlVersion]);

            //modificador por estrés
            double modStress = 1;
            if (finalStress < 0) modStress = 0.5; //cambiar

            //conocimiento
            if (computer.Resource.Knowledge < maxResource)
                computer.Resource.Knowledge += double.Parse(knowledge.Increment.Split(',')[computer.Resource.KnowledgeLevel - 1]) * modStress / MINUT;

            //ingenio
            if (computer.Resource.Ingenyous < maxResource)
                computer.Resource.Ingenyous += double.Parse(ingenyous.Increment.Split(',')[computer.Resource.IngenyousLevel - 1]) * modStress / MINUT;

            //café
            if (computer.Resource.Coffe < maxResource)
                computer.Resource.Coffe += double.Parse(coffee.Increment.Split(',')[computer.Resource.CoffeLevel - 1]) * modStress / MINUT;

            //sleep
            computer.Resource.Stress = finalStress;
        }

        public static void UpdateBuilding(Computer computer, int buildId)
        {
            switch(buildId)
            {
                case 1:
                    computer.Resource.KnowledgeLevel += 1;
                    break;

                case 2:
                    computer.Resource.IngenyousLevel += 1;
                    break;

                case 3:
                    computer.Resource.CoffeLevel += 1;
                    break;

                case 4:
                    computer.Resource.StressLevel += 1;
                    break;

                case 21:
                    computer.Software.GeditVersion += 1;
                    break;

                case 22:
                    computer.Software.MySqlVersion += 1;
                    break;

            }
        }

    }
}
