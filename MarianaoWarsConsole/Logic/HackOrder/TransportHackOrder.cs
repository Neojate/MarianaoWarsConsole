using MarianaoWars.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarianaoWarsConsole.Logic
{
    public class TransportHackOrder
    {
        private Service context;
        private HackOrder hackOrder;
        private Computer computerTo;

        public TransportHackOrder(Service service, HackOrder hackOrder, Computer computerTo)
        {
            context = service;
            this.hackOrder = hackOrder;
            this.computerTo = computerTo;
        }

        public int[] DoTransport()
        {
            SystemSoftware systemSoftware = context.GetSystemSoftware()[Software.MYSQL];
            int warehouse = int.Parse(systemSoftware.Action1.Split(',')[computerTo.Software.MySqlVersion]);

            //se traspasan los recursos de las naves al ordenador
            computerTo.Resource.Knowledge += hackOrder.Knowledge;
            if (computerTo.Resource.Knowledge > warehouse) computerTo.Resource.Knowledge = warehouse;

            computerTo.Resource.Ingenyous += hackOrder.Ingenyous;
            if (computerTo.Resource.Ingenyous > warehouse) computerTo.Resource.Ingenyous = warehouse;

            computerTo.Resource.Coffee += hackOrder.Coffee;
            if (computerTo.Resource.Coffee > warehouse) computerTo.Resource.Coffee = warehouse;

            //se actualiza el ordenador
            context.UpdateComputer(computerTo);

            //se genera el report
            int[] report = new int[] { hackOrder.Knowledge, hackOrder.Ingenyous, hackOrder.Coffee };

            //se vacían los recursos del hackOrder
            hackOrder.Knowledge = 0;
            hackOrder.Ingenyous = 0;
            hackOrder.Coffee = 0;

            //se vuelve hacia la base
            hackOrder.IsReturn = true;

            //se actualiza el hackOrder
            context.UpdateHackOrder(hackOrder);

            return report;
        }

        public int[] DoReturn(Computer computer)
        {
            //se genera el report
            int[] report = new int[]
            {
                hackOrder.Variable,
                hackOrder.Conditional,
                hackOrder.Iterator,
                hackOrder.Json,
                hackOrder.Class,
                hackOrder.BreakPoint
            };

            //se retornan las naves
            computer.Script.Variable += hackOrder.Variable;
            computer.Script.Conditional += hackOrder.Conditional;
            computer.Script.Iterator += hackOrder.Iterator;
            computer.Script.Json += hackOrder.Json;
            computer.Script.Class += hackOrder.Class;
            computer.Script.BreakPoint += hackOrder.BreakPoint;

            //se borra el hackorder
            context.DeleteHackOrder(hackOrder);

            return report;
        }

        public void WriteTransportMesssage(Enrollment enrollment, int[] report)
        {
            Message message = new Message(
                enrollment.InstituteId,
                enrollment.UserId,
                "---",
                "Sistema",
                "Llegada del transporte",
                string.Format("El transporte ha llegado a su destino '{0}' y ha dejado los siguientes recursos: {1} de Conocimiento, {2} de Ingenio y {3} de Café.",
                    computerTo.IpDirection, report[0], report[1], report[2])
                );

            context.CreateMessage(message);
        }

        public void WriteReturnMessage(Enrollment enrollment, int[] report)
        {
            Message message = new Message(
                enrollment.InstituteId,
                enrollment.UserId,
                "---",
                "Sistema",
                "Retorno del transporte",
                string.Format("La misión de transporte con destino {0} ha regresado al ordenador. Vuelven los siguientes scripts: {1}{2}{3}{4}{5}{6}",
                computerTo.IpDirection,
                report[0] != 0 ? report[0] + " de variables, " : "",
                report[1] != 0 ? report[1] + " de condicionales," : "",
                report[2] != 0 ? report[2] + " de iteradores," : "",
                report[3] != 0 ? report[3] + " de jsons," : "",
                report[4] != 0 ? report[4] + " de classes," : "",
                report[5] != 0 ? report[5] + " de breakpoints" : ".")
                );

            context.CreateMessage(message);
        }
    }
}
