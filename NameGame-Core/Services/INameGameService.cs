using System;
using System.Threading.Tasks;
using WillowTree.NameGame.Core.Models;

namespace WillowTree.NameGame.Core.Services
{
    public interface INameGameService
    {
        Task<Profile[]> GetProfiles();
    }
}
