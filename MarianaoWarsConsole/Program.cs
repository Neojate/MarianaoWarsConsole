using MarianaoWars.Models;
using MarianaoWarsConsole.Data;
using MarianaoWarsConsole.Logic;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MarianaoWarsConsole
{
    class Program
    {
        private List<SystemResource> systemResources;
        private List<SystemSoftware> systemSoftwares;

        private Service service;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Launch();
        }

        private void Launch()
        {
            while(true)
            {
                Console.WriteLine("\n- - - - - INICIO DEL BUCLE: " + DateTime.Now + " - - - - -");
                service = new Service();

                //carga de las tablas de sistema
                LoadSystem();

                foreach (Institute institute in service.GetOpenInstitutes())
                {
                    Console.WriteLine("\t-Servidor: " + institute.Name);
                    foreach (Enrollment enrollment in service.GetEnrollments(institute.Id))
                    {
                        foreach (Computer computer in service.GetComputers(enrollment.Id))
                        {
                            List<BuildOrder> buildOrders = service.GetBuildOrder(computer.Id);
                            Calcs(computer, institute, buildOrders);

                            service.UpdateComputer(computer);
                        }
                    }
                    Console.WriteLine("\t\tCalculos... OK");
                    Console.WriteLine("\t\tGuardados... OK");
                }
                Thread.Sleep(1000);
            }
        }

        private void LoadSystem()
        {
            systemResources = service.GetSystemResources();
            systemSoftwares = service.GetSystemSoftware();
        }

        private void Calcs(Computer computer, Institute institute, List<BuildOrder> buildOrders)
        {
            //actualizar recursos
            MarianaoLogic.UpdateResources(computer, systemResources, systemSoftwares);

            //actualizar órdenes de recursos
            foreach (BuildOrder buildOrder in buildOrders)
            {
                if (DateTime.Compare(buildOrder.EndTime, DateTime.Now) < 0)
                {
                    MarianaoLogic.UpdateBuilding(computer, buildOrder.BuildId);
                    service.DeleteBuildOrder(buildOrder.Id);
                } 
            }
        }

    }
}
