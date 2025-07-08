using Microsoft.AspNetCore.Authorization;

namespace SnapSell.Presentation.EndPoints;

[Authorize(Roles = "Customer")]
public sealed class CustomerController : ApiControllerBase;