// This interface represents an abstraction for the NameGameService that will
// retrieve the list of profiles from the NamgeGame API

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using WillowTree.NameGame.Core.Models;

namespace WillowTree.NameGame.Core.Services
{
    public interface INameGameService
    {
        Task<IEnumerable<Profile>> GetProfiles(int numberOfProfiles);
    }
}
