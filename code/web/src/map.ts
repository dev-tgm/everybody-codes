import Map from 'ol/Map.js';
import VectorLayer from 'ol/layer/Vector.js';
import VectorSource from 'ol/source/Vector.js';
import View from 'ol/View.js';
import Feature from 'ol/Feature';
import { Point } from 'ol/geom';
import TileLayer from 'ol/layer/Tile';
import OSM from 'ol/source/OSM.js';
import { transform } from 'ol/proj';
import { CameraData } from './interfaces';
import { Icon, Style, Text } from 'ol/style.js';

import imgUrl from '../video-camera-icon.png';

export class MapHandler {
    private map!: Map;
    private vectorLayer!: VectorLayer<VectorSource>;

    constructor() {
        this.initmap();
    }

    private initmap(): void {
        // Basic vector layer to add our api features to.
        this.vectorLayer = new VectorLayer({
            source: new VectorSource({ features: [], }),
        });

        // Default OpenStreetMaps layer.
        const osmlayer = new TileLayer({
            source: new OSM()
        });

        // OpenLayers boilerplate code to create a mapview.
        this.map = new Map({
            layers: [osmlayer, this.vectorLayer],
            target: 'mapid',
            view: new View({
                center: [0, 0],
                zoom: 13,
            }),
        });

        // Openlayers by default uses EPSG:3857, so we need to transform our long,lat coordinates.
        // Set the map view to the given coordinates in utrecht
        this.map.getView().setCenter(transform([5.1115, 52.0914], 'EPSG:4326', this.map.getView().getProjection()));
    }

    AddCameraToMap(camera: CameraData) {
        // Create a Point, project it to the maps projection
        const geometry = new Point([camera.longitude, camera.latitude]);
        const feature = new Feature(geometry.transform('EPSG:4326', this.map.getView().getProjection()));
        // set some data to the feature
        feature.set('number', camera.number);
        feature.set('camera', camera.camera);

        feature.setStyle(new Style(
            {
                image: new Icon({ src: imgUrl, scale: 0.05 }),
                text: new Text({ text: [camera.number.toString(), 'bold 12px sans-serif'], offsetY: 20, })
            }
        ));

        // add feature to the vector layer.
        this.vectorLayer.getSource()?.addFeature(feature);
    }

    fitCameras() {
        const extent = this.vectorLayer.getSource()?.getExtent();
        if (extent != undefined) {
            this.map.getView().fit(extent, { padding: [50, 50, 50, 50] });
        }
    }
}