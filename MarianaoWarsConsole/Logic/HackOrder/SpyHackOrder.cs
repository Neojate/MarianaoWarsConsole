﻿using MarianaoWars.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarianaoWarsConsole.Logic
{
    public class SpyHackOrder
    {
        Service context;
        HackOrder hackOrder;
        Computer computerFrom;
        Computer computerTo;

        public SpyHackOrder(Service service, HackOrder hackOrder, Computer computerFrom, Computer computerTo)
        {
            context = service;
            this.hackOrder = hackOrder;
            this.computerFrom = computerFrom;
            this.computerTo = computerTo;
        }

        public int[] DoSpy()
        {
            Random rnd = new Random();
            SystemTalent Sftp = context.GetSystemTalents()[Talent.SFTP];
            int sftpAction = int.Parse(Sftp.Action1.Split(',')[computerTo.Talent.SftpLevel]);

            int destroyedBreakpoints = 0;

            int[] report = null;

            //calculamos el porcentaje para cada nave espia
            for (int i = 0; i < hackOrder.BreakPoint; i++)
            {
                int randomNumber = rnd.Next(0, 100);

                //si el breakpoint supera el checkeo
                if (randomNumber > sftpAction)
                {
                    //generamos el reporte
                    report = new int[]
                    {
                        (int)computerTo.Resource.Knowledge,
                        (int)computerTo.Resource.Ingenyous,
                        (int)computerTo.Resource.Coffee,
                        computerTo.Script.Variable,
                        computerTo.Script.Conditional,
                        computerTo.Script.Iterator,
                        computerTo.Script.Json,
                        computerTo.Script.Class,
                        computerTo.Script.BreakPoint,
                        computerTo.Script.Throws,
                        computerTo.Script.TryCatch
                    };
                    break;
                }
                else
                {
                    //el breakpoint se destruye
                    destroyedBreakpoints++;
                }
            }

            //el hackorder regresa
            hackOrder.BreakPoint -= destroyedBreakpoints;
            hackOrder.IsReturn = true;

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

        public void WriteSpyMessage(int[] report)
        {
            Message message = new Message(
                computerFrom.Id,
                "---",
                "Sistema",
                "Reporte de Debug",
                report == null ? 
                    string.Format("El intento de debug al ordenador '{0}' ha sido un fracaso. Has perdido todos tus breakpoints.", computerTo.IpDirection) :
                    string.Format("El intento de debug al ordenador '{0}' ha sido todo un éxito. El ordenador debugado tiene {1} de Conocimiento, {2} de Ingenio, {3} de Café, " +
                        "{4} Variables, {5} Condicionales, {6} Iteradores, {7} Jsons, {8} classes, {9} Breakpoints, {10} Throws y {11} TryCatch.",
                        computerTo.IpDirection, report[0], report[1], report[2], report[3], report[4], report[5], report[6], report[7], report[8], report[9], report[10])
                );

            context.CreateMessage(message);
        }

        public void WriteReceivermessage(int[] report)
        {
            Message message = new Message(
                computerTo.Id,
                "---",
                "Sistema",
                report == null ? "Intento de Debug" : "Alerta, debug!",
                report == null ?
                    string.Format("Te han intentado debugar desde el ordenador {0}. Por suerte, su intento ha sido un fracaso.", computerFrom.IpDirection) :
                    "Has sido debugado. Por desgracia no hemos podido determinar la procedencia del debug. Es mejor que te prepares por si acaso."
                );

            context.CreateMessage(message);
        }

        public void WriteReturnMessage()
        {
            Message message = new Message(
                computerFrom.Id,
                "---",
                "Sistema",
                "Retorno del Debug",
                string.Format("La misión de debug con destino {0} ha regresado al ordenador. Vuelven los siguientes scripts: {1}{2}{3}{4}{5}{6}.",
                    computerTo.IpDirection,
                    hackOrder.Variable != 0 ? hackOrder.Variable + " de variables, " : "",
                    hackOrder.Conditional != 0 ? hackOrder.Conditional + " de condicionales," : "",
                    hackOrder.Iterator != 0 ? hackOrder.Iterator + " de iteradores," : "",
                    hackOrder.Json != 0 ? hackOrder.Json + " de jsons," : "",
                    hackOrder.Class != 0 ? hackOrder.Class + " de classes," : "",
                    hackOrder.BreakPoint != 0 ? hackOrder.BreakPoint + " de breakpoints" : "")
                );

            context.CreateMessage(message);
        }

    }
}
