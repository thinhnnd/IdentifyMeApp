using System.Threading.Tasks;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Configuration;

namespace IdentifyMe.App.Services.Interfaces
{
    public interface ICustomAgentContextProvider : IAgentProvider
    {
        bool AgentExists();

        Task<bool> CreateAgentAsync(AgentOptions options);
    }
}
