using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.Services
{
    public class LoginService
    {
        private User? currentUser;

        public User? CurrentUser
        {
            get { return currentUser; }
            private set
            {
                if (currentUser != value)
                {
                    currentUser = value;
                }
            }
        }

        private Warehouse? currentWarehouse;

        public Warehouse CurrentWarehouse
        {
            get { return currentWarehouse ?? throw new NullReferenceException(); }
            set
            {
                if (currentWarehouse != value)
                {
                    currentWarehouse = value;
                }
            }
        }

        public bool Login(string username, string password, Warehouse? warehouse)
        {
            if (IsValidUser(username, password, out User? curUser) && IsValidWarehouse(warehouse))
            {
                CurrentUser = curUser;
                if(warehouse != null) 
                {
                    CurrentWarehouse = warehouse;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Logout()
        {
            currentUser = null;
        }

        public bool IsUserLoggedIn()
        {
            return currentUser != null;
        }

        private bool IsValidUser(string username, string password, out User? user)
        {
            try
            {
                using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    user = db.FindUserByUsernameAndPassword(username, password);
                }
            }
            catch (Exception ex) 
            {
                using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                {
                    errorLogger.LogError(ex);
                }
                MessageBox.Show("Something went wrong. Try again.");
                user = null;
            }
            return user != null;
        }

        private bool IsValidWarehouse(Warehouse? warehouse)
        {
            return !(warehouse == null);
        }
    }
}
