
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
    public class PGPDecrypt
    {        

        [FunctionName("PGPDecrypt")]
        public async Task<string> Run([WorkflowActionTrigger] string Privatekey, string MessageContent)
        {
            //Stream privateKeyStream = new Stream(Privatekey);
            EncryptionKeys encryptionKeys = new EncryptionKeys(Privatekey,"password");
            var inputStream = GenerateStreamFromString(MessageContent);
            PGP pgp = new PGP(encryptionKeys);
            string DecryptMessage;
            using (Stream outputStream = new MemoryStream())                
            {
                await pgp.DecryptStreamAsync(inputStream, outputStream);
                outputStream.Position = 0;
                Console.WriteLine("Testing Decrypt method");
                StreamReader reader = new StreamReader(outputStream);
                DecryptMessage = reader.ReadToEnd();
                Console.WriteLine(DecryptMessage);
            }
            return DecryptMessage;
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