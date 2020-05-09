using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Entry = Microcharts.Entry;
using LineChart = Microcharts.LineChart;

namespace TestBang.GenericClass
{
    class MultiLineChart : LineChart
    {
        public List<List<Entry>> multiline_entries { get; set; } = null;

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            foreach (List<Entry> list in multiline_entries)
            {
                Entries = list;
                DrawSubplot(canvas, width, height);
            }
        }


        protected void DrawSubplot(SKCanvas canvas, int width, int height)
        {
            var valueLabelSizes = MeasureValueLabels();
            var footerHeight = CalculateFooterHeight(valueLabelSizes);
            var headerHeight = CalculateHeaderHeight(valueLabelSizes);
            var itemSize = CalculateItemSize(width, height, footerHeight, headerHeight);
            var origin = CalculateYOrigin(itemSize.Height, headerHeight);
            var points = this.CalculatePoints(itemSize, origin, headerHeight);

            this.DrawArea(canvas, points, itemSize, origin);
            this.DrawLine(canvas, points, itemSize);
            this.DrawPoints(canvas, points);
            this.DrawFooter(canvas, points, itemSize, height, footerHeight);
            this.DrawValueLabel(canvas, points, itemSize, height, valueLabelSizes);
        }
    }
}