using System.ComponentModel.DataAnnotations;

namespace SnapSell.Domain.Models;

public class Color : BaseEntity
{
    public required string Name { get; set; }
    public string? HexCode { get; set; }
}