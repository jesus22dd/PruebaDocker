namespace AppCompleta.ViewModels.DTOs
{
    public class VentaRequestDTO
    {
        public int? IdCliente { get; set; }
        public decimal Total { get; set; }
        public List<DetalleVentaDTO> Detalles { get; set; } = new List<DetalleVentaDTO>();
    }
}
