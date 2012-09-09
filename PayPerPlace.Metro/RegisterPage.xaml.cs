using GalaSoft.MvvmLight.Ioc;
using ServicesLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Security.Authentication.Web;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace PayPerPlace.Metro
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class RegisterPage : PayPerPlace.Metro.Common.LayoutAwarePage
    {
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void BtnRegister_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            PayPerPlaceServiceClient serviceClient = SimpleIoc.Default.GetInstance<PayPerPlaceServiceClient>();
            bool successful = await serviceClient.RegisterUserAsync(TxtBoxUserName.Text);

            if (successful)
            {
                if (!Frame.Navigate(typeof(StartPage)))
                {
                    throw new Exception("Failed to create start page");
                }

                try
                {
                    string clientID = "7shd6ndha9hflmsduus8u";
                    string clientSecret = "8ndf7shd7fs79df83nfmls";
                    string redirectURL = "http://runwithme.com/givey_callback";
                    String giveyURL = "https://api.givey.com/v1/oauth/authorize?response_type=code&lcient_id=" + Uri.EscapeDataString(clientID) + "&redirect_uri=" + Uri.EscapeDataString(redirectURL) + "&client_secret=" + clientSecret + "&grant_type=authorization_code&scope=read_stream&display=popup&response_type=token";

                    System.Uri StartUri = new Uri(giveyURL);
                    System.Uri EndUri = new Uri(redirectURL);

                    WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                            WebAuthenticationOptions.None,
                                                            StartUri,
                                                            EndUri);
                    if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                    {
                        MessageDialog dlg = new MessageDialog(WebAuthenticationResult.ResponseData.ToString());
                    }
                    else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                    {
                        MessageDialog dlg = new MessageDialog("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                    }
                    else
                    {
                        MessageDialog dlg = new MessageDialog("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                    }
                }
                catch (Exception Error)
                {
                    //
                    // Bad Parameter, SSL/TLS Errors and Network Unavailable errors are to be handled here.
                    //
                    MessageDialog dlg = new MessageDialog("Error, try again. " + Error.ToString());
                }
            }
            else
            {
                MessageDialog dlg = new MessageDialog("Error registering user, try again.");
                dlg.Commands.Add(new UICommand("OK"));
                dlg.ShowAsync();
            }
        }
    }
}
