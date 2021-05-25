using ClientApp.Essencial;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace ClientApp
{

    class ClientApp
    {
        private static UserRepo useRepo = UserRepo.getInstance();
        private static TcpClient client2;
        private System.Timers.Timer tmWait = new System.Timers.Timer();   
        private static int port;
        private static string ip_server;
        public ClientApp()
        {
            tmWait.Interval = 3000;
            tmWait.Elapsed += new System.Timers.ElapsedEventHandler(WaitMessage);
        }

        public void setConnection(string ip, int portS)
        {
            ip_server = ip;
            port = portS;
        }

        public void SendMessage(string command, TcpClient client)
        {
            try
            {
                string httpCMD = HttpHelper.CommandMount(command);
                if (httpCMD != null)
                {
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(httpCMD);
                    NetworkStream stream = client.GetStream();

                    stream.Write(data, 0, data.Length);

                    data = new Byte[4000];

                    String responseData = String.Empty;

                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    string body = responseData.Split('\n')[responseData.Split('\n').Length - 1];
                    if (!String.IsNullOrEmpty(body))
                    {
                        Response resp = JsonConvert.DeserializeObject<Response>(body);
                        Console.WriteLine(resp.message);
                        string cmd = command.Split(' ')[0];
                        if (cmd.ToUpper() == "NICK")
                        {
                            if (resp.success)
                            {
                                tmWait.Enabled = true;
                                Console.WriteLine("##Nick selected succefuly!");
                                User user = JsonConvert.DeserializeObject<User>(resp.message);
                                useRepo.setUser(user);
                            }
                        }
                    }
                }
                else
                    Console.WriteLine("Erro: Invalid command.");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public static void closeConnection()
        {
            if (client2 != null)
                client2.Close();
        }

        private void WaitMessage(object sender, ElapsedEventArgs e)
        {
            if (!cicleClosed)
            {
                if (client2 == null)
                    client2 = new TcpClient(ip_server, port);

                cicleClosed = !cicleClosed;
                IniciarListeningMessage("WAITMESSAGE" + " " +  useRepo.getUse().nickName, client2);
            }
        }


        static bool cicleClosed = false;
        public void IniciarListeningMessage(string command, TcpClient client)
        {
            try

            {
                string httpCMD = HttpHelper.CommandMount(command);
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(httpCMD);
                NetworkStream stream = client.GetStream();

                stream.Write(data, 0, data.Length);

                data = new Byte[4000];

                String responseData = String.Empty;

                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                string body = responseData.Split('\n')[responseData.Split('\n').Length - 1];
                if (!String.IsNullOrEmpty(body))
                {
                    Message message = JsonConvert.DeserializeObject<Message>(body);
                    Console.WriteLine("###Mensagem###" + " " + message.sendDate.ToString("dd/MM/yyyy HH:mm:ss"));
                    Console.WriteLine(message.userAuthor.nickName);
                    Console.WriteLine(message.text);
                    if (message.userDest != null)
                        Console.WriteLine("Enviado para: " + message.userDest.nickName);
                }
                cicleClosed = !cicleClosed;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}
