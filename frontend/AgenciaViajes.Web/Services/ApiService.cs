using System.Net.Http.Json;
using AgenciaViajes.Web.Models;

namespace AgenciaViajes.Web.Services;

public class ApiService(HttpClient http)
{
    public Task<List<DestinoDto>?> GetDestinosAsync() => http.GetFromJsonAsync<List<DestinoDto>>("api/destinos");
    public Task<List<PaqueteDto>?> GetPaquetesAsync() => http.GetFromJsonAsync<List<PaqueteDto>>("api/paquetes");
    public Task<List<ClienteDto>?> GetClientesAsync() => http.GetFromJsonAsync<List<ClienteDto>>("api/clientes");
    public Task<List<ReservaDto>?> GetReservasAsync() => http.GetFromJsonAsync<List<ReservaDto>>("api/reservas");

    public Task<HttpResponseMessage> CreateDestinoAsync(DestinoDto x) => http.PostAsJsonAsync("api/destinos", x);
    public Task<HttpResponseMessage> UpdateDestinoAsync(DestinoDto x) => http.PutAsJsonAsync($"api/destinos/{x.DestinoId}", x);
    public Task<HttpResponseMessage> DeleteDestinoAsync(int id) => http.DeleteAsync($"api/destinos/{id}");

    public Task<HttpResponseMessage> CreatePaqueteAsync(PaqueteDto x) => http.PostAsJsonAsync("api/paquetes", x);
    public Task<HttpResponseMessage> UpdatePaqueteAsync(PaqueteDto x) => http.PutAsJsonAsync($"api/paquetes/{x.PaqueteId}", x);
    public Task<HttpResponseMessage> DeletePaqueteAsync(int id) => http.DeleteAsync($"api/paquetes/{id}");

    public Task<HttpResponseMessage> CreateClienteAsync(ClienteDto x) => http.PostAsJsonAsync("api/clientes", x);
    public Task<HttpResponseMessage> UpdateClienteAsync(ClienteDto x) => http.PutAsJsonAsync($"api/clientes/{x.ClienteId}", x);
    public Task<HttpResponseMessage> DeleteClienteAsync(int id) => http.DeleteAsync($"api/clientes/{id}");

    public Task<HttpResponseMessage> CreateReservaAsync(ReservaDto x) => http.PostAsJsonAsync("api/reservas", x);
    public Task<HttpResponseMessage> UpdateReservaAsync(ReservaDto x) => http.PutAsJsonAsync($"api/reservas/{x.ReservaId}", x);
    public Task<HttpResponseMessage> DeleteReservaAsync(int id) => http.DeleteAsync($"api/reservas/{id}");

    public static async Task<string> ErrorAsync(HttpResponseMessage response, string fallback)
    {
        var text = await response.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(text) ? fallback : text.Trim('"');
    }
}
