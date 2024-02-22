using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;

namespace ConsultaNumerosOlos;

class Program : ComparaOlos
{
    static async Task Main()
    {
        var connectionString = "Data Source=172.22.0.90;Initial Catalog=numerosProcon;User Id=kaiky;Password='xBU#3@p7';";
        var diretorio = @"\\172.22.0.82\ti\4 - Novos Olos";

        try
        {
            Console.WriteLine("Consultando Numeros...");
            Consulta(connectionString, diretorio);

            Console.WriteLine("Terminado;");
        }

        catch(Exception ex) 
        {
            Console.WriteLine($"Algo deu errado. Erro: {ex.Message}");
        }
    }
}
