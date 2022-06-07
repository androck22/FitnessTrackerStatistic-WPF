using LiveCharts;
using LiveCharts.Configurations;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPFEntity.Models;

namespace WPFEntity.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly List<User> users;

        public ObservableCollection<UserMonthStatistics> UsersMonthStatistics { get; }

        public UserMonthStatistics SelectedUsersMonthStatistics { get; set; }

        public ICommand ProcessUsersMonthStatisticsSelectionChangedCommand { get; }
        public ICommand LoadUsersCommand { get; }

        public CartesianMapper<DataModel> RowSeriesConfiguration { get; set; }
        public ChartValues<DataModel> RowSeries { get; set; }
        public ObservableCollection<int> RowSeriesLabels { get; set; }
        public LiveCharts.Wpf.Separator DaysSeparator { get; set; }

        public MainViewModel()
        {
            users = new List<User>();
            UsersMonthStatistics = new ObservableCollection<UserMonthStatistics>();

            ProcessUsersMonthStatisticsSelectionChangedCommand = new RelayCommand(s => ProcessUsersMonthStatisticsSelectionChanged());
            LoadUsersCommand = new RelayCommand(s => LoadUsers());

            RowSeries = new ChartValues<DataModel>();
            RowSeriesLabels = new ObservableCollection<int>();
            RowSeriesConfiguration = new CartesianMapper<DataModel>().Y(dataModel => dataModel.Value);
            DaysSeparator = new LiveCharts.Wpf.Separator
            {
                Step = 1
            };
        }

        private void ProcessUsersMonthStatisticsSelectionChanged()
        {
            if (SelectedUsersMonthStatistics != null)
            {
                RowSeries.Clear();
                RowSeries.AddRange(
                    users.Where(u => u.Name == SelectedUsersMonthStatistics.Name)
                         .OrderBy(u => u.Day)
                         .Select(u => new DataModel(u.Steps, u.Day)));

                RowSeriesLabels.Clear();
                RowSeriesLabels.AddRange(RowSeries.Select(r => r.Label));
            }
            else
            {
                //TODO: smth wrong
            }
        }

        private void LoadUsers()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Json Documents(*.json)|*.json", ValidateNames = true, Multiselect = true };

            Nullable<bool> dialogOK = openFileDialog.ShowDialog();
            if (dialogOK == true)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    string strResult = null;
                    using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        StreamReader sr = new StreamReader(stream);
                        strResult = sr.ReadToEnd();
                        sr.Close();
                    }

                    int day = 0;
                    string path = file.Substring(file.Length - 7, 1);

                    if (path == "y")
                    {
                        day = int.Parse(file.Substring(file.Length - 6, 1));
                    }
                    else 
                    {
                        day = int.Parse(file.Substring(file.Length - 7, 2));
                    }

                    var data = JsonConvert.DeserializeObject<List<JsonModel>>(strResult);

                    UploadFile(data, day);
                }

                ProcessUsersMonthStatisticsSelectionChanged();
            }
        }
        private void UploadFile(List<JsonModel> data, int day)
        {
            users.AddRange(data.Select(d => new User { Rank = d.Rank, Day = day, Name = d.User, Steps = d.Steps, Status = d.Status }));

            UsersMonthStatistics.Clear();
            UsersMonthStatistics.AddRange(
                users.GroupBy(user => user.Name)
                     .Select(group => new UserMonthStatistics(
                                group.First().Name,
                                Math.Round(group.Average(user => user.Steps)),
                                group.Max(user => user.Steps),
                                group.Min(user => user.Steps))));

            SelectedUsersMonthStatistics = UsersMonthStatistics.FirstOrDefault();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
