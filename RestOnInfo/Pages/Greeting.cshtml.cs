using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestOnInfo.Services;

namespace RestOnInfo.Pages
{
    public class GreetingModel : PageModel
    {
        private IGreeter _greeter;

        public string currentGreeting { get; set; }

        public GreetingModel(IGreeter greeter)
        {
            _greeter = greeter;
        }
        public void OnGet(string name)
        {
            currentGreeting = $"{name}: {_greeter.GetMessageOfTheDay()}";
        }
    }
}