using System;
using System.Collections.Generic;
using System.Text;

namespace ClientApp
{
    public enum Commands
    {
        ADMIN, // Retorna informações sobre os administradores do servidor (ADMIN [<target>]). Target é um usuário ou servidor
        CNOTICE, // Notifica um usuário em um que esteja em um mesmo canal (CNOTICE <nickname> <channel> :<message>)
        CONNECT, // Conecta no servidor CONNECT <target server> [<port> [<remote server>]]
        DIE, // Desconecta do Servidor
        HELP, // Solicita ajuda nos comandos do servidor
        KICK, // Comando Utilizado para forçar a saída de algum usuário da sala (somente por usuários operadores da sala)
              // (KICK <channel> <client> :[<message>])
        JOIN, // Faz um usuário se juntar a sala (JOIN <channels> [<keys>])
        LIST, // Lista Todas as salas do servidor (LIST [<channels> [<server>]])
        NICK, // Escolhe um apelido ou altera o apelido (NICK <nickname> [<hopcount>] )
        OPER, // Loga um usuário como Operdor do servidor (OPER <username> <password>)
        PART, // Comando utilizado para sair da sala (PART <channels> [<message>])
        PASS, // Configura a senha de rede ([PASS nick password])
        PRIVMSG, // Envia um mensagem privada para um usuário ou sala (PRIVMSG <msgtarget> :<message>)
        PUBMSG, // Envia uma mensagem publicamente na sala. (PUBMSG <smgtarget> :<message>)
        CREATEROOM,
        QUIT,
        WAITMESSAGE,
        INVALIDCOMAND
    }
}
