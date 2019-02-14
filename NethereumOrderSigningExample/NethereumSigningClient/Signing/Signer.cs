using System;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.Encoders;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.KeyStore.Crypto;
using Nethereum.Signer;
using NethereumSigningClient.Utils;

namespace NethereumSigner.Signing
{
    public class Signer
    {
        private readonly UserDataProvider _userDataProvider;

        public Signer(UserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;
        }

        public EthECDSASignature Sign(string contract, string makerAddress, string makerToken, 
            string takerToken, string makerAmount, string takerAmount, string expires, string nonce)
        {
           
           var plainData = new Object[]
          {
               contract,
               takerToken,
               BigInteger.Parse(takerAmount),
               makerToken,
               BigInteger.Parse(makerAmount),
               BigInteger.Parse(expires),
               BigInteger.Parse(nonce)
          };

            var prms = new[] {
               new Parameter("address",1),
               new Parameter("address",1),
               new Parameter("uint256",1),
               new Parameter("address",1),
               new Parameter("uint256",1),
               new Parameter("uint256",1),
               new Parameter("uint256",1)
           };

           
           var data = SolidityPack(plainData, prms);
           
          
          
           var keystoreCrypto = new KeyStoreCrypto();
           //for etherDelta its SHA256, for IDEX just change with SHA3
           var hashed = keystoreCrypto.CalculateSha256Hash(data);
         
          
           var signer = new EthereumMessageSigner();
           var newHash = signer.HashPrefixedMessage(hashed);
           
            
           var signatureRaw = signer.SignAndCalculateV(newHash, _userDataProvider.PrivateKey);
           var signature = EthECDSASignature.CreateStringSignature(signatureRaw);
           var probe = signer.EcRecover(hashed, signature);
          
               
           var ethEcdsa = MessageSigner.ExtractEcdsaSignature(signature);

           if (probe == makerAddress)
                return ethEcdsa;
           //depending on usage, but it would be better to throw exc here if fails
           return null;

           //throw new Exception("Signing failed");

        }

        private byte[] SolidityPack(object[] paramsArray, Parameter[] paramsTypes)
        {
            var intTypeEncoder = new IntTypeEncoder();
            var cursor = new List<byte>();
            for (var i = 0; i < paramsArray.Length; i++)
            {
               
                if (paramsTypes[i].Type == "address")
                {
                    cursor.AddRange(EncodePacked(paramsArray[i]));
                }
                else if (paramsTypes[i].Type == "uint256")
                {
                    cursor.AddRange(intTypeEncoder.Encode(paramsArray[i]));
                }

            }

            return cursor.ToArray();
        }

        private byte[] EncodePacked(object value)
        {
            var strValue = value as string;

            if (strValue == null) throw new Exception("Invalid type for address expected as string");

            if (strValue != null
                && !strValue.StartsWith("0x", StringComparison.Ordinal))
                value = "0x" + value;

            if (strValue.Length == 42) return strValue.HexToByteArray();

            throw new Exception("Invalid address (should be 20 bytes length): " + strValue);
        }
    }
}
