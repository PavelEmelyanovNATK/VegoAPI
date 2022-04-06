using System;

namespace VegoAPI.Utils
{
    public static class LetExtension
    {
        /// <summary>
        /// Формирует временную область видимости для объекта, 
        /// к которому была применена, и вызывает код, 
        /// указанный в переданной функции.
        /// </summary>
        /// <typeparam name="T">Объект.</typeparam>
        /// <typeparam name="R">Новое возвращаемое значение.</typeparam>
        /// <param name="self"></param>
        /// <param name="converter">Функция преобразоания.</param>
        /// <returns>Преобразованное значение.</returns>
        public static R Let<T, R>(this T self, Func<T, R> converter)
            => converter(self);

        /// <summary>
        /// Формирует временную область видимости для объекта, 
        /// к которому была применена, и вызывает код, 
        /// указанный в переданной функции.
        /// </summary>
        /// <typeparam name="T">Объект.</typeparam>
        /// <param name="self"></param>
        /// <param name="func">Выполняемая функция.</param>
        public static void Let<T>(this T self, Action<T> func)
            => func(self);
    }
}
