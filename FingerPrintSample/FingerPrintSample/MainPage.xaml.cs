using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Fingerprint.Abstractions;
using Xamarin.Forms;

namespace FingerPrintSample
{
    public partial class MainPage : ContentPage
    {
        private CancellationTokenSource _cancel;
        private bool _initialized;

        public MainPage()
        {
            InitializeComponent();
        }
        private async void OnAuthenticate(object sender, EventArgs e)
        {
            await AuthenticationAsync("Put your finger!");

        }
        private async Task AuthenticationAsync(string reason, string cancel = null, string fallback = null, string tooFast = null)    
        {
            _cancel = swAutoCancel.IsToggled ? new CancellationTokenSource(TimeSpan.FromSeconds(10)) : new CancellationTokenSource();
            lblStatus.Text = "";

            var dialogConfig = new AuthenticationRequestConfiguration(reason)
            { // all optional
                CancelTitle = cancel,
                FallbackTitle = fallback,
                AllowAlternativeAuthentication = swAllowAlternative.IsToggled
            };

            // optional

            var result = await Plugin.Fingerprint.CrossFingerprint.Current.AuthenticateAsync(dialogConfig, _cancel.Token);

            await SetResultAsync(result);
        }

        private async Task SetResultAsync(FingerprintAuthenticationResult result)
        {
            if (result.Authenticated)
            {
                await DisplayAlert("FingerPrint Sample", "Success", "Ok");
            }
            else
            {
                await DisplayAlert("FingerPrint Sample", result.ErrorMessage, "Ok");
            }
        }
    }
}