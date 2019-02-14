namespace NethereumSigningClient.Utils
{
    public class UserDataProvider
    {
        public UserDataProvider(string name, string userWallet, string privateKey)
        {
            Name = name;
            UserWallet = userWallet;
            PrivateKey = privateKey;
        }

        public string Name { get; set; }

        public string UserWallet { get; set; }

        public string PrivateKey { get; set; }
    }
}