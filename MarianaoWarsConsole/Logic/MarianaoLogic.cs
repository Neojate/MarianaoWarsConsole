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
            int knowledgeStress = int.Parse(knowledge.Sleep.Split(',')[computer.Resource.KnowledgeLevel]);
            int ingenyousStress = int.Parse(ingenyous.Sleep.Split(',')[computer.Resource.IngenyousLevel]);
            int coffeStress = int.Parse(coffee.Sleep.Split(',')[computer.Resource.CoffeLevel]);
            int currentStress = int.Parse(stress.Increment.Split(',')[computer.Resource.StressLevel]);
            int finalStress = currentStress - knowledgeStress - ingenyousStress - coffeStress;

            //almacenamiento
            SystemSoftware mySql = systemSoftwares[Software.MYSQL];
            int maxResource = int.Parse(mySql.Action1.Split(',')[computer.Software.MySqlVersion]);

            //modificador por estrés
            double modStress = 1;
            if (finalStress < 0) modStress = 0.5; //cambiar

            //conocimiento
            if (computer.Resource.Knowledge < maxResource)
                computer.Resource.Knowledge += double.Parse(knowledge.Increment.Split(',')[computer.Resource.KnowledgeLevel]) * modStress / MINUT;

            //ingenio
            if (computer.Resource.Ingenyous < maxResource)
                computer.Resource.Ingenyous += double.Parse(ingenyous.Increment.Split(',')[computer.Resource.IngenyousLevel]) * modStress / MINUT;

            //café
            if (computer.Resource.Coffe < maxResource)
                computer.Resource.Coffe += double.Parse(coffee.Increment.Split(',')[computer.Resource.CoffeLevel]) * modStress / MINUT;

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

                case 23:
                    computer.Software.GitHubVersion += 1;
                    break;

                case 24:
                    computer.Software.StackOverFlowVersion += 1;
                    break;

                case 25:
                    computer.Software.PostManVersion += 1;
                    break;

                case 26:
                    computer.Software.VirtualMachineVersion += 1;
                    break;

            }
        }

        public static Message GenerateBuildMessage(Enrollment enrollment, Computer computer, BuildOrder buildOrder)
        {
            return new Message(
                enrollment.InstituteId,
                enrollment.UserId,
                computer.Name,
                "Sistema",
                chooseBuildMessageTitle(buildOrder.BuildId),
                chooseBuildMessageBody(buildOrder.BuildId, computer.Name)
                );
        }

        private static string chooseBuildMessageTitle(int buildType)
        {
            switch (buildType / 20)
            {
                case 0:
                    return "Aumento recurso finalizado.";
                case 1:
                    return "Actualización de software finalizada.";
                case 2:
                    return "Desarrollo de talento completado";
                default: 
                    return "Error"; 
            }
        }

        private static string chooseBuildMessageBody(int buildType, string computerName)
        {
            switch (buildType / 20)
            {
                case 0:
                    return string.Format("Se ha completado con éxito el aumento del recurso '{0}' en el ordenador '{1}' a las {2}.",
                        buildIdName(buildType), computerName, DateTime.Now);
                case 1:
                    return string.Format("Se ha completado con éxito la actualización del software '{0}' en el ordenador '{1}' a las {2}.",
                        buildIdName(buildType), computerName, DateTime.Now);
                case 2:
                    return string.Format("Se ha completado con éxito el desarrollo del talento '{0}' en el ordenador '{1}' a las {2}.",
                        buildIdName(buildType), computerName, DateTime.Now);
                default:
                    return "Error al procesar el mensaje.";
            }
        }

        private static string buildIdName(int buildType)
        {
            switch (buildType)
            {
                case 1: return "Conocimiento";
                case 2: return "Ingenio";
                case 4: return "Café";
                case 3: return "Descanso";

                case 21: return "Gedit";
                case 22: return "Mysql";
                case 23: return "GitHub";
                case 24: return "Stack Overflow";
                case 25: return "Postman";
                case 26: return "VirtualBox";

                default: return "";
            }
        }

    }
}
