using System.Collections.Generic;

namespace data 
{
    public interface IDataProvider 
    {
        public IEnumerable<CameraData> GetCameras();
    }
}