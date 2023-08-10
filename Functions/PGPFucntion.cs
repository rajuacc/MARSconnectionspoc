
namespace PGPNamespace
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Azure.Functions.Extensions.Workflows;
    using Microsoft.Azure.WebJobs;
    using System.IO;
    using PgpCore;
    using System.Text;


    public class PGPFucntion
    {
        /// <summary>
        /// Executes the logic app workflow.
        /// </summary>
        /// <param name="zipCode">The zip code.</param>
        /// <param name="temperatureScale">The temperature scale (e.g., Celsius or Fahrenheit).</param>
        [FunctionName("PGPFucntion")]
        public async Task<string> Run([WorkflowActionTrigger] string key, string MessageContent)
        {
            var publickey = key;
            var body= MessageContent;
            string encrytpedmessage;
            var inputStream = GenerateStreamFromString(body);            
            EncryptionKeys encryptionKeys = new EncryptionKeys(publickey);
            PGP pgp = new PGP(encryptionKeys);
            using (Stream outputStream = new MemoryStream())
            {
                await pgp.EncryptStreamAsync(inputStream, outputStream);
                outputStream.Position = 0;

                Console.WriteLine("Testing Encrypt method");
                    //await _destinationService.SetFileStreamAsync(outputStream, "testpgp", inputFileStream.Name+ ".pgp");
                StreamReader reader = new StreamReader(outputStream);
                
                encrytpedmessage = reader.ReadToEnd();
                Console.WriteLine(encrytpedmessage);
            }

                return encrytpedmessage;
        }       

                
        private Stream GenerateStreamFromString(string s)
        {
        var stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}