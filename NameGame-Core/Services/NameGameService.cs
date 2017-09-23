using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WillowTree.NameGame.Core.Models;

namespace WillowTree.NameGame.Core.Services
{
    public class NameGameService :INameGameService
    {
		private static readonly string DataUrl = "https://www.willowtreeapps.com/api/v1.0/profiles";

        private IEnumerable<Profile> profiles = null;

        public async Task<Profile[]> GetProfiles()
        {
            if (profiles == null)
            {
				HttpClient client = new HttpClient();
                var response = await client.GetAsync(DataUrl);
                var content = await response.Content.ReadAsStringAsync();
                profiles = (IEnumerable<Profile>)JsonConvert.DeserializeObject<IEnumerable<Profile>>(content);
			}
            
            Random rng = new Random();
            var profilesArray = profiles.ToArray();
            var returnArray = new Profile[5];
            int length = profiles.Count();

            for (int i = 0; i < 5; i++)
            {
                returnArray[i] = profilesArray[rng.Next(length)];
            }

            return returnArray;
        }
    }
}
