using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ShopApp.WebUI.Identity
{
    //User bilgileri olduğuyapı içerinden falanlar var genişletmek için kedi field oluşturabilirsin
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }

    }
}
