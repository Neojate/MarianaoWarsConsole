using MarianaoWars.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarianaoWarsConsole.Logic
{
    public class AttackHackOrder
    {
        private Service context;
        private HackOrder hackOrder;
        private Computer computerTo;

        public AttackHackOrder(Service service, HackOrder hackOrder, Computer computerTo)
        {
            context = service;
            this.hackOrder = hackOrder;
            this.computerTo = computerTo;
        }

        public int[] DoAttack()
        {
            return null;
        }
    }
}
