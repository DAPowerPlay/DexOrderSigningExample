using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Hex.HexConvertors.Extensions;
using NethereumSigner.Signing;
using NethereumSigningClient.Utils;

namespace NethereumSignerTests
{
    [TestClass]
    public class SignerTest
    {
        [TestMethod]
        public void SignedOrder_EcRecover_ReturnsMakerAddress()
        {
            var userWalletAddress = "0xYOURADDRESSHERE";
            var userPrivateKey = "0xPRIVATEKEY";
            //This is just for example purpose, store key somewhere safe and load it encrypted to your code and decrypt for usage here.
            var userDataProvider = new UserDataProvider("testAccount", userWalletAddress, userPrivateKey);
            var sigClient = new Signer(userDataProvider);
            //here is EtherDelta contract "0x8d12A197cB00D4747a1fe03395095ce2A5CC6819"
            var order = new Order("0x8d12A197cB00D4747a1fe03395095ce2A5CC6819", userDataProvider.UserWallet,
                "0x0000000000000000000000000000000000000000", "0xe41d2489571d322189246dafa5ebde1f4699f498", "10000000000000000", "380000000000000000,",
                "100000000", new Random().Next().ToString(), null);
            //there's address validation already in signer, so if singning fails 
            var signature = sigClient.Sign(order.contract, userDataProvider.UserWallet, order.makerToken, order.takerToken,
                order.makerAmount, order.takerAmount, order.expires, order.nonce);

            order.signature = new Signature(signature.V.First(), signature.R.ToHex(true), signature.S.ToHex(true));
            //there's address validation already in signer, so if singer fails to recover correct address, it will return null
            Assert.IsTrue(signature != null);
        }
    }
}
