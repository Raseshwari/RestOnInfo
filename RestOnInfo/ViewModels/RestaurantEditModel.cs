using RestOnInfo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestOnInfo.ViewModels
{
    public class RestaurantEditModel
    {
        [Required, MaxLength(60)]
        public string Name { get; set; }
        public CuisineType Cuisine { get; set; }
    }
}
