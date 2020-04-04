using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Utils;
using Hyperledger.Aries.Features;
using Hyperledger.Aries.Ledger;
using Hyperledger.Aries.Extensions;
using Hyperledger.Aries.Decorators;
using Hyperledger.Aries.Models;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Contracts;
using Hyperledger.Indy;
using System.Net.Http;

namespace IdentifyMe.App.Services
{
    public class MediatorService
    {
        MediatorService(IHttpClientFactory httpClientFactory, DefaultAgent defaultAgent)
        {
            Hyperledger.Aries.Agents.HttpMessageDispatcher dispatcher = new HttpMessageDispatcher(httpClientFactory);
            string msg = "Helloword";
            byte[] msgBuffer = Encoding.ASCII.GetBytes(msg);

            PackedMessageContext packedMessage = new PackedMessageContext(msgBuffer);
            UnpackResult unpack = Hyperledger.Indy.CryptoApi.Crypto.UnpackMessageAsync(wallet, msgBuffer);
        }

        //public async Task<UnpackedMessageContext> UnpackAsync(Wallet wallet, PackedMessageContext message)
        //{
        //    UnpackedMessageContext a = new UnpackedMessageContext();
        //}
    }
}
