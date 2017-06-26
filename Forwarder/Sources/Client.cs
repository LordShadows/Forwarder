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
        static private Socket SERVERSOCKET;
        static private Thread LISTENTHREAD;
        static private String HOST = "127.0.0.1";
        static private int PORT= 22222;

        static Cryptography cryptography;

        static public bool InitClient()
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

                return true;
            }
            catch
            {
                Dialogs.Dialog.ShowDialogError("Связь с сервером не установлена. Сервер выключен или недоступен. Попробуйте позже.", "Ошибка соединения");
                Functions.Shutdown();
                return false;
            }
        }

        static public void SendMessage(String keyword, String[] textArguments)
        {
            String signature = cryptography.GetHash(textArguments);
            Message message = new Message(keyword, textArguments, signature);
            String json = JsonConvert.SerializeObject(message);
            SERVERSOCKET.Send(Encoding.UTF8.GetBytes(cryptography.Encrypt_AES_String(json)));
        }

        static public void Send(string Buffer)
        {
            SERVERSOCKET.Send(Encoding.UTF8.GetBytes(Buffer));
        }

        static public void HandleCommand(Message message)
        {
            try
            {
                if (cryptography.GetHash(message.TextArguments) != message.Signature)
                {
                    Dialogs.Dialog.ShowError("Целостность принятого пакета нарушена.", "Ошибка обработки");
                    return;
                }
                switch (message.Keyword)
                {
                    case "AuthenticationAttempt":
                        Functions.AuthenticationAttempt(message.TextArguments[0]);
                        break;
                    case "UpdateUsersData":
                        Functions.UpdateUsersData(message.TextArguments[0], message.TextArguments[1]);
                        break;
                    case "UpdateEngineersData":
                        Functions.UpdateEngineersData(message.TextArguments[0]);
                        break;
                    case "UpdateForwardersData":
                        Functions.UpdateForwardersData(message.TextArguments[0]);
                        break;
                    case "UpdateCompaniesData":
                        Functions.UpdateCompaniesData(message.TextArguments[0]);
                        break;
                    case "UpdateRequestsData":
                        Functions.UpdateRequestsData(message.TextArguments[0]);
                        break;
                    case "AccountData":
                        Functions.AccountData(message.TextArguments[0], message.TextArguments[1], message.TextArguments[2]);
                        break;
                }   
            }
            catch (NullReferenceException)
            {
                Dialogs.Dialog.ShowDialogError("Связь с сервером прервана. Приложение будет остановлено!", "Ошибка соединения");
                Functions.Shutdown();
            }
            catch (Exception exp)
            {
                Dialogs.Dialog.ShowError("Ошибка обработки команды сервера.", "Ошибка обработки");
                Console.WriteLine("Ошибка обработки команды сервера: " + exp.Message);
            }
        }

        static public void HandleDirectiveCommand(String message)
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
            catch (NullReferenceException)
            {
                Dialogs.Dialog.ShowDialogError("Связь с сервером прервана. Приложение будет остановлено!", "Ошибка соединения");
                Functions.Shutdown();
            }
            catch (Exception exp)
            {
                Dialogs.Dialog.ShowError("Ошибка обработки директивы сервера.", "Ошибка обработки");
                Console.WriteLine("Ошибка обработки директивы сервера: " + exp.Message);
            }
        }

        static public void Listner()
        {
            try
            {
                StringBuilder messageBuffer = new StringBuilder();
                while (SERVERSOCKET.Connected)
                {
                    byte[] buffer = new byte[1024];
                    int bytesReceive;
                    String message;

                    do
                    {
                        bytesReceive = SERVERSOCKET.Receive(buffer);
                        String temp = Encoding.UTF8.GetString(buffer, 0, bytesReceive);
                        if (temp.Contains("$END$"))
                        {
                            messageBuffer.Append(temp.Substring(0, temp.IndexOf("$END$")));
                            message = messageBuffer.ToString();
                            messageBuffer.Clear();
                            messageBuffer.Append(temp.Substring(temp.IndexOf("$END$") + 5));
                            break;
                        }
                        messageBuffer.Append(temp);
                    }
                    while (true);
               
                    if (message.Contains("$Directives$"))
                        HandleDirectiveCommand(message);
                    else
                        HandleCommand(JsonConvert.DeserializeObject<Message>(cryptography.Decrypt_AES_String(message)));
                }
            }
            catch
            {
                Dialogs.Dialog.ShowDialogError("Связь с сервером прервана. Приложение будет остановлено.", "Ошибка соединения");
                Functions.Shutdown();
            }
        }
    }
}
