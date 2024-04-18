using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public struct RelayInformation
{
    public string relayServerCode;
    public string serverIp;
    public int serverPort;
    public byte[] key;
    public byte[] hostConnectionData;
    public byte[] connectionData;
    public byte[] allocationIdAsBytes;
    public System.Guid allocationId;
}
public class RelayManager : Singleton<RelayManager>
{
    private RelayInformation relayInfo;
    private bool isHost = false;
    public string ConnectionData => relayInfo.connectionData.ToString();
    public string AllocationId => relayInfo.allocationId.ToString();
    public bool IsHost => isHost;

    public async Task<string> CreateRelay(int maxConnections)
    {
        Allocation alloc = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        relayInfo.relayServerCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);

        RelayServerEndpoint endpoint = alloc.ServerEndpoints.First(connection => connection.ConnectionType == "dtls");
        SetRelayInformation(endpoint, alloc);

        isHost = true;
        return relayInfo.relayServerCode;
    }

    public async Task<bool> JoinRelay(string relayServerConnCode)
    {
        JoinAllocation alloc = await RelayService.Instance.JoinAllocationAsync(relayServerConnCode);

        RelayServerEndpoint endpoint = alloc.ServerEndpoints.First(connection => connection.ConnectionType == "dtls");
        SetRelayInformation(endpoint, alloc);

        return true;
    }

    public (byte[] AllocationId, byte[] Key, byte[] ConnectionData, string ServerIp, int Port) GetHostConnectionInfo()
        =>(relayInfo.allocationIdAsBytes, relayInfo.key, relayInfo.connectionData, relayInfo.serverIp, relayInfo.serverPort);

    public (byte[] AllocationId, byte[] Key, byte[] ConnectionData, byte[] HostConnectionData, string ServerIp, int Port) GetClientConnectionInfo()
        => (relayInfo.allocationIdAsBytes, relayInfo.key, relayInfo.connectionData, relayInfo.hostConnectionData, relayInfo.serverIp, relayInfo.serverPort);
    private void SetRelayInformation(RelayServerEndpoint endpoint, Allocation alloc)
    {
        relayInfo.serverIp = endpoint.Host;
        relayInfo.serverPort = endpoint.Port;
        relayInfo.allocationId = alloc.AllocationId;
        relayInfo.connectionData = alloc.ConnectionData;
        relayInfo.key = alloc.Key;
        relayInfo.allocationIdAsBytes = alloc.AllocationIdBytes;
    }

    private void SetRelayInformation(RelayServerEndpoint endpoint, JoinAllocation alloc)
    {
        relayInfo.serverIp = endpoint.Host;
        relayInfo.serverPort = endpoint.Port;
        relayInfo.allocationId = alloc.AllocationId;
        relayInfo.connectionData = alloc.ConnectionData;
        relayInfo.key = alloc.Key;
        relayInfo.allocationIdAsBytes = alloc.AllocationIdBytes;
        relayInfo.hostConnectionData = alloc.HostConnectionData;
    }
}
