namespace NethereumSignerTests
{
    public class Order
    {
       
            public Order(string contract, string maker, string makerToken, string takerToken, string makerAmount, string takerAmount, string expires, string nonce, Signature signature)
            {
                this.contract = contract;
                this.maker = maker;
                this.makerToken = makerToken;
                this.takerToken = takerToken;
                this.makerAmount = makerAmount;
                this.takerAmount = takerAmount;
                this.expires = expires;
                this.nonce = nonce;
                this.signature = signature;
            }

            public Order(string v)
            {
            }

            public string contract { get; set; }
            public string maker { get; set; }
            public string makerToken { get; set; }
            public string takerToken { get; set; }
            public string makerAmount { get; set; }
            public string takerAmount { get; set; }
            public string expires { get; set; }
            public string nonce { get; set; }
            public Signature signature { get; set; }
        
    }
}