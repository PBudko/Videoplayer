using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeWork_C_sharp_45_wpf.Model
{
   public class CommandClass : ICommand
    {

        Action<object> execute;
        Func<object, bool> canExecute;

        public CommandClass(Action<object> execute) : this(execute,null)
        {
            this.execute = execute;
        }

        public CommandClass(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if(canExecute!=null)
            {
                return canExecute(parameter);
            }
            throw new Exception("from canExecute metod in CommandClass");
        }

        public void Execute(object parameter)
        {
            
                execute(parameter);
            
        }
    }
}
