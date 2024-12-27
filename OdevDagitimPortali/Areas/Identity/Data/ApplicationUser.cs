using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations.Schema;
=======
>>>>>>> 69414b7d9e73ab87a30fa9f36aa951a195c8c4ae
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OdevDagitimPortali.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
<<<<<<< HEAD
        [PersonalData]
        [Column(TypeName ="nvarchar(100)")]
        public string user_name { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string password { get; set; }

=======
>>>>>>> 69414b7d9e73ab87a30fa9f36aa951a195c8c4ae
    }
}
