using MarianaoWars.Models;
using MarianaoWarsConsole.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarianaoWarsConsole.Logic
{
    public class AttackHackOrder
    {
        private Service context;
        private HackOrder hackOrder;
        private Computer computerFrom;
        private Computer computerTo;

        private List<SystemScript> sysScripts = new List<SystemScript>();

        public AttackHackOrder(Service service, HackOrder hackOrder, Computer computerFrom, Computer computerTo)
        {
            context = service;
            this.hackOrder = hackOrder;
            this.computerFrom = computerFrom;
            this.computerTo = computerTo;
            sysScripts = context.GetSystemScripts();
        }

        public int[] DoAttack()
        {
            Fighter attacker = new Fighter();
            Fighter defender = new Fighter();

            //foco de los atacantes
            int[] focusAttack = new int[]
            {
                    Script.TRYCATCH, Script.THROW, Script.ITERATOR, Script.CONDITIONAL, Script.VARIABLE, Script.CLASS, Script.JSON, Script.BREAKPOINT
            };

            //foco de los defensores
            int[] focusDefense = new int[]
            {
                    Script.ITERATOR, Script.CONDITIONAL, Script.VARIABLE, Script.CLASS, Script.JSON, Script.BREAKPOINT
            };

            int currentAttackForce = 0;
            int currentDefenseForce = 0;

            int[] attackScripts = null;
            int[] defenseScripts = null;

            //turno
            do
            {
                //array de scripts atacantes
                attackScripts = new int[]
                {
                    hackOrder.Variable,
                    hackOrder.Conditional,
                    hackOrder.Iterator,
                    hackOrder.Json,
                    hackOrder.Class,
                    hackOrder.BreakPoint
                };

                //array de scripts defensores
                defenseScripts = new int[]
                {
                    computerTo.Script.Variable,
                    computerTo.Script.Conditional,
                    computerTo.Script.Iterator,
                    computerTo.Script.Json,
                    computerTo.Script.Class,
                    computerTo.Script.BreakPoint,
                    computerTo.Script.Throws,
                    computerTo.Script.TryCatch
                };

                //atributos totales del atacante
                attacker = fillFighter(attackScripts);

                //atributos totales del defensor
                defender = fillFighter(defenseScripts);

                //calculo de los daños
                int attackDamage = attacker.Power - defender.Defense;
                int defenseDamage = defender.Power - attacker.Defense;

                //fase del atacante
                round(attackDamage, defenseScripts, focusAttack);

                //fase del defensor
                round(defenseDamage, attackScripts, focusDefense);

                currentAttackForce = 0;
                foreach (int i in attackScripts)
                    currentAttackForce += i;

                currentDefenseForce = 0;
                foreach (int i in defenseScripts)
                    currentDefenseForce += i;

                if (currentAttackForce == attacker.Quantity && currentDefenseForce == defender.Quantity)
                    break;

                //guardamos los resultados del hackOrder
                hackOrder.Variable = attackScripts[Script.VARIABLE];
                hackOrder.Conditional = attackScripts[Script.CONDITIONAL];
                hackOrder.Iterator = attackScripts[Script.ITERATOR];
                hackOrder.Json = attackScripts[Script.JSON];
                hackOrder.Class = attackScripts[Script.CLASS];
                hackOrder.BreakPoint = attackScripts[Script.BREAKPOINT];

                //guardamos los resultados del ordenador
                computerTo.Script.Variable = defenseScripts[Script.VARIABLE];
                computerTo.Script.Conditional = defenseScripts[Script.CONDITIONAL];
                computerTo.Script.Iterator = defenseScripts[Script.ITERATOR];
                computerTo.Script.Json = defenseScripts[Script.JSON];
                computerTo.Script.Class = defenseScripts[Script.CLASS];
                computerTo.Script.BreakPoint = defenseScripts[Script.BREAKPOINT];
                computerTo.Script.Throws = defenseScripts[Script.THROW];
                computerTo.Script.TryCatch = defenseScripts[Script.TRYCATCH];

            }
            while (currentAttackForce > 0 && currentDefenseForce > 0);

            //guardamos los resultados del hackOrder
            hackOrder.Variable = attackScripts[Script.VARIABLE];
            hackOrder.Conditional = attackScripts[Script.CONDITIONAL];
            hackOrder.Iterator = attackScripts[Script.ITERATOR];
            hackOrder.Json = attackScripts[Script.JSON];
            hackOrder.Class = attackScripts[Script.CLASS];
            hackOrder.BreakPoint = attackScripts[Script.BREAKPOINT];

            //si ganan los atacantes
            if (currentAttackForce > 0 && currentDefenseForce <= 0)
            {
                int carry = hackOrder.Json * sysScripts[Script.JSON].Carry + hackOrder.Class * sysScripts[Script.CLASS].Carry;
                hackOrder.Knowledge = (int)computerTo.Resource.Knowledge;
                hackOrder.Ingenyous = (int)computerTo.Resource.Ingenyous;
                hackOrder.Coffee = (int)computerTo.Resource.Coffee;

                //el atacante gana recursos
                if (hackOrder.Knowledge > carry / 3) hackOrder.Knowledge = carry / 3;
                if (hackOrder.Ingenyous > carry / 3) hackOrder.Ingenyous = carry / 3;
                if (hackOrder.Coffee > carry / 3) hackOrder.Coffee = carry / 3;

                //el ordenador pierde sus recursos
                computerTo.Resource.Knowledge -= hackOrder.Knowledge;
                computerTo.Resource.Ingenyous -= hackOrder.Ingenyous;
                computerTo.Resource.Coffee -= hackOrder.Coffee;
            }

            hackOrder.IsReturn = true;
            context.UpdateHackOrder(hackOrder);

            //guardamos los resultados del ordenador
            computerTo.Script.Variable = defenseScripts[Script.VARIABLE];
            computerTo.Script.Conditional = defenseScripts[Script.CONDITIONAL];
            computerTo.Script.Iterator = defenseScripts[Script.ITERATOR];
            computerTo.Script.Json = defenseScripts[Script.JSON];
            computerTo.Script.Class = defenseScripts[Script.CLASS];
            computerTo.Script.BreakPoint = defenseScripts[Script.BREAKPOINT];
            computerTo.Script.Throws = defenseScripts[Script.THROW];
            computerTo.Script.TryCatch = defenseScripts[Script.TRYCATCH];

            context.UpdateComputer(computerTo);

            return new int[]
            {
                currentAttackForce > 0 && currentDefenseForce <= 0 ? 1 : 0,
                hackOrder.Knowledge,
                hackOrder.Ingenyous,
                hackOrder.Coffee,
                hackOrder.Variable,
                hackOrder.Conditional,
                hackOrder.Iterator,
                hackOrder.Json,
                hackOrder.Class,
                hackOrder.BreakPoint,
                computerTo.Script.Variable,
                computerTo.Script.Conditional,
                computerTo.Script.Iterator,
                computerTo.Script.Json,
                computerTo.Script.Class,
                computerTo.Script.BreakPoint,
                computerTo.Script.Throws,
                computerTo.Script.TryCatch
            };
        }

        public void DoReturn()
        {
            SystemSoftware systemSoftware = context.GetSystemSoftware()[Software.MYSQL];
            int warehouse = int.Parse(systemSoftware.Action1.Split(',')[computerTo.Software.MySqlVersion]);

            //se retornan los recursos
            computerFrom.Resource.Knowledge += hackOrder.Knowledge;
            if (computerFrom.Resource.Knowledge > warehouse) computerFrom.Resource.Knowledge = warehouse;

            computerFrom.Resource.Ingenyous += hackOrder.Ingenyous;
            if (computerFrom.Resource.Ingenyous > warehouse) computerFrom.Resource.Ingenyous = warehouse;

            computerFrom.Resource.Coffee += hackOrder.Coffee;
            if (computerFrom.Resource.Coffee > warehouse) computerFrom.Resource.Coffee = warehouse;

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
        public void WriteAttackMessage(int[] report)
        {
            Message message = new Message(
                computerFrom.Id,
                "---",
                "Sistema",
                "Reporte del hacking",
                report[0] == 1 ?
                    string.Format("El intento de hacking al ordenador '{0}' ha sido todo un éxito.", computerTo.IpDirection) :
                    string.Format("El intento de hacking al ordenador '{0}' ha sido todo un fracaso.", computerTo.IpDirection)
                );

            context.CreateMessage(message);
        }

        public void WriteReceiverMessage(int[] report)
        {
            Message message = new Message(
                computerTo.Id,
                "---",
                "Sistema",
                "Reporte del hacking",
                report[0] == 1 ?
                    string.Format("El intento de hacking desde el ordenador '{0}' ha sido todo un éxito.", computerFrom.IpDirection) :
                    string.Format("El intento de hacking desde el ordenador '{0}' ha sido todo un fracaso.", computerFrom.IpDirection)
                );

            context.CreateMessage(message);
        }

        public void WriteReturnMessage()
        {
            Message message = new Message(
                computerFrom.Id,
                "---",
                "Sistema",
                "Retorno del hacking",
                string.Format("La misión de hacking con destino {0} ha regresado al ordenador. Vuelven los siguientes scripts: {1}{2}{3}{4}{5}{6}.",
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


        private Fighter fillFighter(int[] scripts)
        {
            Fighter fighter = new Fighter();
            for (int i = 0; i < scripts.Length; i++)
            {
                fighter.Quantity += scripts[i];
                fighter.Power += scripts[i] * (int)sysScripts[i].BasePower;
                fighter.Defense += scripts[i] * (int)sysScripts[i].BaseDefense;
                fighter.Integrity += scripts[i] * (int)sysScripts[i].BaseIntegrity;
            }
            return fighter;
        }

        private void round(int damage, int[] scripts, int[] focusList)
        {
            int focusIndex = 0;
            do
            {
                //se determina el foco
                int focus = focusList[focusIndex];

                //si no hay se sigue con el siguiente foco
                if (scripts[focus] == 0)
                {
                    focusIndex++;
                    continue;
                }
                    
                //se eliminan los scripts dañados
                int currentScripts = scripts[focus];
                scripts[focus] -= damage / (int)sysScripts[focus].BaseIntegrity;
                if (scripts[focus] < 0) scripts[focus] = 0;

                //se descuenta el poder de ataque
                damage -= currentScripts * (int)sysScripts[focus].BaseIntegrity;
                if (damage < 0) damage = 0;

                //se cambia el foco
                focusIndex++;
            }
            while (damage > 0 && focusIndex < focusList.Length);

        }
    }
}
