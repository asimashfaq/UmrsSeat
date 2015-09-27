using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UmarSeat.Models
{
    public class menulinks
    {
        public int linkId { get; set; }
        public string url { get; set; }
        public string dtext { get; set; }
        public string thumbtext { get; set; }
        public int parentId { get; set; }
        public string linktype { get; set; }
        public string roleName { get; set; }

      
    }
    public class menuList
    {
        public List<menulinks> menu;
        public menuList()
        {
            menu = new List<menulinks>();
            var menu2 = new List<menulinks>();
            menu.Add(new menulinks() { linkId = 1, url = "/", dtext = "Home", thumbtext = "Ho", parentId = 0, roleName = "" });
            menu.Add(new menulinks() { linkId = 2, url = "javascript:;", dtext = "Reports", thumbtext = "Re", parentId = 0, roleName = "PNR Reports,Administrator" });
            menu.Add(new menulinks() { linkId = 3, url = "/pnr", dtext = "PNR", thumbtext = "pnr", parentId = 2, roleName = "PNR Reports,Administrator" });
            menu.Add(new menulinks() { linkId = 4, url = "/pnr/log", dtext = "PNR Log", thumbtext = "pl", parentId = 2, roleName = "PNR Reports,Administrator" });
            menu.Add(new menulinks() { linkId = 5, url = "/pnr/tree", dtext = "PNR Tree view", thumbtext = "pt", parentId = 2, roleName = "PNR Reports,Administrator" });

            menu.Add(new menulinks() { linkId = 6, url = "javascript:;", dtext = "Booking", thumbtext = "Bo", parentId = 0, roleName = "ReadBooking,UpdateBooking,DeleteBooking,CreatBooking,Administrator" });
            menu.Add(new menulinks() { linkId = 7, url = "/booking/entry", dtext = "Entry", thumbtext = "en", parentId = 6, roleName = "CreateBooking,Administrator" });
            menu.Add(new menulinks() { linkId = 8, url = "/booking/index", dtext = "Manage", thumbtext = "ma", parentId = 6, roleName = "ReadBooking,UpdateBooking,DeleteBooking,CreatBooking,Administrator" });

            menu.Add(new menulinks() { linkId = 9, url = "javascript:;", dtext = "Group Split", thumbtext = "Gs", parentId = 0, roleName = "ReadGroupSplit,UpdateGroupSplit,DeleteGroupSplit,CreateGroupSplit,Administrator" });
            menu.Add(new menulinks() { linkId = 10, url = "/booking/groupsplit", dtext = "Addition", thumbtext = "ad", parentId = 9, roleName = "CreateGroupSplit,Administrator" });
            menu.Add(new menulinks() { linkId = 11, url = "/booking/groupsplitlist", dtext = "Manage", thumbtext = "ma", parentId = 9, roleName = "ReadGroupSplit,UpdateGroupSplit,DeleteGroupSplit,CreateGroupSplit,Administrator" });

            menu.Add(new menulinks() { linkId = 12, url = "javascript:;", dtext = "Stock Selling", thumbtext = "Ss", parentId = 0, roleName = "ReadStockSell,UpdateStockSell,DeleteStockSell,CreatStockSell,Administrator" });
            menu.Add(new menulinks() { linkId = 13, url = "/stock/sellingcreate", dtext = "Create", thumbtext = "sc", parentId = 12, roleName = "CreateStockSell" });
            menu.Add(new menulinks() { linkId = 14, url = "/stock/selling", dtext = "Manage", thumbtext = "ma", parentId = 12, roleName = "ReadStockSell,UpdateStockSell,DeleteStockSell,CreatStockSell,Administrator" });

            menu.Add(new menulinks() { linkId = 15, url = "javascript:;", dtext = "Stock Transfer", thumbtext = "St", parentId = 0, roleName = "ReadStockTransfer,UpdateStockTransfer,DeleteStockTransfer,CreateStockTransfer,Administrator" });
            menu.Add(new menulinks() { linkId = 16, url = "/stock/transfercreate", dtext = "Transfer", thumbtext = "tr", parentId = 15, roleName = "CreateStockTransfer,Administrator" });
            menu.Add(new menulinks() { linkId = 17, url = "/stock/transferlist", dtext = "Manage", thumbtext = "ma", parentId = 15, roleName = "ReadStockTransfer,UpdateStockTransfer,DeleteStockTransfer,CreateStockTransfer,Administrator" });

            menu.Add(new menulinks() { linkId = 18, url = "/stock/receive", dtext = "Stock Recieve", thumbtext = "Sr", parentId = 0, roleName = "StockReceive,Administrator" });

            menu.Add(new menulinks() { linkId = 19, url = "/stockId/Index", dtext = "Stock Id", thumbtext = "Si", parentId = 0, roleName = "ManageStockId,Administrator" });

            menu.Add(new menulinks() { linkId = 20, url = "/agents/Index", dtext = "Agents", thumbtext = "Ag", parentId = 0, roleName = "ManageAgents,Administrator" });

            menu.Add(new menulinks() { linkId = 21, url = "/airline/Index", dtext = "Airline", thumbtext = "Ar", parentId = 0, roleName = "ManageAirline,Administrator" });

            menu.Add(new menulinks() { linkId = 22, url = "/branches/Index", dtext = "Branches", thumbtext = "Br", parentId = 0, roleName = "Managebranches,Administrator" });

            menu.Add(new menulinks() { linkId = 23, url = "/category/Index", dtext = "Category", thumbtext = "Ca", parentId = 0, roleName = "ManageCategory,Administrator" });

            menu.Add(new menulinks() { linkId = 24, url = "/usermanagement/Index", dtext = "User Management", thumbtext = "Um", parentId = 0, roleName = "ManageUsers,Administrator" });

            menu.Add(new menulinks() { linkId = 25, url = "javascript:;", dtext = "User Roles", thumbtext = "Ur", parentId = 0, roleName = "ManageUserRoles,Administrator" });
            menu.Add(new menulinks() { linkId = 26, url = "/userrole/create", dtext = "Add Roles", thumbtext = "ar", parentId = 25, roleName = "ManageUserRoles,Administrator" });
            menu.Add(new menulinks() { linkId = 27, url = "/userrole/index", dtext = "Manage", thumbtext = "ma", parentId = 25, roleName = "ManageUserRoles,Administrator" });


            

        }
        public List<menulinks> getLinks(List<IdentityUserRole> userroles)
        {



            var menu2 = new List<menulinks>();
            menu2.AddRange(menu.Where(z => z.roleName == "").ToList());
            userroles.ForEach(x => {
                var isexist = menu2.Where(y => y.roleName.Contains(x.Role.Name)).FirstOrDefault();
                if(isexist == null)
                {
                    menu2.AddRange(menu.Where(z => z.roleName.Contains(x.Role.Name)).ToList());
                }

            });

            menu2 = menu2.OrderBy(x => x.linkId).ToList();
            return menu2;
        }
    }
}