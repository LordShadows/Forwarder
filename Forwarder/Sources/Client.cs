using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;

namespace Forwarder.Sources
{
    class Client
    {
        private Socket SERVERSOCKET;
        private Thread LISTENTHREAD;
        private const String HOST = "127.0.0.1";
        private const int PORT= 22222;

        Cryptography cryptography;
        Functions function;

        public Client()
        {
            try
            {
                IPAddress temp = IPAddress.Parse(HOST);
                SERVERSOCKET = new Socket(temp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                SERVERSOCKET.Connect(new IPEndPoint(temp, PORT));
                LISTENTHREAD = new Thread(Listner)
                {
                    IsBackground = true
                };
                LISTENTHREAD.Start();

                cryptography = new Cryptography();
                function = new Functions();
            }
            catch
            {
                System.Windows.MessageBox.Show("Связь с сервером не установлена.");
                App.Current.Shutdown();
            }
        }

        public void SendMessage(String keyword, String[] textArguments)
        {
            String signature = cryptography.GetHash(textArguments);
            Message message = new Message(keyword, textArguments, signature);
            String json = JsonConvert.SerializeObject(message);
            SERVERSOCKET.Send(cryptography.EncryptAES(json));
        }

        public void Send(string Buffer)
        {
            SERVERSOCKET.Send(Encoding.Unicode.GetBytes(Buffer));
        }

        public void HandleCommand(Message message)
        {
            try
            {
                switch (message.Keyword)
                {
                    case "Message":
                        Functions.AddJournalEntry(message.TextArguments[0]);
                        break;
                }   
            }
            catch (NullReferenceException ignore)
            {
                System.Windows.MessageBox.Show("Связь с сервером прервана");
                Functions.Shutdown();
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show("Error with handleCommand: " + exp.Message);
            }
        }

        public void HandleDirectiveCommand(String message)
        {
            try
            {
                message = message.Replace("$Directives$", "");
                if (message.Contains("$RSAPublicKey$"))
                {
                    String AESKeys = cryptography.GenerateAESKey(message.Replace("$RSAPublicKey$", ""));
                    Send("$Directives$" + "$AESKeys$" + AESKeys);
                }
            }
            catch (NullReferenceException ignore)
            {
                System.Windows.MessageBox.Show("Связь с сервером прервана");
                Functions.Shutdown();
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show("Error with HandleDirectiveCommand: " + exp.Message);
            }
        }

        public void Listner()
        {
            try
            {
                while (SERVERSOCKET.Connected)
                {
                    byte[] buffer = new byte[4096];
                    int bytesReceive = SERVERSOCKET.Receive(buffer);
                    String message = Encoding.Unicode.GetString(buffer, 0, bytesReceive);
                    if (message.Contains("$Directives$"))
                        HandleDirectiveCommand(message);
                    else
                        HandleCommand(JsonConvert.DeserializeObject<Message>(cryptography.DecryptAES(Encoding.Unicode.GetBytes(message))));
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Связь с сервером прервана");
                Functions.Shutdown();
            }
        }
    }
}
