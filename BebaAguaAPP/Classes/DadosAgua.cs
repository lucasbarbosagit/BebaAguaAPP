using System;
//using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BebaAguaAPP.Classes
{
    public class DadosAgua
    {
          
            public long? Id { get; set; }
            public string ValorCopo { get; set; }
            public string ValorTotal { get; set; }
            public string Contador { get; set; }
            public string PegaAguaValor { get; set; }
    }
}