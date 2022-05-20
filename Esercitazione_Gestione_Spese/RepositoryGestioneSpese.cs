using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esercitazione_Gestione_Spese
{
    public static class RepositoryGestioneSpese
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GestioneSpese;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private static SqlDataAdapter InizializzaAdapter(SqlConnection conn)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();

            //fill

            adapter.SelectCommand = new SqlCommand("select * from Spese", conn);

            //prende la struttura delle chiavi

            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            //insert


            //adapter.InsertCommand = GeneraInsertSpesa(conn);


            //UPDATE

            adapter.UpdateCommand = GeneraUpdateApprovazione(conn);


            //DELETE

            adapter.DeleteCommand = GeneraDeleteSpesa(conn);

            return adapter;
        }

        public static SqlCommand GeneraDeleteSpesa(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from Spese where SId=@id";

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "Id"));

            return cmd;
        }

        
        public static SqlCommand GeneraUpdateApprovazione(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update Spese set Approvato=@approvato where Id=@id";

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "Id"));
            cmd.Parameters.Add(new SqlParameter("@approvato", SqlDbType.Bit, 0, "Approvato"));


            return cmd;
        }



        //public static SqlCommand GeneraInsertSpesa(SqlConnection conn)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = "insert into Spesa values(@data,@categoriaId,@descrizione,@utente,@importo,@approvato)";

        //    cmd.Parameters.Add(new SqlParameter("@data", SqlDbType.Date, 0, "DataSpesa"));
        //    cmd.Parameters.Add(new SqlParameter("@categoria", SqlDbType.Int, 0, "CategoriaId"));
        //    cmd.Parameters.Add(new SqlParameter("@descrizione", SqlDbType.VarChar, 500, "Descrizione"));
        //    cmd.Parameters.Add(new SqlParameter("@utente", SqlDbType.VarChar, 100, "Utente"));
        //    cmd.Parameters.Add(new SqlParameter("@importo", SqlDbType.Decimal, 10, "Importo"));
        //    cmd.Parameters.Add(new SqlParameter("@approvato", SqlDbType.Bit, 0, "Approvato"));


        //    return cmd;
        //}



        /// <summary>
        /// Connected mode
        /// </summary>
        public static void InserimentoSpesa()
        {


            using SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    Console.WriteLine("Connesso al DB");
                }
                else
                {
                    Console.WriteLine("Non connesso al DB");
                }



                Console.WriteLine("Inserire descrizione Spesa");
                string descrizione = Console.ReadLine();

                Console.WriteLine("Inserire importo Spesa");
                decimal importo;
                while (!decimal.TryParse(Console.ReadLine(), out importo) && importo > 0)
                {
                    Console.WriteLine("scelta non valida, inserire importo correttamente");
                }

                Console.WriteLine("Inserire nome Utente");
                string utente = Console.ReadLine();

                Console.WriteLine("Inserire Id Categoria Spesa");
                int CategoriaId;
                while (!int.TryParse(Console.ReadLine(), out CategoriaId) && CategoriaId > 0)
                {
                    Console.WriteLine("scelta non valida, inserire Id correttamente");
                }

                Console.WriteLine("Inserire la nuova data nel formato yyyy-mm-gg");
                string dataSpesa = Console.ReadLine();
                

                string Approvato = "0";

                string insertSql = $"insert into Spese values ('{dataSpesa}','{CategoriaId}','{descrizione}','{utente}','{importo}','{Approvato}')";

                SqlCommand insertCommand = conn.CreateCommand();

                insertCommand.CommandText = insertSql;

                int righeInserite = insertCommand.ExecuteNonQuery();

                if (righeInserite >= 1)
                {
                    Console.WriteLine($"Spesa inserita correttamente");
                }
                else
                {
                    Console.WriteLine("qualcosa é andato storto");
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }


        /// <summary>
        /// Disconnected mode
        /// </summary>
        public static void CancellaSpesa()
        {
            DataSet SpesaDS = new DataSet();

            using SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    Console.WriteLine("Connesso al DB");
                }
                else
                {
                    Console.WriteLine("Non connesso al DB");
                }



                SqlDataAdapter spesaAdapter = InizializzaAdapter(conn);
                spesaAdapter.Fill(SpesaDS, "SpesaDT");
                conn.Close();


                int idSpesaDaEliminare;
                Console.WriteLine("Inserire ID della spesa da eliminare");

                while (!int.TryParse(Console.ReadLine(), out idSpesaDaEliminare) && idSpesaDaEliminare > 0)
                {
                    Console.WriteLine("scelta non valida, inserire ID corretto");
                }

               
                DataRow rigaDaEliminare = SpesaDS.Tables["SpesaDT"].Rows.Find(idSpesaDaEliminare);

                if (rigaDaEliminare != null)
                {
                    rigaDaEliminare.Delete();

                    

                }


                

                spesaAdapter.Update(SpesaDS, "SpesaDT");

                Console.WriteLine("Spesa eliminata correttamente");

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore in cancella riga: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Disconnected mode
        /// </summary>
        
        public static void ApprovaSpesa()
        {
            DataSet SpesaDS = new DataSet();

            using SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    Console.WriteLine("Connesso al DB");
                }
                else
                {
                    Console.WriteLine("Non connesso al DB");
                }



                SqlDataAdapter spesaAdapter = InizializzaAdapter(conn);
                spesaAdapter.Fill(SpesaDS, "SpesaDT");
                conn.Close();


                int idSpesaDaApprovare;
                Console.WriteLine("Inserire ID della spesa da approvare");

                while (!int.TryParse(Console.ReadLine(), out idSpesaDaApprovare) && idSpesaDaApprovare > 0)
                {
                    Console.WriteLine("scelta non valida, inserire ID corretto");
                }

                
                
                DataRow rigaDaApprovare = SpesaDS.Tables["SpesaDT"].Rows.Find(idSpesaDaApprovare);

                


                if (rigaDaApprovare != null)
                {
                    

                    DateTime data = (DateTime)rigaDaApprovare["DataSpesa"];
                    Console.WriteLine($"Approvare Spesa {rigaDaApprovare["Descrizione"]} effettuata il {data.ToShortDateString()} da {rigaDaApprovare["Utente"]} ? ");
                    Console.WriteLine("premere Y per approvare, qualunque altro tasto per non approvare");

                    if (Console.ReadLine().ToLower() == "y")
                    {
                        rigaDaApprovare["Approvato"] = true;
                        spesaAdapter.Update(SpesaDS, "SpesaDT");
                        Console.WriteLine("Spesa Approvata correttamente");
                    }


                }
                else Console.WriteLine("riga non trovata");


               

                

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore in cancella riga: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }


        /// <summary>
        /// Connected mode
        /// </summary>
        
        public static void MostraTotalePerCategoria()
        {

            using SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    Console.WriteLine("Connesso al DB");
                }
                else
                {
                    Console.WriteLine("Non connesso al DB");
                }

                string query = "select Categorie.Categoria as Categoria, sum(Importo) as Somma from Spese join Categorie on Spese.CategoriaId=Categorie.Id group by Categoria ";
                
                SqlCommand comando = new SqlCommand();
                comando.Connection = conn;
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = query;

                SqlDataReader reader = comando.ExecuteReader();

                Console.WriteLine("-----------Spese per categoria------------\n");
                while (reader.Read())
                {

                    
                    var categoria = (string)reader["Categoria"];                   
                    var importo = (decimal)reader["Somma"];
                    Console.WriteLine($"{categoria} -- {importo} ");

                }




            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Connected mode
        /// </summary>
        public static void MostraSpeseUtente()
        {
            using SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    Console.WriteLine("Connesso al DB");
                }
                else
                {
                    Console.WriteLine("Non connesso al DB");
                }

                string query = "select * from Spese ";
                SqlCommand comando = new SqlCommand();
                comando.Connection = conn;
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = query;

                SqlDataReader reader = comando.ExecuteReader();

                Console.WriteLine("inserire nome Utente");
                string nomeUtente=Console.ReadLine();
                    

                Console.WriteLine($"-----------Spese {nomeUtente}------------\n");
                while (reader.Read())
                {

                    var id = (int)reader["Id"];
                    var descrizione = (string)reader["Descrizione"];
                    var dataSpesa = (DateTime)reader["DataSpesa"];
                    var utente = (string)reader["Utente"];
                    var importo = (decimal)reader["Importo"];
                    if (nomeUtente==utente)
                    Console.WriteLine($"{id} -- {descrizione} -- {dataSpesa.ToShortDateString()} -- {importo} ");

                }




            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Connected mode
        /// </summary>
        public static void MostraSpeseApprovate()
        {
            using SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    Console.WriteLine("Connesso al DB");
                }
                else
                {
                    Console.WriteLine("Non connesso al DB");
                }

                string query = "select * from Spese where Approvato=1";
                SqlCommand comando = new SqlCommand();
                comando.Connection = conn;
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = query;

                SqlDataReader reader = comando.ExecuteReader();

                Console.WriteLine("-----------Spese Approvate------------\n");
                while (reader.Read())
                {

                    var id = (int)reader["Id"];
                    var descrizione = (string)reader["Descrizione"];
                    var dataSpesa = (DateTime)reader["DataSpesa"];
                    var utente = (string)reader["Utente"];
                    var importo = (decimal)reader["Importo"];
                    Console.WriteLine($"{id} -- {descrizione} -- {dataSpesa.ToShortDateString()} -- {utente} -- {importo} ");

                }




            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
