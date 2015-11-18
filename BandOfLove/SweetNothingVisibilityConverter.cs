using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;

namespace BandOfLove
{
    public class SweetNothingVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((string)parameter == "block")
            {
                return ViewModel.instance.IsEditMode ? Visibility.Collapsed : Visibility.Visible;
            }
            else if ((string)parameter == "box")
            {
                return ViewModel.instance.IsEditMode ? Visibility.Visible : Visibility.Collapsed;
            }
            else
                throw new NotImplementedException();
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}