using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Storage;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;
using System.Collections.Generic;
using Run.Models; 
/*using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Firebase.Auth;*/
using System.Threading.Tasks;

namespace Run.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {

        private string _mobileNumber;
        private string _otp;
        private bool _isOtpEnabled;

        public string MobileNumber
        {
            get => _mobileNumber;
            set { _mobileNumber = value; OnPropertyChanged(); }
        }

        public string Otp
        {
            get => _otp;
            set { _otp = value; OnPropertyChanged(); }
        }

        public bool IsOtpEnabled
        {
            get => _isOtpEnabled;
            set { _isOtpEnabled = value; OnPropertyChanged(); }
        }

        public ICommand SendOtpCommand { get; }
        public ICommand SubmitOtpCommand { get; }

        public LoginViewModel()
        {
            SendOtpCommand = new Command(OnSendOtp);
            SubmitOtpCommand = new Command(OnSubmitOtp);
            IsOtpEnabled = false;
        }

        private void OnSendOtp()
        {
            // Simulate OTP send logic here
            if (!string.IsNullOrWhiteSpace(MobileNumber) && MobileNumber.Length == 10)
            {
                // You can integrate actual OTP service logic here
                IsOtpEnabled = true;
            }
        }

        private void OnSubmitOtp()
        {
            // Validate OTP logic here
            // For example: if (Otp == "123456") { ... }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
