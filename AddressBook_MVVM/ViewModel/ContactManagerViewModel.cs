using AddressBook_MVVM.Commands;
using AddressBook_MVVM.Model;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace AddressBook_MVVM.ViewModel
{
    public class ContactManagerViewModel : DependencyObject
    {
        public DelegateCommand AddCommand { get; }
        public DelegateCommand ModifyCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand LoadCommand { get; }

        public static readonly DependencyProperty PersonsProperty =
            DependencyProperty.Register("Persons", typeof(ObservableCollection<PersonViewModel>), typeof(ContactManagerViewModel), new PropertyMetadata(new ObservableCollection<PersonViewModel>()));

        public ObservableCollection<PersonViewModel> Persons
        {
            get { return (ObservableCollection<PersonViewModel>)GetValue(PersonsProperty); }
            set { SetValue(PersonsProperty, value); }
        }
        public PersonViewModel CurrentPerson { get; set; } = new PersonViewModel();
       
        public ContactManagerViewModel()
        {
            AddCommand = new DelegateCommand(_ => AddPerson(),_ => AddCommand_CanExecute());

            ModifyCommand = new DelegateCommand(_ => ModifyPerson(),_ => ModifyCommand_CanExecute());

            DeleteCommand = new DelegateCommand(_ => DeletePerson(),_ => DeleteCommand_CanExecute());

            SaveCommand = new DelegateCommand(_ => SaveData(),_ => SaveCommand_CanExecute());

            LoadCommand = new DelegateCommand(_ => LoadData(), _ => true);

        }

        public void AddPerson()
        {
            Persons.Add(CurrentPerson.Clone());

            CurrentPerson.Name = string.Empty;
            CurrentPerson.Address = string.Empty;
            CurrentPerson.Phone = string.Empty;
        }
        private bool AddCommand_CanExecute()
        {
            if (!string.IsNullOrWhiteSpace(CurrentPerson.Name)
                           && !string.IsNullOrWhiteSpace(CurrentPerson.Phone)
                           && !string.IsNullOrWhiteSpace(CurrentPerson.Address))
            {
                return true;
            }
            return false;
        }

        private void ModifyPerson()
        {
            if (SelectedPersonViewModel != null)
            {
                SelectedPersonViewModel.Name = CurrentPerson.Name;
                SelectedPersonViewModel.Address = CurrentPerson.Address;
                SelectedPersonViewModel.Phone = CurrentPerson.Phone; 
            }
        }
        private bool ModifyCommand_CanExecute()
        {
            return SelectedPersonViewModel != null;
        }

        private void DeletePerson()
        {
            if (SelectedPersonViewModel != null)
                Persons.Remove(SelectedPersonViewModel);
        }
        private bool DeleteCommand_CanExecute() {  
            return SelectedPersonViewModel != null && Persons.Count > 0;
        }

        private void SaveData()
        {
            var json = JsonConvert.SerializeObject(Persons.ToList());

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*",
                DefaultExt = "json",
                FileName = "contacts.json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, json);
            }
        }
        private bool SaveCommand_CanExecute()
        {
            return Persons.Count > 0;
        }

        private void LoadData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string jsonText = File.ReadAllText(openFileDialog.FileName);
                try
                {
                    ObservableCollection<PersonViewModel> loadedPersons = JsonConvert.DeserializeObject<ObservableCollection<PersonViewModel>>(jsonText);

                    if (loadedPersons != null)
                    {
                        Persons.Clear();

                        foreach (var person in loadedPersons)
                        {
                            Persons.Add(person);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при загрузке контактов.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}");
                }
            }
        }


        public static readonly DependencyProperty SelectedPersonViewModelProperty =
    DependencyProperty.Register("SelectedPersonViewModel", typeof(PersonViewModel), typeof(ContactManagerViewModel), new PropertyMetadata(null, OnSelectedPersonChanged));

        private static void OnSelectedPersonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = (ContactManagerViewModel)d;
            var selectedPerson = e.NewValue as PersonViewModel;

            if (selectedPerson != null)
            {
                viewModel.CurrentPerson.Name = selectedPerson.Name;
                viewModel.CurrentPerson.Address = selectedPerson.Address;
                viewModel.CurrentPerson.Phone = selectedPerson.Phone;
            }
        }

        public PersonViewModel SelectedPersonViewModel
        {
            get { return (PersonViewModel)GetValue(SelectedPersonViewModelProperty); }
            set { SetValue(SelectedPersonViewModelProperty, value); }
        }
    }

}
