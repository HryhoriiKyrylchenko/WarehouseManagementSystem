using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Exceptions;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Models.Builders
{
    public class LabelBuilder : IBuilder<Label>
    {
        private Label label;

        public LabelBuilder(string barcode, int productId)
        {
            try
            {
                this.label = Initialize(new Label(barcode, productId));
            }
            catch
            {
                throw;
            }
        }

        public LabelBuilder(Label label)
        {
            try
            {
                this.label = Initialize(label);
            }
            catch
            {
                throw;
            }
        }

        private Label Initialize(Label label)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddLabel(label);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return label;
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

        public Label Build()
        {
            return label;
        }
    }
}
