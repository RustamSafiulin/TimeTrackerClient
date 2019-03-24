
using System.Collections.ObjectModel;
using System.Collections.Generic;

using TimeTracker.EventBus;
using TimeTracker.Helpers;
using TimeTracker.Service;

namespace TimeTracker.ViewModels
{
    public class MyData
    {
        public string Name { get; set; }
        public double Count { get; set; }
    }

    public class NonAimedUsage
    {
        public string Site { get; set; }
        public double Hours { get; set; }
    }

    public class DataSet
    {
        public ObservableCollection<MyData> Data { get; set; }
        public ObservableCollection<NonAimedUsage> Sites { get; set; }

        public DataSet()
        { }
    }

    public sealed class ReportsViewModel : DomainModelBase
    {
        public ReportsViewModel(ApplicationEnvironment appApplicationEnvironment)
        {
            _appApplicationEnvironment = appApplicationEnvironment;

            Days = new List<int>() { 1, 3, 7, 10, 30, 90 };
            ReportPeriodChangedCommand = new RelayCommand(ReportPeriodChangedHandler);

            var dataSet1 = new DataSet()
            {
                Data = new ObservableCollection<MyData>()
                {
                    new MyData {Name="Учеба", Count = 3.5 },
                    new MyData {Name="Спорт", Count = 1.5 },
                    new MyData {Name="Худ. школа", Count = 2 },
                    new MyData {Name="Интернет", Count = 1 }
                },
                Sites = new ObservableCollection<NonAimedUsage>()
                {
                    new NonAimedUsage {Site = "pikabu.ru", Hours = 0.3 },
                    new NonAimedUsage {Site = "vk.com", Hours = 0.7 }
                }
            };

            var dataSet2 = new DataSet()
            {
                Data = new ObservableCollection<MyData>()
                {
                    new MyData {Name="Учеба", Count = 11 },
                    new MyData {Name="Спорт", Count = 4.5 },
                    new MyData {Name="Худ. школа", Count = 6 },
                    new MyData {Name="Интернет", Count = 3.2 }
                },
                Sites = new ObservableCollection<NonAimedUsage>()
                {
                    new NonAimedUsage {Site = "pikabu.ru", Hours = 1 },
                    new NonAimedUsage {Site = "vk.com", Hours = 2.2 }
                }
            };

            var dataSet3 = new DataSet()
            {
                Data = new ObservableCollection<MyData>()
                {
                    new MyData {Name="Учеба", Count = 40 },
                    new MyData {Name="Спорт", Count = 11 },
                    new MyData {Name="Худ. школа", Count = 14 },
                    new MyData {Name="Интернет", Count = 7 }
                },
                Sites = new ObservableCollection<NonAimedUsage>()
                {
                    new NonAimedUsage {Site = "pikabu.ru", Hours = 2 },
                    new NonAimedUsage {Site = "vk.com", Hours = 5 }
                }
            };

            var dataSet4 = new DataSet()
            {
                Data = new ObservableCollection<MyData>()
                {
                    new MyData {Name="Учеба", Count = 65 },
                    new MyData {Name="Спорт", Count = 15 },
                    new MyData {Name="Худ. школа", Count = 19 },
                    new MyData {Name="Интернет", Count = 8 }
                },
                Sites = new ObservableCollection<NonAimedUsage>()
                {
                    new NonAimedUsage {Site = "pikabu.ru", Hours = 2.2 },
                    new NonAimedUsage {Site = "vk.com", Hours = 5.8 }
                }
            };

            var dataSet5 = new DataSet()
            {
                Data = new ObservableCollection<MyData>()
                {
                    new MyData {Name="Учеба", Count = 181 },
                    new MyData {Name="Спорт", Count = 30 },
                    new MyData {Name="Худ. школа", Count = 41 },
                    new MyData {Name="Интернет", Count = 20 }
                },
                Sites = new ObservableCollection<NonAimedUsage>()
                {
                    new NonAimedUsage {Site = "pikabu.ru", Hours = 4.5 },
                    new NonAimedUsage {Site = "vk.com", Hours = 15.5 }
                }
            };

            var dataSet6 = new DataSet()
            {
                Data = new ObservableCollection<MyData>()
                {
                    new MyData {Name="Учеба", Count = 400 },
                    new MyData {Name="Спорт", Count = 55 },
                    new MyData {Name="Худ. школа", Count = 100 },
                    new MyData {Name="Интернет", Count = 32 }
                },
                Sites = new ObservableCollection<NonAimedUsage>()
                {
                    new NonAimedUsage {Site = "pikabu.ru", Hours = 5.2 },
                    new NonAimedUsage {Site = "vk.com", Hours = 26.8 }
                }
            };

            Data = dataSet1.Data;
            Sites = dataSet1.Sites;

            TestDataSets = new Dictionary<int, DataSet>()
            {
                { 1, dataSet1 },
                { 3, dataSet2 },
                { 7, dataSet3 },
                { 10, dataSet4 },
                { 30, dataSet5 },
                { 90, dataSet6 }
            };
        }

        #region Props and Fields

        public RelayCommand ReportPeriodChangedCommand { get; set; }

        private ObservableCollection<NonAimedUsage> _sites = new ObservableCollection<NonAimedUsage>();
        public ObservableCollection<NonAimedUsage> Sites
        {
            get { return _sites; }
            set { SetProperty(ref _sites, value); }
        }

        private ObservableCollection<MyData> _data = new ObservableCollection<MyData>();
        public ObservableCollection<MyData> Data
        {
            get { return _data; }
            set { SetProperty(ref _data, value); }
        }

        public List<int> Days { get; set; }

        public Dictionary<int, DataSet> TestDataSets;

        private int _selectedPeriod = 1;
        public int SelectedPeriod
        {
            get { return _selectedPeriod; }
            set { SetProperty(ref _selectedPeriod, value); }
        }

        private ApplicationEnvironment _appApplicationEnvironment;

        #endregion

        private void ReportPeriodChangedHandler()
        {
            if (TestDataSets.ContainsKey(SelectedPeriod))
            {
                Sites = TestDataSets[SelectedPeriod].Sites;
                Data = TestDataSets[SelectedPeriod].Data;
            }
        }
    }
}
