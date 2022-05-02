using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Класс создания панели просмотра структуры каталога
    /// </summary>
    public class FolderViewFactory : UIFactory<UIFolderView>
    {
        public Coordinates Coordinates { get; set; }
        public Dimensions Dimensions { get; set; }
        public int LineWidth { get; set; }

        public FolderViewFactory(Coordinates coordinates, Dimensions dimensions, int lineWidth)
        {
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
            Dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
            LineWidth = lineWidth;
        }

        public override UIFolderView CreateView()
        {
            Coordinates position = Coordinates == null ? new Coordinates() : Coordinates;
            Dimensions size = Dimensions == null ? new Dimensions() : Dimensions;

            // *** Border **

            UIBoxStyle borderStyle = new UIBoxStyle();
            Dimensions borderSize = size;
            Coordinates borderPosition = position;
            UIBox border = new UIBox(borderPosition, borderSize, LineWidth, borderStyle);

            // *** Lines ***

            UILineStyle lineStyle = new UILineStyle();
            Dimensions lineDimensions = new Dimensions(size.Width, LineWidth);

            // Header separator
            Coordinates lineCoordinates = new Coordinates(position.Left, position.Top + 4);
            UILine headerDivider = new UILine(lineCoordinates, lineDimensions, lineStyle);

            // Footer separator
            lineCoordinates = new Coordinates(position.Left, position.Top + Dimensions.Height - 5);
            UILine footerDivider = new UILine(lineCoordinates, lineDimensions, lineStyle);

            // Folder lines
            lineDimensions = new Dimensions(LineWidth, footerDivider.Position.Top - headerDivider.Position.Top + 1);

            // Info Attr Divider
            lineCoordinates = new Coordinates(position.Left + size.Width - 12, headerDivider.Position.Top);
            UILine infoAttrDivider = new UILine(lineCoordinates, lineDimensions, lineStyle);

            // Info Type Divider
            lineCoordinates = new Coordinates(position.Left + size.Width - 24, headerDivider.Position.Top);
            UILine infoTypeDivider = new UILine(lineCoordinates, lineDimensions, lineStyle);

            // _info Size Divider
            lineCoordinates = new Coordinates(position.Left + size.Width - 36, headerDivider.Position.Top);
            UILine infoSizeDivider = new UILine(lineCoordinates, lineDimensions, lineStyle);

            // Crate Folder View
            return new UIFolderView(border, headerDivider, footerDivider, infoSizeDivider, infoTypeDivider, infoAttrDivider);
        }
    }
}
