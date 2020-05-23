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

        public void DoReturn(Computer computer)
        {
            //se retornan las naves
            computer.Script.Variable += hackOrder.Variable;
            computer.Script.Conditional += hackOrder.Conditional;
            computer.Script.Iterator += hackOrder.Iterator;
            computer.Script.Json += hackOrder.Json;
            computer.Script.Class += hackOrder.Class;
            computer.Script.BreakPoint += hackOrder.BreakPoint;

            //se borra el hackorder
            context.DeleteHackOrder(hackOrder);
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

        public void WriteReceiverMessage(int instituteId, Computer computerFrom, int[] report)
        {
            Enrollment enrollment = CatchEnrollment(instituteId, computerTo.Id);
            Message message = new Message(
                enrollment.InstituteId,
                enrollment.UserId,
                "---",
                "Sistema",
                "Llegada del transporte",
                string.Format("El transporte con origen '{0}' ha llegado al ordenador y ha dejado los siguientes recursos: {1} de Conocimiento, {2} de Ingenio y {3} de Café.",
                    computerFrom.IpDirection, report[0], report[1], report[2])
                );

            context.CreateMessage(message);
        }

        public void WriteReturnMessage(Enrollment enrollment)
        {
            Message message = new Message(
                enrollment.InstituteId,
                enrollment.UserId,
                "---",
                "Sistema",
                "Retorno del transporte",
                string.Format("La misión de transporte con destino {0} ha regresado al ordenador. Vuelven los siguientes scripts: {1}{2}{3}{4}{5}{6}",
                    computerTo.IpDirection,
                    hackOrder.Variable != 0 ? hackOrder.Variable + " de variables, " : "",
                    hackOrder.Conditional != 0 ? hackOrder.Conditional + " de condicionales," : "",
                    hackOrder.Iterator != 0 ? hackOrder.Iterator + " de iteradores," : "",
                    hackOrder.Json != 0 ? hackOrder.Json + " de jsons," : "",
                    hackOrder.Class != 0 ? hackOrder.Class + " de classes," : "",
                    hackOrder.BreakPoint != 0 ? hackOrder.BreakPoint + " de breakpoints" : ".")
                );

            context.CreateMessage(message);
        }

        private Enrollment CatchEnrollment(int instituteId, int computerId)
        {
            foreach (Enrollment enrollment in context.GetEnrollments(instituteId))
                foreach (Computer computer in context.GetComputers(enrollment.Id))
                    if (computer.Id == computerId) return enrollment;
            return null;
        }
    }
}
