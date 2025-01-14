namespace UrlShortener.Core.Entities;

public class Algorithm
{
    public int Id { get; set; }
    
    public required string Title { get; set; }
    
    public required string Description { get; set; }
}