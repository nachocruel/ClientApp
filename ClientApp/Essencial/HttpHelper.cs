using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientApp.Essencial
{
    public class HttpHelper
    {
        public static string CommandMount(string command)

        {
            Command cmd =  new Command(){ cmd = command, commandType = GetCommandType(command.Split(' ')[0]) };
            if(cmd.commandType != Commands.INVALIDCOMAND)
            {
                string strCMD = JsonConvert.SerializeObject(cmd);
                string httpCommand = "User-Agent: ClientApp/0.0.1" + Environment.NewLine
                                      + "Accept: */*" + Environment.NewLine
                                      + "Connection: keep-alive" + Environment.NewLine
                                      + "Content-Type: application/json; charset=utf-8" + Environment.NewLine
                                      + "Content-Length: " + strCMD.Length + Environment.NewLine + Environment.NewLine
                                      + strCMD;
                return strCMD;
            }
            
            return null;
        }


        private static Commands GetCommandType(string cmd)
        {
            switch (cmd.ToUpper())
            {
                case "ADMIN":
                    return Commands.ADMIN;
                case "CNOTICE":
                    return Commands.CNOTICE;
                case "CONNECT":
                    return Commands.CONNECT;
                case "DIE":
                    return Commands.DIE;
                case "HELP":
                    return Commands.HELP;
                case "KICK":
                    return Commands.KICK;
                case "JOIN":
                    return Commands.JOIN;
                case "LIST":
                    return Commands.LIST;
                case "NICK":
                    return Commands.NICK;
                case "OPER":
                    return Commands.OPER;
                case "PART":
                    return Commands.PART;
                case "PASS":
                    return Commands.PASS;
                case "PRIVMSG":
                    return Commands.PRIVMSG;
                case "PUBMSG":
                    return Commands.PUBMSG;
                case "CREATEROOM":
                    return Commands.CREATEROOM;
                case "QUIT":
                    return Commands.QUIT;
                case "WAITMESSAGE":
                    return Commands.WAITMESSAGE;
                default:
                    return Commands.INVALIDCOMAND;
            }
        }
    }
}
