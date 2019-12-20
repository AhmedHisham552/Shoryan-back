using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoryan.StoredProcedures
{
    public class UsersProcedures
    {
        public static string addUser = "addUser";
        public static string getAllUsers = "getAllUsers";
        public static string deleteUser = "deleteUser";
        public static string editUserDetails = "editUserDetails";
        public static string authenticateUser = "authenticateUser";
        public static string getUserDetails = "getUserDetails";
        public static string addPhoneNumber = "addPhoneNumber";
        public static string getPhoneNumber = "getPhoneNumber";
        public static string getActiveListings = "getAllActiveListingsByUserId";
    }
}
