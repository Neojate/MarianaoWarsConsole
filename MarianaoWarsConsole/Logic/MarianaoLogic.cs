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
            int coffeStress = int.Parse(coffee.Sleep.Split(',')[computer.Resource.CoffeeLevel]);
            int currentStress = int.Parse(stress.Increment.Split(',')[computer.Resource.StressLevel]);
            int finalStress = currentStress - knowledgeStress - ingenyousStress - coffeStress;

            //almacenamiento
            SystemSoftware mySql = systemSoftwares[Software.MYSQL];
            int maxResource = int.Parse(mySql.Action1.Split(',')[computer.Software.MySqlVersion]);

            //modificador por estrés
            double modStress = 1;
            if (finalStress < 0) modStress = 0.5;

            //conocimiento
            if (computer.Resource.Knowledge < maxResource)
                computer.Resource.Knowledge += double.Parse(knowledge.Increment.Split(',')[computer.Resource.KnowledgeLevel]) * modStress / MINUT;

            //ingenio
            if (computer.Resource.Ingenyous < maxResource)
                computer.Resource.Ingenyous += double.Parse(ingenyous.Increment.Split(',')[computer.Resource.IngenyousLevel]) * modStress / MINUT;

            //café
            if (computer.Resource.Coffee < maxResource)
                computer.Resource.Coffee += double.Parse(coffee.Increment.Split(',')[computer.Resource.CoffeeLevel]) * modStress / MINUT;

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
                    computer.Resource.CoffeeLevel += 1;
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

                case 41:
                    computer.Talent.RefactorLevel += 1;
                    break;

                case 42:
                    computer.Talent.InheritanceLevel += 1;
                    break;

                case 43:
                    computer.Talent.InjectionLevel += 1;
                    break;

                case 44:
                    computer.Talent.UdpLevel += 1;
                    break;

                case 45:
                    computer.Talent.TcpIpLevel += 1;
                    break;

                case 46:
                    computer.Talent.SftpLevel += 1;
                    break;

                case 47:
                    computer.Talent.EcbLevel += 1;
                    break;

                case 48:
                    computer.Talent.RsaLevel += 1;
                    break;

                case 61:
                    computer.Script.Comparator += 1;
                    break;

                case 62:
                    computer.Script.Conditional += 1;
                    break;

                case 63:
                    computer.Script.Iterator += 1;
                    break;

                case 64:
                    computer.Script.Json += 1;
                    break;

                case 65:
                    computer.Script.Class += 1;
                    break;

                case 66:
                    computer.Script.BreakPoint += 1;
                    break;

                case 67:
                    computer.Script.Throws += 1;
                    break;

                case 68:
                    computer.Script.TryCatch += 1;
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
                    return "Desarrollo de talento completado.";
                case 3:
                    return "Elaboración de script finalizado.";
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
                case 3:
                    return string.Format("Se ha completado con éxito la elaboración del script '{0}' en el ordenador '{1}' a las {2}",
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

                case 41: return "Refactorización";
                case 42: return "Herencia";
                case 43: return "Inyección de Dependencias";
                case 44: return "UDP";
                case 45: return "Tcp/Ip";
                case 46: return "SFTP";
                case 47: return "ECB";
                case 48: return "RSA";

                case 61: return "Comparador";
                case 62: return "Condicional";
                case 63: return "Iterador";
                case 64: return "Json";
                case 65: return "Class";
                case 66: return "Breakpoint";
                case 67: return "Throws";
                case 68: return "TryCatch";


                default: return "";
            }
        }

    }
}
