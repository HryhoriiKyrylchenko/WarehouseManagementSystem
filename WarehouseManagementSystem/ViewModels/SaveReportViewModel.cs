using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.Models.Builders;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.ViewModels
{
    public class SaveReportViewModel : ViewModelBaseRequestClose
    {
        private ReportTypeEnum reportType;

        private string content;

        private int userId;

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    this.title = value;
                    OnPropertyChanged(nameof(Title));
                };
            }
        }

        public ICommand SaveCommand => new RelayCommand(SaveReport);
        public ICommand CanselCommand => new RelayCommand(Cancel);

        public SaveReportViewModel(string title, ReportTypeEnum reportType, string content, int userId)
        {
            this.title = title;
            this.reportType = reportType;
            this.content = content;
            this.userId = userId;
        }

        private void SaveReport(object obj)
        {
            if (!string.IsNullOrWhiteSpace(this.title)) 
            {
                try
                {
                    new ReportBuilder(Title, DateTime.Now, reportType, content, userId);

                    CloseParentWindow();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Some error occured", 
                                    "Error", 
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Error);

                    using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                    {
                        logger.LogError(ex);
                    }

                }
            }
            else
            {
                MessageBox.Show("Title is required",
                                "Caution",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
            }
        }

        private void Cancel(object obj)
        {
            CloseParentWindow();
        }
    }
}
