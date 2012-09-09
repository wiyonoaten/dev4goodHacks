using GalaSoft.MvvmLight.Ioc;
using ServicesLib;
using ServicesLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace PayPerPlace.Metro
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class StartPage : PayPerPlace.Metro.Common.LayoutAwarePage
    {
        public StartPage()
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

        private void BtnViewRunningChallenges_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(RunningChallengesPage)))
            {
                throw new Exception("Failed to create running challenges page");
            }
        }

        private async void BtnStartNewChallenge_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PayPerPlaceServiceClient serviceClient = SimpleIoc.Default.GetInstance<PayPerPlaceServiceClient>();

            Challenge newChallenge = new Challenge()
                {
                    Name = "Test Challenge",
                    CreatorUserId = (await serviceClient.GetCurrentUserAsync()).Id,
                    ExpiryTime = DateTime.Now.ToUniversalTime().AddDays(7), //TODO:
                    Wager = 5.0M,
                    LocationList = new List<Location>()
                        {
                            new Location
                                {
                                    Latitude = 0.0,
                                    Longitude = 0.0,
                                },
                            new Location
                                {
                                    Latitude = 50.0,
                                    Longitude = 50.0,
                                },
                            new Location
                                {
                                    Latitude = 100.0,
                                    Longitude = 50.0,
                                },
                        },
                };

            bool successful = await serviceClient.CreateNewChallengeAsync(newChallenge);
        }
    }
}
