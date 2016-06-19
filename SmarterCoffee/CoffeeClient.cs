using System;
using System.Net;
using System.Net.Sockets;
namespace SmarterCoffee
{
    public enum CoffeeStrength
    {
        Low = 0x00,
        Medium = 0x01,
        High = 0x02
    }

    public class CoffeeClient
    {
        TcpClient client;
        TcpClient sclient;
        NetworkStream statusStream;
        const int bufferSize = 1;
        string address;
        int port;
        NetworkStream stream;
        byte[] statusBuffer = new byte[6];

        public WaterLevelMessage Water {
            get;
            private set;
        }

        public StrengthLevelMessage Strength {
            get;
            private set;
        }

        public CupsMessage Cups {
            get;
            private set;
        }

        public StatusMessage Status {
            get;
            private set;
        }


        public CoffeeClient (string Address,int Port = 2081)
        {
            address = Address;
            port = Port;
            client = new TcpClient();
            try
            {
                maintainConnection();
            }
            catch(SocketException e){
                throw new Exception("Failed to connect to coffee machine :(",e);
            }
        }

        public void StartStatusThread(){
            sclient = new TcpClient(address,port);
            statusStream = sclient.GetStream();
            statusStream.BeginRead(statusBuffer,0,6,onStatus,null);
        }

        public void onStatus(IAsyncResult result){
            if(statusBuffer[0] == 0x14)//Not a status
            {
                //Read this
                Status = new StatusMessage(statusBuffer[1]);
                Water = new WaterLevelMessage(statusBuffer[2]);
                //WIFI level
                Strength = new StrengthLevelMessage(statusBuffer[4]);
                Cups = new CupsMessage(statusBuffer[5]);
            }
            statusStream.BeginRead(statusBuffer,0,6,onStatus,null);
        }
        
        private void maintainConnection(){
            if(!client.Connected){
                client.Connect(address,port);
                stream = client.GetStream();
            }
        }

        private ReturnStatus sendCommand(byte cmd,byte[] data){
            maintainConnection();
            stream.WriteByte(cmd);
            stream.Write(data,0,data.Length);
            stream.WriteByte(0x7e);//Footer
            stream.Flush();
            byte[] buffer = new byte[bufferSize];
            stream.Read(buffer,0,bufferSize);
            return new ReturnStatus(buffer[0]);
        }

        public void SetStrength(CoffeeStrength strength){
            ReturnStatus result = sendCommand(0x35,new byte[1]{(byte)strength});

        }

        public void SetCups(int cups){
            if(cups < 1 || cups > 12){
                throw new Exception("Cups must be between 1 and 12");
            }
            ReturnStatus result = sendCommand(0x36,new byte[1]{(byte)cups});
        }
        
        public void ToggleGrinder(){
            ReturnStatus result  = sendCommand(0x3c,new byte[0]);
        }

        public void TurnOnHotplate(int time = 5){
            if(time < 5){
                throw new Exception("Time must be a minimum of 5 (minutes)");
            }
            ReturnStatus result  = sendCommand(0x3e,new byte[1]{(byte)time});
        }

        public void TurnOffHotplate(){
            ReturnStatus result  = sendCommand(0x4a,new byte[0]);
        }


    }
}

