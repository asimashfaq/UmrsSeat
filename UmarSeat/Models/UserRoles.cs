using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UmarSeat.Models
{
    public class UserRoles
    {
        [Key]
        public int id_UserRroles { get; set; }
        public string userRolesType { get; set; }
        public string userRolesName { get; set; }
        public int id_Subscription { get; set; }
    }

    public class ManageUserRoles
    {
        

       public string userId { get; set; }

       public string roleName { get; set; }
       public string branchName { get; set; }
       public List<SelectListItem> listUsers { get; set; }
       public List<SelectListItem> listRoles { get; set; }
       public List<SelectListItem> listBranches { get; set; }
    }
   
}