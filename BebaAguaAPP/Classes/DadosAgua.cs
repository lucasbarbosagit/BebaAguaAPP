using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BebaAguaAPP.Classes
{
    class DadosAgua
    {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }
            public string ValorCopo { get; set; }
            public string ValorTotal { get; set; }
            public string Contador { get; set; }
    }
}