using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestingApp.Models;

namespace TestingApp.ViewModels
{
     public class StorePageView_Model
    {
         public ObservableCollection<Item> obs_Food { get; set; }
         public ObservableCollection<Item> obs_Drink { get; set; }
         
        public StorePageView_Model() 
        {
              obs_Food = new ObservableCollection<Item>();
              obs_Drink = new ObservableCollection<Item>();
              GetJson();
        }


        public async void GetJson() 
        {

              try
              {
                var client = new HttpClient();
                var response = await client.GetAsync("http://hello987.azurewebsites.net/store.html");
                var result = await response.Content.ReadAsStringAsync();

                var rootObject = JsonConvert.DeserializeObject<RootStore>(result);

                foreach (Item everyItemInList in rootObject.items)
                {
                    if (everyItemInList.tag.Equals("food"))
                    {
                        obs_Food.Add(everyItemInList);
                    }
                    if (everyItemInList.tag.Equals("drink"))
                    {
                        obs_Drink.Add(everyItemInList);

                    }
                }

            }
            catch (Exception exc)
            {
            }
        }
    }
}
