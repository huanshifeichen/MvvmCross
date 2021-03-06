// MvxNativeValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using Cirrious.CrossCore.Converters;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Cirrious.CrossCore.WindowsStore.Converters
{
    public class MvxNativeValueConverter
        : IValueConverter
    {
        private readonly IMvxValueConverter _wrapped;

        public MvxNativeValueConverter(IMvxValueConverter wrapped)
        {
            _wrapped = wrapped;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // note - Language ignored here!
            var toReturn = _wrapped.Convert(value, targetType, parameter, CultureInfo.CurrentUICulture);
            return MapIfSpecialValue(toReturn);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // note - Language ignored here!
            var toReturn = _wrapped.ConvertBack(value, targetType, parameter, CultureInfo.CurrentUICulture);
            return MapIfSpecialValue(toReturn);
        }

        private static object MapIfSpecialValue(object toReturn)
        {
            if (toReturn == MvxBindingConstant.DoNothing)
            {
                Mvx.Trace("DoNothing does not have an equivalent in WinRT - returning UnsetValue instead");
                return DependencyProperty.UnsetValue;
            }

            if (toReturn == MvxBindingConstant.UnsetValue)
            {
                return DependencyProperty.UnsetValue;
            }

            return toReturn;
        }
    }

    public class MvxNativeValueConverter<T>
        : MvxNativeValueConverter
        where T : IMvxValueConverter, new()
    {
        public MvxNativeValueConverter()
            : base(new T())
        {
        }
    }
}