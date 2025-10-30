namespace FrontPatient.AspNetCore.Models.Dtos;

public class TokenDto
{
    public string? Token { get; set; }
    public DateTime? Expiration { get; set; }
}