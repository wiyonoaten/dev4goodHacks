using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bing.Maps;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using ServicesLib;
using GalaSoft.MvvmLight.Ioc;
using ServicesLib.Models;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace PayPerPlace.Metro
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class CreateChallenge : PayPerPlace.Metro.Common.LayoutAwarePage
    {
        const double maxZoom = 19;
        const double minZoom = 5;

        private CustomPin pin;
        public CreateChallenge()
        {
            this.InitializeComponent();
            pin = new CustomPin();
            map.Children.Add(pin);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }


        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            var zoom = map.ZoomLevel - 2;
            map.SetZoomLevel(zoom < minZoom ? minZoom : zoom);
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            var zoom = map.ZoomLevel + 2;
            map.SetZoomLevel(zoom > maxZoom ? maxZoom : zoom);
        }

        private void btnChangeMapType_Click(object sender, RoutedEventArgs e)
        {
            switch (map.MapType)
            {
                case Bing.Maps.MapType.Aerial:
                    map.MapType = Bing.Maps.MapType.Birdseye;
                    break;
                case Bing.Maps.MapType.Birdseye:
                    map.MapType = Bing.Maps.MapType.Road;
                    break;
                default:
                    map.MapType = Bing.Maps.MapType.Aerial;
                    break;
            }
        }

        private async void btnSetLocation_Click(object sender, RoutedEventArgs e)
        {
            Geolocator geolocator = new Geolocator();
            var pos = await geolocator.GetGeopositionAsync(TimeSpan.FromDays(10), TimeSpan.FromHours(1));
            Bing.Maps.Location location = new Bing.Maps.Location(pos.Coordinate.Latitude, pos.Coordinate.Longitude);

            //Center map view on current location.
            MapLayer.SetPosition(pushPin, location);
            map.SetView(location, 15.0f);
        }

        private void map_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var pos = e.GetPosition(map);
            Bing.Maps.Location location;
            map.TryPixelToLocation(pos, out location);

            MapLayer.SetPosition(pin, location);
            map.SetView(location);
        }

        private void btn_CreateNewChallenge(object sender, RoutedEventArgs e)
        {
            PayPerPlaceServiceClient a = SimpleIoc.Default.GetInstance<PayPerPlaceServiceClient>();
            Challenge c = new Challenge(); 
	    try
	    {
	    	if (txtTitle.Text != "")
	    	{
            		c.Name = txtTitle.Text;
            	}
	    	if (txtWager.Text != "")
	    	{
            		c.Wager = Convert.ToInt32(txtWager.Text);
            	}
                a.CreateNewChallengeAsync(c);

                MessageDialog dlg = new MessageDialog("Challenge Created.");
                dlg.Commands.Add(new UICommand("OK"));
                dlg.ShowAsync();
	   }
	   catch (Exception ex)
	   {
	        MessageDialog dlg = new MessageDialog("Error - " + ex.Message);
                dlg.Commands.Add(new UICommand("OK"));
                dlg.ShowAsync();
	   }	 
        }
    }
}
