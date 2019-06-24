/*
Thomas Sanchez Lengeling.
http://codigogenerativo.com/

KinectPV2, Kinect for Windows v2 library for processing

Color to fepth Example,
Color Frame is aligned to the depth frame
*/

import KinectPV2.*;

KinectPV2 kinect;

ScreenBroadcast sb;

 int pixalateSize = 15;

int [] depthZero;

//BUFFER ARRAY TO CLEAN DE PIXLES
PImage img;

void setup() {
  size(512, 424);
  sb = new ScreenBroadcast();
  kinect = new KinectPV2(this);
  kinect.enableDepthImg(true);
  kinect.enableColorImg(true);
  kinect.enablePointCloud(true);
  kinect.init();
  img = createImage(KinectPV2.WIDTHDepth, KinectPV2.HEIGHTDepth, PImage.RGB);
  //depthZero    = new int[ KinectPV2.WIDTHDepth * KinectPV2.HEIGHTDepth];
}

void draw() {
  background(0);
  
  img.loadPixels();
  
  int [] rawData = kinect.getRawDepthData();
  
  for (int x = 0; x < KinectPV2.WIDTHDepth-pixalateSize; x=x+pixalateSize){
    for (int y = 0; y < KinectPV2.HEIGHTDepth-pixalateSize; y=y+pixalateSize){
      
      
      int offset = x + y * KinectPV2.WIDTHDepth;
      int d = rawData[offset];
      long dsum = 0;
      
      for(int i=0;i<pixalateSize;i++)
      {
        for(int j=0;j<pixalateSize;j++)
        {
           offset = (x+i) + (y+j) * KinectPV2.WIDTHDepth;

           d = rawData[offset];
             if(d<500)
             d=1050;
           if(d>1050)
             d=1050;
           dsum += d;
        }
      }
      d = (int)(dsum/(pixalateSize*pixalateSize));
      
      //rood
      for(int i=0;i<pixalateSize;i++)
      for(int j=0;j<pixalateSize;j++)
      {
        offset = (x+i) + (y+j) * KinectPV2.WIDTHDepth;
      
      //int i=0;
      {
        img.pixels[offset] = color(0, 255, 0);
        if (d > 500 && d < 600) {
          img.pixels[offset] = color(255, 0, 0);
        } 
        //donker oranje
        if (d > 600 && d < 700) {
          img.pixels[offset] = color(255, 64, 0);   
        }
        //oranje
        if (d > 700 && d < 760) {
          img.pixels[offset] = color(255, 128, 0);   
        }
        //licht oranje
        if (d > 760 && d < 815) {
          img.pixels[offset] = color(255, 191, 0);   
        }
        //geel
        if (d > 815 && d < 880) {
          img.pixels[offset] = color(255, 255, 0);   
        }
        //lime groen
        if (d > 880 && d < 920) {
          img.pixels[offset] = color(191, 255, 0);   
        }
        // groen
        if (d > 990 && d < 1050) {
          img.pixels[offset] = color(128, 255, 0); 
        }
      }
      }
    }
  }
  
  img.updatePixels();
  image(img, 0, 0);
  scale(-1.0, 1.0);
  image(img,-img.width,0);
  //loadPixels();
  //send processing data
  sb.BroadcastSplit(get());
  delay(1000);
  //delay(1);
  
}
