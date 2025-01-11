namespace Cashify.Application.DTOs.Filters;

public class GetFilterRequestDto
{
    public string Search { get; set; }
    
    public string OrderBy { get; set; }
    
    public bool IsDescending { get; set; }
}