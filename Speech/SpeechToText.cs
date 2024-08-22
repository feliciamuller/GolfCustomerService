using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCustomerService.Speech
{
    public class SpeechToText
    {
        public static SpeechConfig speechConfig;
        public static async Task Speech()
        {
            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                string cogSvcKey = configuration["CognitiveServiceKey"];
                string cogSvcRegion = configuration["CognitiveServiceRegion"];

                //Configure speech
                speechConfig = SpeechConfig.FromSubscription(cogSvcKey, cogSvcRegion);

                int choice;

                //Loop to make it possible to transcribe more questions
                do
                {
                    string command = "";
                    command = await TranscribeCommand();
                    Console.WriteLine("Press 1 if you want to record a new question.\nPress 9 to exit.");

                    string input = Console.ReadLine();
                    bool success = int.TryParse(input, out choice);
                    if(choice == 9)
                    {
                        Environment.Exit(9);
                    }
                } while (choice == 1);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static async Task<string> TranscribeCommand()
        {
            string command = "";

            //Configure speech recognizer and audio
            using AudioConfig audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using SpeechRecognizer speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);
            
            Console.WriteLine("Speech now...");

            try
            {
                SpeechRecognitionResult speech = await speechRecognizer.RecognizeOnceAsync();
                if (speech.Reason == ResultReason.RecognizedSpeech)
                {
                    command = speech.Text;
                    Console.WriteLine($"Your question: {command}");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(speech.Reason);
                    if (speech.Reason == ResultReason.Canceled)
                    {
                        var cancellation = CancellationDetails.FromResult(speech);
                        Console.WriteLine(cancellation.Reason);
                        Console.WriteLine(cancellation.ErrorDetails);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return command;
        }
    }
}
