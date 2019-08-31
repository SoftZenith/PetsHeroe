﻿using System;
using System.Threading.Tasks;
using PetsHeroe.Services;
using Xamarin.Forms;
using ZXing.Mobile;

[assembly: Dependency(typeof(PetsHeroe.iOS.Services.QRScanningService))]
namespace PetsHeroe.iOS.Services
{
    public class QRScanningService : IQRScanning
    {
        public QRScanningService()
        {
        }

        public async Task<string> ScanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionsCustom = new MobileBarcodeScanningOptions();

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Escanea el código QR",
                BottomText = "Por favor espere",
            };
            ZXing.Result scanResult;
            try{
                scanResult = await scanner.Scan(optionsCustom);
            }catch (Exception) {
                return "";
            }
            return scanResult.Text;
        }
    }
}
