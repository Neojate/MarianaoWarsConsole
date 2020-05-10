﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarianaoWars.Models
{
    /*
     * Computer hace referencia al ordenador (planeta).
     */
    public class Computer
    {
        // ID del Ordenador. Primary Key Autoincremental
        public int Id { get; set; }

        // Nombre del ordenador.
        public string Name { get; set; }

        // Posición del ordenador en el universo. 192.168.x.y 
        public string IpDirection { get; set; }

        // Campo que indica si el Ordenador es la Capital
        public bool IsDesk { get; set; }

        // Indica el rango del ordenador. A más downloads, más rango.
        public int Downloads { get; set; }

        // Campo que señala cuantos programas puede almacenar el ordenador, en megas.
        public double Memmory { get; set; }

        // Campo que indica cuanta memoria se ha utilizado.
        public double MemmoryUsed { get; set; }

        // Campo que se relaciona con la tabla Resource.
        public int ResourceId { get; set; }

        // Campo que se relaciona con la tabla Software.
        public int SoftwareId { get; set; }

        // Campo que se relaciona con la tabla Talent.
        public int TalentId { get; set; }

        // Campo que se relaciona con la tabla AttackScript.
        public int AttackScriptId { get; set; }

        // Campo que se relaciona con la tabla DefenseScript.
        public int DefenseScriptId { get; set; }

        // Campo que se relaciona con la tabla Enrollment
        public int EnrollmentId { get; set; }

        public Enrollment Enrollment { get; set; }

        // Recurso que utiliza el ordenador.
        public Resource Resource { get; set; }

        // Software que utiliza el ordenador.
        public Software Software { get; set; }

        // Talento que utiliza el ordenador.
        public Talent Talent { get; set; }

        // Scripts de ataque almacenados en el ordenador.
        public AttackScript AttackScript { get; set; }

        // Scripts de defensa almacenados en el ordenador.
        public DefenseScript DefenseScript { get; set; }

        public Computer()
        {

        }

        public Computer(string name, string ipDirection, bool isDesk, Resource resource, Software software, Talent talent, AttackScript attackScript, DefenseScript defenseScript, Enrollment enrollment)
        {
            Name = name;
            IpDirection = ipDirection;
            IsDesk = isDesk;
            Downloads = 0;
            Memmory = 8;
            MemmoryUsed = 0;
            Resource = resource;
            Software = software;
            Talent = talent;
            AttackScript = attackScript;
            DefenseScript = defenseScript;
            Enrollment = enrollment;
        }

    }
}