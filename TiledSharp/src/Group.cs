// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Xml.Linq;

namespace TiledSharp
{
    public class TmxGroup : ITmxElement
    {
        public string Name { get; private set; }

        public double Opacity { get; private set; }
        public bool Visible { get; private set; }
        public double? OffsetX { get; private set; }
        public double? OffsetY { get; private set; }

        public TmxList<TmxLayer> Layers { get; private set; }
        public TmxList<TmxObjectGroup> ObjectGroups { get; private set; }
        public TmxList<TmxImageLayer> ImageLayers { get; private set; }
        public TmxList<TmxGroup> Groups { get; private set; }
        public PropertyDict Properties { get; private set; }

        public TmxGroup(XElement xGroup, int width, int height, string tmxDirectory)
        {
            Name = (string)xGroup.Attribute("name") ?? String.Empty;
            Opacity = (double?)xGroup.Attribute("opacity") ?? 1.0;
            Visible = (bool?)xGroup.Attribute("visible") ?? true;
            OffsetX = (double?)xGroup.Attribute("offsetx") ?? 0.0;
            OffsetY = (double?)xGroup.Attribute("offsety") ?? 0.0;

            Properties = new PropertyDict(xGroup.Element("properties"));

            Layers = new TmxList<TmxLayer>();
            ObjectGroups = new TmxList<TmxObjectGroup>();
            ImageLayers = new TmxList<TmxImageLayer>();
            Groups = new TmxList<TmxGroup>();

            int elementIndex = 0;

            foreach (var e in xGroup.Elements())
            {
                switch (e.Name.ToString())
                {
                    case "layer":
                        Layers.Add(new TmxLayer(e, elementIndex, width, height));
                        break;
                    case "objectgroup":
                        ObjectGroups.Add(new TmxObjectGroup(e, elementIndex));
                        break;
                    case "imagelayer":
                        ImageLayers.Add(new TmxImageLayer(e, elementIndex, tmxDirectory));
                        break;
                    case "group":
                        Groups.Add(new TmxGroup(e, width, height, tmxDirectory));
                        break;
                }

                elementIndex++;
            }
        }
    }
}
