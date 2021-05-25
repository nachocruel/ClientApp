using ClientApp.Essencial;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClientApp
{
    class Program
    {
        private static TcpClient client;
        private static int port;
        private static bool connected = false;
        private static bool died = false;
        private static string ip_server = null;
        private static UserRepo useRepo = UserRepo.getInstance();
        private static ClientApp clientApp = new ClientApp();
        static void Main(string[] args)
        {
            while (!died)
            {
                Console.WriteLine("Digite o comando ou 'HELP' para ajuda");
                string command = Console.ReadLine();

                if (!String.IsNullOrEmpty(command))
                {
                    if (!String.IsNullOrEmpty(command))
                        switch (command.Split(' ')[0].ToUpper())
                        {
                            case "CONNECT":
                                if (!connected)

                                {
                                    ip_server = command.Split(' ')[1];
                                    if (command.Split(' ').Length > 2)
                                        port = Int32.Parse(command.Split(' ')[2]);
                                    else
                                        port = 5000;

                                    if (!String.IsNullOrEmpty(ip_server))
                                    {

                                        client = new TcpClient(ip_server, port);
                                        connected = !connected;
                                        clientApp.setConnection(ip_server, port);
                                        Console.WriteLine("Connectado em: " + command.Split(' ')[1] + ":" + port);
                                    }
                                    else
                                        Console.WriteLine("Erro: IP not informed, Try again!");
                                }
                                else
                                    Console.WriteLine("Client is already connected!");
                                break;
                            case "DIE":
                                if (connected)
                                {
                                    User user = useRepo.getUse();
                                    if (user != null)
                                        clientApp.SendMessage(String.Format("QUIT {0}", user.nickName), client);
                                    Console.WriteLine("Success, Disconnected!");
                                    client.Close();
                                    ClientApp.closeConnection();
                                    connected = !connected;
                                    died = !died;
                                }
                                else
                                    Console.WriteLine("Client is not connected!");
                                break;
                            case "HELP":
                                string cmd = String.Empty;
                                if (command.Split(' ').Length > 1)
                                    cmd = command.Split(' ')[1];
                                Console.WriteLine(CommandList(cmd));
                                break;
                            default:
                                if (connected)
                                {
                                    string pzero = command.Split(' ')[0];
                                    if (useRepo.getUse() != null)
                                    {
                                        if (pzero.ToUpper() == "PUBMSG")
                                        {
                                            string []strSplit = command.Split(' ');
                                            string newMsg = strSplit[0] + " " + strSplit[1] + " ";
                                            for (int i = 2; i < strSplit.Length; i++)
                                                if (i < strSplit.Length-1)
                                                    newMsg += strSplit[i] + "#";
                                                else
                                                    newMsg += strSplit[i];

                                            command = newMsg;
                                            command += " " + useRepo.getUse().nickName;
                                        }

                                        if(pzero.ToUpper() == "PRIVMSG" || pzero.ToUpper() == "CNOTICE")
                                        {
                                            string nickAuthor = useRepo.getUse().nickName;
                                            string[] strSplit = command.Split(' ');

                                           
                                            string newMsg = strSplit[0] + " " + nickAuthor +
                                                " " + strSplit[1] + " " + strSplit[2] + " ";

                                            for (int i = 3; i < strSplit.Length; i++)
                                                if (i < strSplit.Length - 1)
                                                    newMsg += strSplit[i] + "#";
                                                else
                                                    newMsg += strSplit[i];
                                            command = newMsg;
                                        }

                                        if(pzero.ToUpper() == "PART")
                                        {
                                            command = "PART " + useRepo.getUse().nickName + " " + command.Split(' ')[1];
                                        }

                                        clientApp.SendMessage(command, client);
                                        Console.WriteLine("\n\n");
                                    }
                                    else
                                    {
                                        if (pzero.ToUpper() != "NICK")
                                            Console.WriteLine("Por favor, Selecione um apelido primeiro! Comando: 'NICK <youtnick>'.");
                                        else
                                        {
                                            clientApp.SendMessage(command, client);
                                        }
                                        Console.WriteLine("\n\n");
                                    }

                                }
                                else
                                    Console.WriteLine("Erro: O cliente não está conectado! Para conectar: CONNECT <server> <port>");
                                break;
                        }
                }
            }
        }

        private static string CommandList(string cmd)

        {
            if (String.IsNullOrEmpty(cmd))
                return Environment.NewLine + "ADMIN: Retorna informações sobre os administradores do servidor (ADMIN [<alvo>]). Alvo é um usuário ou servidor. (Nome do servidor: ServerAPP)" + Environment.NewLine
                    + "CNOTICE: Notifica um usuário em um que esteja em um mesmo canal (CNOTICE <apelido-destinatario> <sala> <message>)" + Environment.NewLine
                    + "CONNECT: Conecta no servidor(caso a porta não seja informada padrão será 5000) CONNECT <servidor> [<port>]" + Environment.NewLine
                    + "DIE: Desconecta do Servidor" + Environment.NewLine
                    + "HELP: Solicita ajuda nos comandos no comandos." + Environment.NewLine
                    + "JOIN: Faz um usuário se juntar a sala ([JOIN <apelido> <sala>]). Existe sala 'default' disponível', mas pode-se criar outras." + Environment.NewLine
                    + "LIST: Lista Todas as salas do servidor ([LIST])" + Environment.NewLine
                    + "NICK: Escolhe um apelido ou altera o apelido (NICK <apelido>)" + Environment.NewLine
                    + "PART: Comando utilizado para sair da sala (PART <sala>)" + Environment.NewLine
                    + "PRIVMSG: Envia um mensagem privada para um usuário (PRIVMSG <apelido-destinatario> <sala> <message>)" + Environment.NewLine
                    + "CREATEROOM: Criará um nova sala no servidor. (CREATEROOM <nome-da-sala>)" + Environment.NewLine
                    + "PUBMSG: Envia uma mensagem publicamente na sala. (PUBMSG <sala> :<message>). Obs: A mensagem deve estar entre Aspas." + Environment.NewLine + Environment.NewLine;
            else
            {
                switch (cmd.ToUpper())
                {
                    case "ADMIN":
                        return "ADMIN: Retorna informações sobre os administradores do servidor (ADMIN [<Alvo>]). Alvo é um usuário ou servidor"; ;
                    case "CNOTICE":
                        return "CNOTICE: Notifica um usuário em um que esteja em um mesmo canal (CNOTICE <apelido> <sala> <message>)";
                    case "CONNECT":
                        return "CONNECT: Conecta no servidor CONNECT <servidor> [<porta>]";
                    case "DIE":
                        return "DIE: Desconecta do Servidor";
                    case "HELP":
                        return "HELP: Solicita ajuda nos comandos no comandos.";
                    case "JOIN":
                        return "JOIN: Faz um usuário se juntar a sala (JOIN <apelido> [<sala>])";
                    case "LIST":
                        return "LIST: Lista Todas as salas do servidor ([LIST])";
                    case "NICK":
                        return "NICK: Escolhe um apelido ou altera o apelido (NICK <apelido>)";
                    case "PART":
                        return "PART: Comando utilizado para sair da sala (PART <sala>)";
                    case "PRIVMSG":
                        return "PRIVMSG: Envia um mensagem privada para um usuário (PRIVMSG <apelido-destinatario> <sala> <message>)";
                    case "PUBMSG":
                        return "PUBMSG: Envia uma mensagem publicamente na sala. (PUBMSG <sala> <message>). Obs: A mensagem deve estar entre Aspas";
                    default:
                        return "Comando inválido!";
                }
            }
        }
    }
}
