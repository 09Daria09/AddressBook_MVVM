using AddressBook_MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AddressBook_MVVM.ViewModel
{
    public class PersonViewModel : DependencyObject
    {
        private Person _person;

        public PersonViewModel(Person person)
        {
            _person = person;

            Name = _person.Name;
            Address = _person.Address;
            Phone = _person.Phone;
        }
        public PersonViewModel()
        {
            _person = new Person();
        }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(PersonViewModel),
            new PropertyMetadata(string.Empty, OnNameChanged));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value);
            }
        }

        private static void OnNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = (PersonViewModel)d;
            viewModel._person.Name = (string)e.NewValue;
        }

        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(string), typeof(PersonViewModel),
            new PropertyMetadata(string.Empty, OnAddressChanged));

        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value);
            }
        }

        private static void OnAddressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = (PersonViewModel)d;
            viewModel._person.Address = (string)e.NewValue;
        }

        public static readonly DependencyProperty PhoneProperty =
            DependencyProperty.Register("Phone", typeof(string), typeof(PersonViewModel),
            new PropertyMetadata(string.Empty, OnPhoneChanged));

        public string Phone
        {
            get { return (string)GetValue(PhoneProperty); }
            set { SetValue(PhoneProperty, value);
            }
        }

        private static void OnPhoneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = (PersonViewModel)d;
            viewModel._person.Phone = (string)e.NewValue;
        }
        public PersonViewModel Clone()
        {
            return new PersonViewModel
            {
                Name = this.Name,
                Address = this.Address,
                Phone = this.Phone
            };
        }
    }

}
