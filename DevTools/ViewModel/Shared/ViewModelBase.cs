using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DevTools.ViewModel.Shared
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //gonna do some linq magics, finally applying something funny from college
        protected void OnPropertyChanged<T>(Expression<Func<T>> extraction)
        {
            //this will blow up if it's static or you put something other than a property in it
            MemberExpression member = extraction.Body as MemberExpression;
            OnPropertyChanged(member.Member.Name);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged !=null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
