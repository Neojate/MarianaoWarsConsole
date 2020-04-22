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
                Service service = new Service();

                //carga de las tablas de sistema
                LoadSystem(service);

                foreach (Institute institute in service.GetOpenInstitutes())
                {
                    Console.WriteLine("\t-Servidor: " + institute.Name);
                    foreach (Enrollment enrollment in service.GetEnrollments(institute.Id))
                    {
                        foreach (Computer computer in service.GetComputers(enrollment.Id))
                        {
                            Calcs(computer, institute);

                            service.UpdateComputer(computer);
                        }
                    }
                    Console.WriteLine("\t\tCalculos... OK");
                    Console.WriteLine("\t\tGuardados... OK");
                }
                Thread.Sleep(1000);
            }
        }

        private void LoadSystem(Service service)
        {
            systemResources = service.GetSystemResources();
        }

        private void Calcs(Computer computer, Institute institute)
        {
            //actualizar recursos
            MarianaoLogic.UpdateResources(computer, systemResources);
        }

        private void Saves(Computer computer)
        {
            
        }
    }
}
