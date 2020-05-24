﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarianaoWars.Models
{
    public class Script
    {
        public const int VARIABLE = 0;
        public const int CONDITIONAL = 1;
        public const int ITERATOR = 2;
        public const int JSON = 3;
        public const int CLASS = 4;
        public const int BREAKPOINT = 5;
        public const int THROW = 6;
        public const int TRYCATCH = 7;

        // Identificador del script. Primary Key autoincremental
        public int Id { get; set; }

        // Caza
        public int Variable { get; set; }

        // Destructor
        public int Conditional { get; set; }

        // Crucero
        public int Iterator { get; set; }

        // Nave carga
        public int Json { get; set; }

        // Nave colonizadora
        public int Class { get; set; }

        // Nave espía
        public int BreakPoint { get; set; }

        // Defensa 1
        public int Throws { get; set; }

        // Defensa 2
        public int TryCatch { get; set; }

        public Script()
        {
            Variable = 0;
            Conditional = 0;
            Iterator = 0;
            Json = 0;
            Class = 0;
            BreakPoint = 0;
            Throws = 0;
            TryCatch = 0;
        }

    }
}
