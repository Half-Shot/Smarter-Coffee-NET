using System;
using System.Collections.Generic;
namespace SmarterCoffee
{
    public class Message
    {
        readonly byte Code;
        protected readonly string Status;
        public Message(byte Code,Dictionary<byte,string> msgs){
            this.Code = Code;
            this.Status = GetMsg(msgs);
        }
        
        public static bool operator ==(Message msgA,Message msgB)
        {
            return msgA.Code == msgB.Code;
        }

        public static bool operator !=(Message msgA,Message msgB)
        {
            return msgA.Code != msgB.Code;
        }

        protected string GetMsg(Dictionary<byte,string> msgs){
            if(msgs.ContainsKey(Code)){
                return msgs[Code];
            }
            else
            {
                #if DEBUG
                Console.WriteLine("Unknown code", Code);
                #endif
                return "Unknown Code";
            }
        }
    }

    


    public class StatusMessage : Message
    {
        private static Dictionary<byte,string> msgs = new Dictionary<byte,string>(){
            {0x4,"Filter, ?"},
            {0x5 , "Grinder, ?"},
            {0x6 , "Filter, OK to start"},
            {0x7 , "Grinder, OK to start"},
            {0x20 , "Filter, No carafe"},
            {0x22 , "Grinder, No carafe"},
            {0x45 , "Filter, Done"},
            {0x47 , "Grinder, Done"},
            {0x53 , "Boiling"},
            {0x60 , "Filter, No carafe, Hotplate On"},
            {0x61 , "Filter, Hotplate On"},
            {0x62 , "Grinder, No carafe, Hotplate On"},
            {0x63 , "Grinder, Hotplate On"}
        };
        
        public StatusMessage(byte Code) : base(Code,msgs){

        }
    }

    public class WaterLevelMessage : Message {

        private static Dictionary<byte,string> msgs = new Dictionary<byte,string>()
        {
            {0x2,"Empty"},
            {0x12 , "Half"},
            {0x13, "Full"},
        };

        public WaterLevelMessage(byte Code) : base(Code,msgs){

        }
    }

    public class StrengthLevelMessage : Message {

        private static Dictionary<byte,string> msgs = new Dictionary<byte,string>()
        {
            {0x0,"Low"},
            {0x1 , "Medium"},
            {0x2, "High"},
        };

        public StrengthLevelMessage(byte Code) : base(Code,msgs){

        }
    }

    public class CupsMessage : Message {
        //TODO: Cups
        private static Dictionary<byte,string> msgs = new Dictionary<byte,string>()
        {
            {0x0,"Low"},
            {0x1 , "Medium"},
            {0x2, "High"},
        };

        public CupsMessage(byte Code) : base(Code,msgs){

        }
    }

    public class ReturnStatus : Message {

        private static Dictionary<byte,string> msgs = new Dictionary<byte,string>()
        {
            {0x0, "OK"},
            {0x1, "Error: Brewing"},
            {0x2, "Error: No carafe"},
            {0x3, "Error: Not enough water"},
            {0x4, "Error: You sent the wrong value"}
        };

        public ReturnStatus(byte Code) : base(Code,msgs){

        }
    }

    
}

