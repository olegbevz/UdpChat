namespace UdpChat.Common
{
    public enum MessageType
    {
        Login,      //Log into the server
        Logout,     //Logout of the server
        Message,    //Send a text message to all the chat clients
        Contacts,       //Get a list of users in the chat room from the server
        Null,        //No command
        Information
    }
}