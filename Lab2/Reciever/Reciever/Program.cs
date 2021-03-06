﻿namespace Reciever
{
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Security.Cryptography;
    using System.Text;
    using Consumer;
    using Consumer.DataDestination;
    using Consumer.Domain.Converter;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;   
    using System.Collections.Generic;

    
    
    class Program
    {
        private static readonly RSA Rsa = RSA.Create();
        
        private static byte[] _desKey;
        private static readonly byte[] InitialVector = {1, 2, 3, 4, 5, 6, 7, 8, };
        
        private static string _serializedData;
        
        
        
        private static void Main()
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

                    consumer.Received += ReceivedDesAndDataByMessageQueue;

                    channel.BasicConsume(
                        queue: "default",
                        autoAck: true,
                        consumer: consumer);
                    
                    SendPublicRsaKey(channel, Rsa);
                    
                    ReceivedDesAndDataBySocket();
                }
            }

            var flatTableRows = JsonConvert.DeserializeObject<IEnumerable<FlatTableRow>>(_serializedData);

            flatTableRows
                .ConvertToEntities()
                .SaveToMySql();
        }

        private static void SendPublicRsaKey(IModel channel, RSA rsa)
        {
            var publicRsaKey = rsa.ExportParameters(includePrivateParameters: false);

            var serializedPublicRsaKey = JsonConvert.SerializeObject(publicRsaKey);

            var serializedPublicRsaKeyBytes = Encoding.UTF8.GetBytes(serializedPublicRsaKey);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "default",
                basicProperties: null,
                body: serializedPublicRsaKeyBytes);
        }

        private static void ReceivedDesAndDataByMessageQueue(object model, BasicDeliverEventArgs args)
        {
            var serializedMessageBytes = args.Body;

            FetchData(serializedMessageBytes);
        }
        
        private static void ReceivedDesAndDataBySocket()
        {
            var receiver = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
                    
            using (var socket = new Socket(receiver.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Bind(receiver);
                socket.Listen(10);

                using (var handler = socket.Accept())
                {
                    var buffer = new byte[1024 * 16];
                            
                    handler.Receive(buffer);
                            
                    FetchData(buffer);
                            
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
                        
                socket.Close();
            };
        }

        private static void FetchData(byte[] serializedMessageBytes)
        {
            var serializedMessage = Encoding.UTF8.GetString(serializedMessageBytes);

            var message = JsonConvert.DeserializeObject<Message>(serializedMessage);


            _desKey = Rsa.Decrypt(message.EncodedDes, RSAEncryptionPadding.Pkcs1);

            DecryptData(message.EncodedData);
        }
        
        private static void DecryptData(byte[] encryptedData)
        {   
            using (var des = DES.Create())
            {
                using (var stream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(stream, des.CreateDecryptor(_desKey, InitialVector), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(encryptedData, 0, encryptedData.Length);
                        
                        cryptoStream.FlushFinalBlock();
                    }

                    _serializedData = Encoding.UTF8.GetString(stream.ToArray());
                }
            }
        }

        class Message
        {
            public byte[] EncodedDes { get; set; }

            public byte[] EncodedData { get; set; }
        }
    }
}