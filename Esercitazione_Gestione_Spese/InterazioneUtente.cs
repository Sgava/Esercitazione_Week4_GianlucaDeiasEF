using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esercitazione_Gestione_Spese
{
    internal class InterazioneUtente
    {

        private static int Menu()
        {
            Console.WriteLine("\n\n---------------MENU----------");
            Console.WriteLine("1.Inserire una nuova Spesa");
            Console.WriteLine("2.Approvare Spesa esistente");
            Console.WriteLine("3.Cancellare Spesa Esistente");
            Console.WriteLine("4.Mostra elenco Spese approvate");
            Console.WriteLine("5.Mostra elenco Spese utente");
            Console.WriteLine("6.Mostra totale Spese per categoria");
            

            Console.WriteLine("\nInserisci la tua scelta:");

            int sceltaUtente;
            while (!(int.TryParse(Console.ReadLine(), out sceltaUtente) && sceltaUtente >= 0 && sceltaUtente <= 6))
            {
                Console.WriteLine("Scelta errata. Riprova..");
            }
            return sceltaUtente;
        }

        
        internal static void Start()
        {
            bool continua = true;
            while (continua)
            {
                int scelta = Menu();
                switch (scelta)
                {
                    case 1:
                        RepositoryGestioneSpese.InserimentoSpesa();
                        break;
                    case 2:
                        RepositoryGestioneSpese.ApprovaSpesa();
                        break;
                    case 3:
                        RepositoryGestioneSpese.CancellaSpesa();
                        break;
                    case 4:
                        RepositoryGestioneSpese.MostraSpeseApprovate();
                        break;
                    case 5:
                        RepositoryGestioneSpese.MostraSpeseUtente();
                        break;
                    case 6:
                        RepositoryGestioneSpese.MostraTotalePerCategoria();
                        break;
                    case 0:
                        continua = false;
                        Console.WriteLine("Arrivederci");
                        break;
                    default:
                        Console.WriteLine("Scelta errata.");
                        break;
                }
            }
        }

        private static void MostraTotalePerCategoria()
        {
            throw new NotImplementedException();
        }

        private static void MostraSpeseUtente()
        {
            throw new NotImplementedException();
        }

        private static void MostraSpeseApprovate()
        {
            throw new NotImplementedException();
        }

        private static void CancellaSpesa()
        {
            throw new NotImplementedException();
        }

        private static void ApprovaSpesa()
        {
            throw new NotImplementedException();
        }

   
    }   
}

