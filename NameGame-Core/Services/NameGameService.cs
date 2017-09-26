using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WillowTree.NameGame.Core.Models;
using MvvmCross.Platform;

namespace WillowTree.NameGame.Core.Services
{
    public class NameGameService : INameGameService
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
                var allProfiles = JsonConvert.DeserializeObject<IEnumerable<Profile>>(content);
                profiles = allProfiles.Where(p => p.Headshot.Url != null && p.Headshot.Id != "5ZUiD3uOByWWuaSQsayAQ6"); 
			}

            Random rng = new Random();
            var indexList = new List<int>();
			int length = profiles.Count();
            int randomNumber;
            while (indexList.Count < 5)
            {
                randomNumber = rng.Next(length);
                if (!indexList.Contains(randomNumber))
                    indexList.Add(randomNumber);
            }
            var returnArray = new Profile[5];
            var indexArray = indexList.ToArray();
            for (int i = 0; i < 5; i++)
            {
				returnArray[i] = profiles.ElementAt(indexArray[i]);
			}

            return returnArray;
        }
    }
}
