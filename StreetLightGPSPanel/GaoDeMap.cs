using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetLightPanel
{
    public class GaoDeMap : ESRI.ArcGIS.Client.TiledMapServiceLayer
    {


        private const double cornerCoordinate = 20037508.342787;
        public override void Initialize()
        {

            this.FullExtent = new
             ESRI.ArcGIS.Client.Geometry.Envelope(-20037508.342787, -20037508.342787, 20037508.342787, 20037508.342787);
            {
                SpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference(102100);
            };


            this.SpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference(102100);

            this.TileInfo = new TileInfo()
            {
                Height = 256,
                Width = 256,

                Origin = new ESRI.ArcGIS.Client.Geometry.MapPoint(-20037508.342787, 20037508.342787)
                {
                    SpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference(102100)
                },
                Lods = new Lod[19]
            };


            double resolution = 156543.033928;

            for (int i = 0; i < TileInfo.Lods.Length; i++)
            {

                TileInfo.Lods[i] = new Lod() { Resolution = resolution };
                resolution /= 2;
            }

            // Call base initialize to raise the initialization event 
            
        
            
            base.Initialize();
        }


        public void ZoomToLevel(int level, double x, double y)
           
        {
            MapPoint point = new ESRI.ArcGIS.Client.Projection.WebMercator().FromGeographic(new MapPoint(x,y)) as MapPoint;
            bool zoomentry = false;
            double resolution;
            if (level == -1)
                resolution = Map.Resolution;
            else
                resolution = (this.Map.Layers["base"] as TiledLayer).TileInfo.Lods[level].Resolution;


            if (Math.Abs(this.Map.Resolution - resolution) < 0.05)
            {
                this.Map.PanTo(point);
                return;
            }
            zoomentry = false;
            this.Map.ZoomToResolution(resolution);

            Map.ExtentChanged += (s, a) =>
            {
                if (!zoomentry)
                    this.Map.PanTo(point);

                zoomentry = true;

                //   SwitchLayerVisibility();
            };

          
        }
        public override string GetTileUrl(int level, int row, int col)
        {
            //   string baseUrl = "http://webrd0{0}.is.autonavi.com/appmaptile?x={1}&y={2}&z={3}&lang=zh_cn&size=1&scale=1&style=7"; ;
            string baseUrl = "http://webrd02.is.autonavi.com/appmaptile?x={0}&y={1}&z={2}&lang=zh_cn&size=1&scale=1&style=7"; ;

            string quard = GetQuard(col, row, level);

            return string.Format(baseUrl, col, row, level);
        }

        public static string GetQuard(int x, int y, int zoomLevel)
        {
            string str = "";
            while (x > 0 || y > 0)
            {
                str = ((x & 1) << 1 | y & 1).ToString() + str;
                x >>= 1;
                y >>= 1;
            }
            return ((object)str).ToString().PadLeft(zoomLevel, '0');
        }

        public static void LongitudeLatitude2GoogleTileXT(double longitude, double latitude, int level, out int tilex, out int tiley)
        {
            double sinLatitude = Math.Sin(latitude * Math.PI / 180);

            double pixelX = ((longitude + 180) / 360) * 256 * Math.Pow(2, level);

            double pixelY = (0.5 - Math.Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Math.PI)) * 256 * Math.Pow(2, level);

            tilex = (int)(pixelX / 256);
            tiley = (int)(pixelY / 256);

            double relx = (int)pixelX % 256;
            double rely = (int)pixelY % 256; ;


        }

    }
}
