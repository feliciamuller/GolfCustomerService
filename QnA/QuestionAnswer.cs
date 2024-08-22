using Azure;
using Azure.AI.Language.QuestionAnswering;
using GolfCustomerService.Speech;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCustomerService.QnA
{
    public class QuestionAnswer
    {
        public void AskQuestion()
        {
            try
            {
                //Configuring the connection to Azure
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                Uri endpoint = new Uri(configuration["Endpoint"]); 
                AzureKeyCredential credential = new AzureKeyCredential(configuration["ApiKey"]);
                string projectName = configuration["ProjectName"];
                string deploymentName = configuration["DeploymentName"];

                //Creating instance to make it possible to ask questions
                QuestionAnsweringClient client = new QuestionAnsweringClient(endpoint, credential);
                QuestionAnsweringProject project = new QuestionAnsweringProject(projectName, deploymentName);

                Console.WriteLine();
                Console.WriteLine("Ask a question.\nIf you don't find the answer press 3 to go back to menu to record your question.\nPress 9 to exit");

                int choice;

                while (true)
                {
                    Console.Write("Q: ");
                    string question = Console.ReadLine();

                    //Checking conditions depending on input from user
                    bool success = int.TryParse(question, out choice);
                    if (choice == 9)
                    {
                        Environment.Exit(9);
                    }
                    else if(choice == 3)
                    {
                        break;
                    }
                    try
                    {
                        Response<AnswersResult> response = client.GetAnswers(question, project);
                        foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
                        {
                            Console.WriteLine($"A: {answer.Answer}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
