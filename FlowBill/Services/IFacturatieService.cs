using FlowBill.Models;

namespace FlowBill.Services
{
    public interface IFacturatieService
    {
        Task<Factuur> GenereerFactuur(int bestellingId);
        Task<byte[]> GenereerPDF(Factuur factuur);
        Task<bool> VerstuurFactuurEmail(Factuur factuur);
    }
}