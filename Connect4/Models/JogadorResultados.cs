using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4.Models
{
    public class JogadorResultados
    {
        public int Id { get; set; }
        public Jogador Jogador { get; set; }

        public Torneio Torneio { get; set; }

        public int Pontos { get; set; }
    }
}