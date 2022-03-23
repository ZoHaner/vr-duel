using System;

namespace CodeBase.Data
{
    [Serializable]
    public class NetworkConfig
    {
        public bool IsActive = false;
        public string Scheme = "http";
        public string Host = "localhost";
        public int Port = 7350;
        public string ServerKey = "defaultkey";
    }
}