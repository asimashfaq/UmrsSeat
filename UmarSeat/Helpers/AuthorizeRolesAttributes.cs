using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace UmarSeat.Helpers
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }
    }
    public static class Role
    {
        public const string Administrator = "Administrator";
        public const string PNRReports = "PNR Reports";
        public const string ReadBooking = "ReadBooking";
        public const string UpdateBooking = "UpdateBooking";
        public const string DeleteBooking = "DeleteBooking";
        public const string CreatBooking = "CreatBooking";
        public const string ReadGroupSplit = "ReadGroupSplit";
        public const string UpdateGroupSplit = "UpdateGroupSplit";
        public const string DeleteGroupSplit = "DeleteGroupSplit";
        public const string CreateGroupSplit = "CreateGroupSplit";
        public const string ReadStockSell = "ReadStockSell";
        public const string UpdateStockSell = "UpdateStockSell";
        public const string DeleteStockSell = "DeleteStockSell";
        public const string CreatStockSell = "CreatStockSell";
        public const string ReadStockTransfer = "ReadStockTransfer";
        public const string UpdateStockTransfer = "UpdateStockTransfer";
        public const string DeleteStockTransfer = "DeleteStockTransfer";
        public const string CreateStockTransfer = "CreateStockTransfer";
        public const string StockReceive = "StockReceive";
        public const string ManageStockId = "ManageStockId";
        public const string ManageAgents = "ManageAgents";
        public const string ManageAirline = "ManageAirline";
        public const string Managebranches = "Managebranches";
        public const string ManageCategory = "ManageCategory";
        public const string ManageUsers = "ManageUsers";
        public const string ManageUserRoles = "ManageUserRoles";
    }
}