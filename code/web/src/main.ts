import './style.css';
import { MapHandler } from './map.ts';
import { CameraData } from './interfaces.ts';
import { AddCameraRowToTable, GetTableidForCameranumber } from './tablehelper.ts';

const map = new MapHandler();

// Load data from API.
fetch('/api/camera').then(async (result) => {
  const data = await result.json() as CameraData[];
  console.log(data);

  // Loop through camera's
  for (const camera of data) {
    map.AddCameraToMap(camera);

    const table = GetTableidForCameranumber(camera.number);
    AddCameraRowToTable(table, camera);
  }
})
