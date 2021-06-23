﻿using gView.Cmd.RenderTileCache.Extensions;
using gView.Framework.Geometry;
using gView.Framework.Geometry.Tiling;
using gView.Framework.Metadata;
using gView.Framework.system;
using System;
using System.Linq;

namespace gView.Cmd.RenderTileCache
{
    class Program
    {
        public enum Action
        {
            None = 0,
            Info = 1,
            Render = 2
        };

        static int Main(string[] args)
        {
            try
            {
                PlugInManager.InitSilent = true;

                Action action = Action.None;
                string server = String.Empty, service = String.Empty, cacheFormat = "normal";
                int epsg = 0, maxParallelRequests = 1;
                GridOrientation orientation = GridOrientation.UpperLeft;
                IEnvelope bbox = null;

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "-server")
                    {
                        server = args[++i];
                    }
                    if (args[i] == "-service")
                    {
                        service = args[++i];
                    }
                    if (args[i] == "-info")
                    {
                        action = Action.Info;
                    }
                    if (args[i] == "-render")
                    {
                        action = Action.Render;
                    }
                    if(args[i] == "-compact")
                    {
                        cacheFormat = "compact";
                    }
                    if(args[i] == "-epsg")
                    {
                        epsg = int.Parse(args[++i]);
                    }
                    if(args[i] == "-orientation")
                    {
                        switch(args[++i].ToLower())
                        {
                            case "ul":
                            case "upperleft":
                                orientation = GridOrientation.UpperLeft;
                                break;
                            case "ll":
                            case "lowerleft":
                                orientation = GridOrientation.LowerLeft;
                                break;
                        }
                    }
                    if(args[i] == "-bbox")
                    {
                        bbox = Envelope.FromBBox(args[++i]);
                    }
                    if(args[i] == "-threads")
                    {
                        maxParallelRequests = int.Parse(args[++i]);
                    }
                }

                if (action == Action.None ||
                    String.IsNullOrWhiteSpace(server) ||
                    String.IsNullOrWhiteSpace(service))
                {
                    Console.WriteLine("USAGE:");
                    Console.WriteLine("gView.Cmd.RenderTileCache <-info|-render> -server <server> -service <service>");
                    Console.WriteLine("       optional paramters: -epsg <epsg-code>                            [default: first]");
                    Console.WriteLine("                           -compact ... create a compact tile cache");
                    Console.WriteLine("                           -orientation <ul|ll|upperleft|upperright>    [default: upperleft]");
                    Console.WriteLine("                           -bbox <minx,miny,maxx,maxy>                  [default: fullextent]");
                    Console.WriteLine("                           -threads <max-parallel-requests>             [default: 1]");

                    return 1;
                }

                #region Read Metadata

                var metadata = new TileServiceMetadata().FromService(server, service);
                if (metadata == null)
                {
                    throw new Exception("Can't read metadata from server. Are you sure taht ervice is a gView WMTS service?");
                }

                #endregion

                if (action == Action.Info)
                {
                    #region TileSize

                    Console.WriteLine($"TileSize [Pixel]: { metadata.TileWidth } x { metadata.TileHeight }");

                    #endregion

                    #region ImageFormat

                    Console.Write("ImageFormats:");
                    Console.Write(metadata.FormatJpg ? " jpg" : "");
                    Console.Write(metadata.FormatPng ? " png" : "");
                    Console.WriteLine();

                    #endregion

                    #region Scales

                    Console.WriteLine("Scales:");
                    if (metadata.Scales != null)
                    {
                        foreach (var scale in metadata.Scales)
                        {
                            Console.WriteLine($"  1 : { scale }");
                        }
                    }

                    #endregion

                    #region Origin

                    Console.Write("Origin:");
                    Console.Write(metadata.UpperLeft ? " upperleft" : "");
                    Console.Write(metadata.LowerLeft ? " lowerleft" : "");
                    Console.WriteLine();

                    if (metadata.EPSGCodes != null)
                    {
                        foreach (var epsgCode in metadata.EPSGCodes)
                        {
                            if (metadata.UpperLeft)
                            {
                                var ul = metadata.GetOriginUpperLeft(epsgCode);
                                Console.WriteLine($"  EPSG:{ epsgCode } upperleft: { ul.X }, { ul.Y }");
                            }
                            if (metadata.LowerLeft)
                            {
                                var ll = metadata.GetOriginUpperLeft(epsgCode);
                                Console.WriteLine($"  EPSG:{ epsgCode } lowerleft: { ll.X }, { ll.Y }");
                            }
                        }
                    }

                    #endregion

                    #region Extents

                    Console.WriteLine("BBox:");
                    if (metadata.EPSGCodes != null)
                    {
                        foreach (var epsgCode in metadata.EPSGCodes)
                        {
                            var envelope = metadata.GetEPSGEnvelope(epsgCode);
                            if (envelope != null)
                            {
                                Console.WriteLine($"  EPSG:{ epsgCode }: { envelope.minx }, { envelope.miny }, { envelope.maxx }, { envelope.maxy }");
                            }
                        }
                    }

                    #endregion
                }
                else if (action == Action.Render)
                {
                    var startTime = DateTime.Now;

                    var tileRender = new TileRenderer(metadata,
                                                      epsg > 0 ? epsg : metadata.EPSGCodes.First(),
                                                      cacheFormat: cacheFormat,
                                                      orientation: orientation,
                                                      bbox: bbox,
                                                      maxParallelRequests: maxParallelRequests);

                    tileRender.Renderer(server, service);

                    Console.WriteLine();
                    Console.WriteLine($"Finished: { Math.Round((DateTime.Now - startTime).TotalSeconds) }sec");
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:");
                Console.WriteLine(ex.Message);

                return 1;
            }
        }
    }
}