﻿using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Sender
{
    class Program
    {
        private static readonly byte[] DesKey = { 1, 3, 4, 2, 5, 9, 0, 11 };
        private static readonly byte[] InitialVector = {1, 2, 3, 4, 5, 6, 7, 8, };
        
        private static RSAParameters _publicRsaKey;
        private static bool _publicRsaKeyReceived;
        
        
        public static void Main()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "default",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += ReceivedPublicRsaKey;
                    
                    channel.BasicConsume(
                        queue: "default",
                        autoAck: true,
                        consumer: consumer);

                    while (!_publicRsaKeyReceived)
                        Thread.Sleep(500);

                    var encodedDesKey = EncodeDesKey();
                    var encodeData = EncodeData("hello");
                    
                    SendDesKeyAndData(channel, encodedDesKey, encodeData);
                }
            }
        }
        
        private static void ReceivedPublicRsaKey(object model, BasicDeliverEventArgs args)
        {
            var serializedPublicRsaKeyBytes = args.Body;
            
            var serializedPublicRsaKey = Encoding.UTF8.GetString(serializedPublicRsaKeyBytes);
            
            _publicRsaKey = JsonConvert.DeserializeObject<RSAParameters>(serializedPublicRsaKey);

            _publicRsaKeyReceived = true;
        }

        private static byte[] EncodeDesKey()
        {
            using (var rsa = RSA.Create(_publicRsaKey))
            {
                return rsa.Encrypt(DesKey, RSAEncryptionPadding.Pkcs1);
            }
        }

        private static byte[] EncodeData(string data)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            
            using (var des = DES.Create())
            {
                using (var stream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(stream, des.CreateEncryptor(DesKey, InitialVector), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                        
                        cryptoStream.FlushFinalBlock();
                    }

                    return stream.ToArray();
                }
            }
        }

        private static void SendDesKeyAndData(IModel channel, byte[] encodedDesKeyBytes, byte[] encodedDataBytes)
        {
            var message = new
            {
                EncodedDes = encodedDesKeyBytes,
                EncodedData = encodedDataBytes
            };

            var serialisedMessage = JsonConvert.SerializeObject(message);

            var serialisedMessageBytes = Encoding.UTF8.GetBytes(serialisedMessage);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "default",
                basicProperties: null,
                body: serialisedMessageBytes);
        }
    }
}