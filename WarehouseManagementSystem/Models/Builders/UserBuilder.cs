using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Exceptions;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Models.Entities.Enums;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Models.Builders
{
    public class UserBuilder : IBuilder<User>
    {
        private User user;

        public UserBuilder(string username, string password, UserRolesEnum role)
        {
            try
            {
                this.user = Initialize(new User(username, password, role));
            }
            catch
            {
                throw;
            }
        }

        public UserBuilder(User user)
        {
            try
            {
                this.user = Initialize(user);
            }
            catch
            {
                throw;
            }
        }

        private User Initialize(User user)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddUser(user);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return user;
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        errorLogger.LogError(ex);
                    }
                    throw;
                }
            }
        }

        public UserBuilder WithAdditionalInfo(string additionalInfo)
        {
            user.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    user = entityManager.UpdateUser(user);
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        errorLogger.LogError(ex);
                    }
                    throw;
                }
            }

            return this;
        }

        public User Build()
        {
            return user;
        }
    }
}
