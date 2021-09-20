using CutMp3.Domain.Enums;
using System;
using System.Timers;

namespace CutMp3.Application.Services
{
    public class ToastService : IDisposable
    {
        private Timer _countdown = new Timer(10000);

        public event Action<string, ToastLevel>? OnShow;
        public event Action? OnHide;
        
        public void ShowToast(string message, ToastLevel level)
        {
            OnShow?.Invoke(message, level);
            StartCountdown();
        }

        private void StartCountdown()
        {
            SetCountdown();
            if (_countdown.Enabled)
            {
                _countdown.Stop();
                _countdown.Start();
            }
            else
            {
                _countdown.Start();
            }
        }

        private void SetCountdown()
        {
            if (_countdown == null)
            {
                _countdown = new Timer(10000);
                _countdown.Elapsed += HideToast;
                _countdown.AutoReset = false;
            }
        }

        private void HideToast(object source, ElapsedEventArgs args)
        {
            OnHide?.Invoke();
        }

        public void Dispose()
        {
            _countdown?.Dispose();
        }
    }
}
