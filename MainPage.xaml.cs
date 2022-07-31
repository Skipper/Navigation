using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Storage.Streams;

using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI;
using System.Globalization;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace Navegador
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        // https://docs.microsoft.com/es-es/windows/uwp/maps-and-location/routes-and-directions
        // https://geeks.ms/santypr/2014/02/22/la-odisea-de-cmo-pasar-de-string-a-decimal-o-double/

        public MainPage()
        {
            this.InitializeComponent();
        }

        // Entrega
        private async void btn_trazar_ruta (object sender, RoutedEventArgs e)
        {

            String latitudeOrigen = tb_latitude_origen.Text;
            String longitudeOrigen = tb_longitude_origen.Text;

            String latitudeDestino = tb_latitude_destino.Text;
            String longitudeDestino = tb_longitude_destino.Text;


            CultureInfo culture = new CultureInfo("en-US");

            if ( (!string.IsNullOrEmpty(latitudeOrigen)) && (!string.IsNullOrEmpty(longitudeOrigen))
                && (!string.IsNullOrEmpty(latitudeDestino)) && (!string.IsNullOrEmpty(longitudeDestino))
                ) {

                btn_trazar_ruta_title.Content = "Trazar ruta";

                double latitudeOrigenDouble     = double.Parse(latitudeOrigen, culture);
                double longitudeOrigenDouble    = double.Parse(longitudeOrigen, culture);
                
                double latitudeDestinoDouble    = double.Parse(latitudeDestino, culture);
                double longitudeDestinoDouble   = double.Parse(longitudeDestino, culture);

                BasicGeoposition basicGeopositionOrigen = new BasicGeoposition()
                {
                    Latitude    = latitudeOrigenDouble,
                    Longitude   = longitudeOrigenDouble
                };

                BasicGeoposition basicGeopositionDestino = new BasicGeoposition()
                {
                    Latitude    = latitudeDestinoDouble,
                    Longitude   = longitudeDestinoDouble
                };

                // Get the route between the points.
                MapRouteFinderResult mapRouteFinderResult;
                
                mapRouteFinderResult = await MapRouteFinder.GetDrivingRouteAsync(
                      new Geopoint(basicGeopositionOrigen),
                      new Geopoint(basicGeopositionDestino),
                      MapRouteOptimization.Time,
                      MapRouteRestrictions.None);

                    
                if (mapRouteFinderResult.Status == MapRouteFinderStatus.Success)
                {

                    // Use the route to initialize a MapRouteView.
                    MapRouteView mapRouteView;
                    
                    mapRouteView  = new MapRouteView(mapRouteFinderResult.Route);
                    mapRouteView.RouteColor = Colors.Pink;
                    mapRouteView.OutlineColor = Colors.DeepPink;
                    
                    // Add the new MapRouteView to the Routes collection
                    // of the MapControl.
                    mapaRUTA.Routes.Clear();
                    mapaRUTA.Routes.Add(mapRouteView);

                    // Fit the MapControl to the route.
                    await mapaRUTA.TrySetViewBoundsAsync(mapRouteFinderResult.Route.BoundingBox,
                                                         null,
                                                         Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);

                } // End If

            } else {
                btn_trazar_ruta_title.Content = "Valor nulo => 0";
            }

        } // End Button trazar ruta

    } // End Clase Main Page

}