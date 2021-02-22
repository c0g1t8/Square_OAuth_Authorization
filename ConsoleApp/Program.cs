using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sample Console Application for Testing OAuth Proxy Server");

            // provide path to proxy server executable
            const string proxyPath = @".\AuthorizationProxy\bin\Debug\netcoreapp3.1";
            var message = RequestApplicationAuthorization(proxyPath);
            Console.WriteLine($"OAuth Response : '{message}'");

            Console.WriteLine("Finished");
        }

        /// <summary>
        /// Instantiates a local proxy server, initiates OAuth proxy process and waits for response.
        /// </summary>
        /// <param name="proxyPath"></param>
        /// <returns>OAuth response in json format</returns>
        private static string RequestApplicationAuthorization(string proxyPath)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-use-anonymous-pipes-for-local-interprocess-communication
            using (var server = new AnonymousPipeServerStream(
                PipeDirection.In,
                HandleInheritability.Inheritable))
            {
                string handle = server.GetClientHandleAsString();
                var client = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        UseShellExecute = false,
                        WorkingDirectory = proxyPath,
                        FileName = Path.Combine(proxyPath, "AuthorizationProxy.exe"),
                        Arguments = $"--handle {handle}"
                    }
                };
                var _ = client.Start();
                server.DisposeLocalCopyOfClientHandle();

                //*** call the proxy server through the default browser to start
                //*** application authorization process
                // https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
                // https://github.com/dotnet/runtime/issues/28005#issuecomment-735788443
                Process.Start(
                    new ProcessStartInfo("https://localhost:5001/authorizationrequest")
                    {
                        UseShellExecute = true
                    });

                //*** wait for response from proxy server
                string message;
                using (StreamReader sr = new StreamReader(server))
                {
                    message = sr.ReadLine();
                }

                //*** wait for client process to exit before continuing 
                client.WaitForExit();

                //*** the proxy server will automatically stop after message is received
                return message;
            }
        }
    }
}
