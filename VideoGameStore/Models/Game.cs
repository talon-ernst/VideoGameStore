﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VideoGameStore.Models
{
    public class Game
    {      
        [Key]
        public Guid gameGuid { get; set; }
        public string gameTitle { get; set; }
        public string gameDescription { get; set; }
        public DateTime gameReleaseDate { get; set; }



        public Game()
        {

        }
    }


}