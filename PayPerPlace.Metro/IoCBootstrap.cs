using GalaSoft.MvvmLight.Ioc;
using ServicesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPerPlace.Metro
{
    public class IoCBootstrap
    {
        public static void Configure(App app)
        {
            SimpleIoc.Default.Register<PayPerPlaceServiceClient>(() => new PayPerPlaceServiceClient());
        }
    }
}
