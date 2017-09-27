using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WillowTree.NameGame.Core.Models;

namespace WillowTree.NameGame.Core.Services
{
    public class NameGameService : INameGameService
    {
        private static readonly string DataUrl = "https://www.willowtreeapps.com/api/v1.0/profiles";

        private static readonly string DefaultImageId = "5ZUiD3uOByWWuaSQsayAQ6";

        private IEnumerable<Profile> profiles = null;

        public async Task<IEnumerable<Profile>> GetProfiles(int numberOfProfiles)
        {
            // If the profiles have not yet been retrieved, eager load them and store them in 
            // a private IEnumerable field that can be queried.
            if (profiles == null)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(DataUrl);
                var content = await response.Content.ReadAsStringAsync();
                var allProfiles = JsonConvert.DeserializeObject<IEnumerable<Profile>>(content);
                // Filter out profiles that have no images or contain the default WillowTree image
                profiles = allProfiles.Where(p => p.Headshot.Url != null && p.Headshot.Id != DefaultImageId);
            }

            // Create a random number generator and fill a list with random indexes
            // from the profile set.
            Random rng = new Random();
            var indexList = new List<int>();
            int length = profiles.Count();
            int randomNumber;
            while (indexList.Count < numberOfProfiles)
            {
                randomNumber = rng.Next(length);
                if (!indexList.Contains(randomNumber))
                    indexList.Add(randomNumber);
            }

            // Fill a new array with random elements from the profile set and return it.
            var returnList = new List<Profile>();
            var indexArray = indexList.ToArray();
            for (int i = 0; i < numberOfProfiles; i++)
            {
                returnList.Add(profiles.ElementAt(indexArray[i]));
            }

            return returnList;
        }
    }
}
