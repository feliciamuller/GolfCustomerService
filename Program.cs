using GolfCustomerService.QnA;
using GolfCustomerService.Speech;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace GolfCustomerService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            QuestionAnswer question = new QuestionAnswer { };
            bool success;
            int choice;

            while(true)
            {
                Console.WriteLine("\r\n ██████   ██████  ██      ███████     ███████  █████   ██████  \r\n██       ██    ██ ██      ██          ██      ██   ██ ██    ██ \r\n██   ███ ██    ██ ██      █████       █████   ███████ ██    ██ \r\n██    ██ ██    ██ ██      ██          ██      ██   ██ ██ ▄▄ ██ \r\n ██████   ██████  ███████ ██          ██      ██   ██  ██████  \r\n                                                          ▀▀   \r\n                                                               \r\n");

                Console.WriteLine("Welcome to the ultimate golf FAQ!\n\nPress 1 to type in your question.\n\nPress 2 to record your question if you don't find the answer. " +
                    "\nAn golf agent will look in to your question.\n\nPress 9 to exit.");
                Console.WriteLine("--------------------------");
                string input = Console.ReadLine();
                
                success = int.TryParse(input, out choice);

                if (!success)
                {
                    Console.WriteLine("Invalid input, you must type in a number");
                }
                switch (choice)
                {
                    case 1:
                        question.AskQuestion();
                        break;
                    case 2:
                        await SpeechToText.Speech();
                        break;
                    case 9:
                        Environment.Exit(9);
                         break;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        continue;
                }
            }
        }
    }

}
