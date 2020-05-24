using MarianaoWars.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarianaoWarsConsole.Logic
{
    public class ColonizeHackOrder
    {
        private Service context;
        private HackOrder hackOrder;
        private Computer computerFrom;
        private Computer computerTo;

        public ColonizeHackOrder(Service service, HackOrder hackOrder, Computer computerFrom, Computer computerTo)
        {
            context = service;
            this.hackOrder = hackOrder;
            this.computerFrom = computerFrom;
            this.computerTo = computerTo;
        }

        public string[] DoColonize(Enrollment enrollment)
        {
            //inializamos los recursos
            Resource resource = context.SaveResource(new Resource());

            //inicializamos el software
            Software software = context.SaveSoftware(new Software());

            //inicializamos talentos
            Talent talent = context.SaveTalent(new Talent());

            //inicializamos los scripts
            Script script = context.SaveScript(new Script());

            //inicializamos el ordenador
            Computer computer = new Computer(
                generateName(enrollment),
                hackOrder.NewIp,
                false,
                resource,
                software,
                talent,
                script,
                enrollment
                );

            //damos los recursos al nuevo ordenador
            computer.Resource.Knowledge += hackOrder.Knowledge;
            computer.Resource.Ingenyous += hackOrder.Ingenyous;
            computer.Resource.Coffee += hackOrder.Coffee;

            computer = context.SaveComputer(computer);

            //soltamos los recursos
            string[] report = new string[]
            {
                hackOrder.Knowledge.ToString(),
                hackOrder.Ingenyous.ToString(),
                hackOrder.Coffee.ToString(),
                computer.IpDirection
            };

            hackOrder.Knowledge = 0;
            hackOrder.Ingenyous = 0;
            hackOrder.Coffee = 0;

            //perdemos el script de class
            hackOrder.Class -= 1;
            hackOrder.IsReturn = true;

            context.UpdateHackOrder(hackOrder);

            return report;
        }

        public void DoReturn()
        {
            //se retornan las naves
            computerFrom.Script.Variable += hackOrder.Variable;
            computerFrom.Script.Conditional += hackOrder.Conditional;
            computerFrom.Script.Iterator += hackOrder.Iterator;
            computerFrom.Script.Json += hackOrder.Json;
            computerFrom.Script.Class += hackOrder.Class;
            computerFrom.Script.BreakPoint += hackOrder.BreakPoint;

            //se borra el hackorder
            context.DeleteHackOrder(hackOrder);

            //se actualiza el computer
            context.UpdateComputer(computerFrom);
        }

        public void WriteColonizeMessage(string[] report)
        {
            Message message = new Message(
                computerFrom.Id,
                "---",
                "Sistema",
                "Creación de ordenador",
                string.Format("La creación del nuevo ordenador con dirección {0} se ha completado con éxito. También se han depositado en él los siguientes recursos: {1} de Conocimiento, {2} de Ingenio y {3} de Café.",
                    report[3], report[0], report[1], report[2])
                );

            context.CreateMessage(message);
        }

        public void WriteReceiverMessage()
        {
            Message message = new Message(
                computerTo.Id,
                "---",
                "Sistema",
                "¡Ordenador conectado!",
                string.Format("¡Enhorabuena! Tu ordenador con Ip '{0}' se ha conectado con éxito a la red",
                    computerFrom.IpDirection)
                );

            context.CreateMessage(message);
        }

        public void WriteReturnMessage(Enrollment enrollment)
        {
            Message message = new Message(
                computerFrom.Id,
                "---",
                "Sistema",
                "Retorno de la conexión",
                string.Format("La misión de conexión con destino {0} ha regresado al ordenador. Vuelven los siguientes scripts: {1}{2}{3}{4}{5}{6}",
                    hackOrder.NewIp,
                    hackOrder.Variable != 0 ? hackOrder.Variable + " de variables, " : "",
                    hackOrder.Conditional != 0 ? hackOrder.Conditional + " de condicionales," : "",
                    hackOrder.Iterator != 0 ? hackOrder.Iterator + " de iteradores," : "",
                    hackOrder.Json != 0 ? hackOrder.Json + " de jsons," : "",
                    hackOrder.Class != 0 ? hackOrder.Class + " de classes," : "",
                    hackOrder.BreakPoint != 0 ? hackOrder.BreakPoint + " de breakpoints" : "")
                );

            context.CreateMessage(message);
        }

        private string generateName(Enrollment enrollment)
        {
            int count = context.GetComputers(enrollment.Id).Count + 1; 
            return "Ordenador " + count;
        }
    }
}
