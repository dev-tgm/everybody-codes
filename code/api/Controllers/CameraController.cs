using Microsoft.AspNetCore.Mvc;
using data;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CameraController : ControllerBase
{
    private readonly IDataProvider _dataProvider;
    public CameraController(IDataProvider provider) 
    {
        _dataProvider = provider;
    }

    [HttpGet]
    public IEnumerable<CameraData> GetCameras() 
    {
        return _dataProvider.GetCameras();
    }
}