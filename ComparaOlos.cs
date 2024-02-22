using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace ConsultaNumerosOlos
{
    public class ComparaOlos
    {
        public static async Task Consulta(string connectString, string diretorio)
        {
            Console.WriteLine("Conectando ao banco...");
            Console.WriteLine();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectString))
                {
                    connection.Open();

                    Console.WriteLine("Conectado ao banco.");
                    Console.WriteLine();
                    var selectQuery = "SELECT DDD, Telefone FROM NumerosDiscador";

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Verifica se há linhas retornadas
                            if (reader.HasRows)
                            {
                                Console.WriteLine("Separando txt");

                                int contadorLinhas = 0;
                                int contadorArquivos = 1;
                                int contadorGlobal = 10000; // Inicializa o contador global a partir de 10000
                                StreamWriter writer = null;

                                // Loop através das linhas retornadas
                                while (reader.Read())
                                {
                                    // Cria um novo arquivo a cada 99.000 registros
                                    if (contadorLinhas % 99000 == 0)
                                    {
                                        // Fecha o writer anterior, se existir
                                        if (writer != null)
                                        {
                                            writer.Close();
                                        }

                                        string path = Path.Combine(diretorio, $"BLOCKLIST_{contadorArquivos}.txt");

                                        // Cria um novo StreamWriter para o próximo arquivo
                                        writer = new StreamWriter(path);

                                        // Remove as aspas dos valores antes de escrever no arquivo
                                        string telefone = reader["Telefone"].ToString().Trim('\"');
                                        string telefoneSemDoisPrimeirosDigitos = telefone.Length >= 2 ? telefone.Substring(2) : ""; // Remove os dois primeiros números, se houver pelo menos dois números
                                        writer.WriteLine($"{contadorGlobal};{reader["DDD"].ToString().Trim('\"')};{telefoneSemDoisPrimeirosDigitos}");

                                        Console.WriteLine($"{contadorGlobal};");
                                        Console.WriteLine($"{telefoneSemDoisPrimeirosDigitos}");

                                        contadorArquivos++;
                                    }
                                    else
                                    {
                                        // Adiciona os valores das colunas ao arquivo existente
                                        string telefone = reader["Telefone"].ToString().Trim('\"');
                                        string telefoneSemDoisPrimeirosDigitos = telefone.Length >= 2 ? telefone.Substring(2) : ""; // Remove os dois primeiros números, se houver pelo menos dois números
                                        writer.WriteLine($"{contadorGlobal};{reader["DDD"].ToString().Trim('\"')};{telefoneSemDoisPrimeirosDigitos}");

                                        Console.WriteLine($"{contadorGlobal};");
                                    }

                                    contadorLinhas++;
                                    contadorGlobal++;
                                }

                                // Fecha o writer após o loop
                                if (writer != null)
                                {
                                    writer.Close();
                                }

                                Console.WriteLine($"Consulta concluída. Dados salvos em arquivos no diretório: {diretorio}");
                            }
                            else
                            {
                                Console.WriteLine("Nenhum dado encontrado na tabela NumerosDiscador.");
                            }

                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro: " + ex.Message);
            }
        }
    }
}
