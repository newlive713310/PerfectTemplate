using PerfectTemplate.Application.Interfaces;

namespace PerfectTemplate.Application.Services
{
    public class ComparerService<T> : IDynamicComparer<T>
    {
        public int Compare(object x, object y, string propertyName, bool ascending)
        {
            // Приводим объекты x и y к типу T
            T typedX = (T)x;
            T typedY = (T)y;

            // Получаем значения свойств с использованием рефлексии
            var propertyX = typedX.GetType().GetProperty(propertyName).GetValue(typedX);
            var propertyY = typedY.GetType().GetProperty(propertyName).GetValue(typedY);

            // Сравниваем значения свойств
            int result = Comparer<object>.Default.Compare(propertyX, propertyY);

            // Учитываем порядок сортировки
            if (!ascending)
                result = -result;

            return result;
        }
    }
}
