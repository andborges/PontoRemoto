namespace PontoRemoto.Application.Encryption
{
    public class ApplicationAesEncryptionInfo
    {
        public string Password
        {
            get { return "IcnP12msKQrzfBcrJ"; }
        }

        public string Salt
        {
            get { return "PontoRemoto"; }
        }

        public int PasswordIterations
        {
            get { return 2; }
        }

        public string InitialVector
        {
            get { return "DJBar03x#hqo50vL"; }
        }

        public int KeySize
        {
            get { return 256; }
        }
    }
}