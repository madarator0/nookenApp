using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using nookenApp.Models;

namespace nookenApp.ViewsModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly TQFHRepository _tqfhRepository;
        private readonly TopertimeRepository _topertimeRepository;
        private readonly TbalansRepository _tbalansRepository;

        public MainViewModel(TQFHRepository tqfhRepository, TopertimeRepository topertimeRepository, TbalansRepository tbalansRepository)
        {
            _tqfhRepository = tqfhRepository;
            _topertimeRepository = topertimeRepository;
            _tbalansRepository = tbalansRepository;
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public async Task SaveTQFHData(float urov, float rashod, short kodObj)
        {
            var success = await _tqfhRepository.InsertAsync(urov, rashod, kodObj);
            StatusMessage = success ? "TQFH data saved successfully." : "Failed to save TQFH data.";
        }

        public async Task SaveTopertimeData(int kodobj, int kodpokaz, DateTime datatime, double ti, int tc)
        {
            var success = await _topertimeRepository.InsertAsync(kodobj, kodpokaz, datatime, ti, tc);
            StatusMessage = success ? "Topertime data saved successfully." : "Failed to save Topertime data.";
        }

        public async Task SaveTbalansData(byte kodvodvod, byte typevodvod, DateTime datatime, double ti)
        {
            var success = await _tbalansRepository.InsertAsync(kodvodvod, typevodvod, datatime, ti);
            StatusMessage = success ? "Tbalans data saved successfully." : "Failed to save Tbalans data.";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
