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
    public class ReportBuilder : IBuilder<Report>
    {
        private Report report;

        public ReportBuilder(string title, DateTime reportDate, ReportTypeEnum reportType, string content, int userId)
        {
            try
            {
                this.report = InitializeAsync(new Report(title, reportDate, reportType, content, userId)).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        public ReportBuilder(Report report)
        {
            try
            {
                this.report = InitializeAsync(report).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        private Report Initialize(Report report)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddReport(report);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return report;
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

        private async Task<Report> InitializeAsync(Report report)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddReportAsync(report);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return report;
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        await errorLogger.LogErrorAsync(ex);
                    }
                    throw;
                }
            }
        }

        public Report Build()
        {
            return report;
        }
    }
}
