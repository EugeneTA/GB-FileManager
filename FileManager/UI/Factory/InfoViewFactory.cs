using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Класс создания информационной панели
    /// </summary>
    public class InfoViewFactory : UIFactory<UIInfoView>
    {
        private List<string> _data;
        Coordinates _coordinates;
        Dimensions _dimensions;
        int _lineWidth;

        public InfoViewFactory(Coordinates coordinates, Dimensions dimensions, int lineWidth, List<string> data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
            _dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
            _lineWidth = lineWidth;
        }

        /// <summary>
        /// Создиние информационной панели
        /// </summary>
        /// <returns></returns>
        public override UIInfoView CreateView()
        {
            Coordinates position = _coordinates == null ? new Coordinates() : _coordinates;
            Dimensions size = _dimensions == null ? new Dimensions() : _dimensions;

            // Задаем стиль, размер и координаты рамки диалогового окна
            UIBoxStyle borderStyle = new UIBoxStyle();
            Dimensions borderSize = size;
            Coordinates borderPosition = position;
            // Создаем рамку
            UIBox border = new UIBox(borderPosition, borderSize, _lineWidth, borderStyle);

            // Создаем диалоговое окно
            return new UIInfoView(border, _data);
        }
    }
}
