using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestOnInfo.Models;
using RestOnInfo.Services;
using RestOnInfo.ViewModels.Home;

namespace RestOnInfo.Pages
{
    public class IndexModel : PageModel
    {
        private IRestaurantData _restaurantData;
        private IGreeter _greeter;

        public IEnumerable<Restaurant> restaurants { get; set; }

        public IndexModel(IRestaurantData restaurantData,
            IGreeter greeter)
        {
            _restaurantData = restaurantData;
            _greeter = greeter;
        }
        public void OnGet()
        {
            var model = new HomeIndexViewModel();
            model.Restaurants = _restaurantData.GetAll();
            model.CurrentMessage = _greeter.GetMessageOfTheDay();

            restaurants = model.Restaurants;
        }
    }
}