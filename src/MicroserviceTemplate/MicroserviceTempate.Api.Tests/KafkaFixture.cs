using Confluent.Kafka;
using System.Net;
using System.Net.Sockets;
using Testcontainers.Kafka;

namespace RulesEngine.Api.IntegrationTests;
public class KafkaFixture : IAsyncLifetime
{
    private readonly KafkaContainer kafkaContainer;
    private int kafkaHostPort;

    public KafkaFixture()
    {
        kafkaHostPort = GetAvailablePort();
        var kafkaContainerName = $"kafka_{GenerateRandomString(5)}";
        kafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:7.0.1")
            .WithName(kafkaContainerName)
            .WithHostname(kafkaContainerName)
            .WithPortBinding(kafkaHostPort, 9092)
          .Build();
    }


    public Task DisposeAsync() => kafkaContainer.StopAsync();

    public async Task InitializeAsync()
    {
        await kafkaContainer.StartAsync();
        await CreateKafkaTopic("topic-name", $"localhost:{kafkaHostPort}");

    }

    private static async Task CreateKafkaTopic(string topicName, string bootstrapServers)
    {
        using var adminClient =
            new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();

        await adminClient.CreateTopicsAsync(
        [
            new() { Name = topicName, ReplicationFactor = 1, NumPartitions = 1 }
        ]);
    }

    private static int GetAvailablePort()
    {
        IPEndPoint defaultLoopbackEndpoint = new(IPAddress.Loopback, 0);

        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(defaultLoopbackEndpoint);
        int port = (socket.LocalEndPoint as IPEndPoint)!.Port;

        return port;
    }

    public static string GenerateRandomString(int length)
    {
        Random random = new();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
