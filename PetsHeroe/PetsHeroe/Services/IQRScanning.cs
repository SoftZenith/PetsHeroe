using System;
using System.Threading.Tasks;

namespace PetsHeroe.Services
{
    public interface IQRScanning
    {
        Task<string> ScanAsync();
    }
}
