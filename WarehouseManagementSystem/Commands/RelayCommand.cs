using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WarehouseManagementSystem.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object>? execute;
        private readonly Func<object, bool>? canExecute;
        private ICommand? deleteProductDetailCommand;

        public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public RelayCommand(ICommand deleteProductDetailCommand)
        {
            this.deleteProductDetailCommand = deleteProductDetailCommand;
        }

        public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter ?? throw new ArgumentNullException(nameof(parameter))) ?? true;

        public void Execute(object? parameter) => execute?.Invoke(parameter ?? throw new ArgumentNullException(nameof(parameter)));

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
