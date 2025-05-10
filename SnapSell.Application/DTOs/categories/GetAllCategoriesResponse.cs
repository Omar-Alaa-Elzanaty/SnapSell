namespace SnapSell.Application.DTOs.categories;

public sealed record GetAllCategoriesResponse(
    Guid CategoryId,
    string Name,
    Guid? ParentCategoryId);