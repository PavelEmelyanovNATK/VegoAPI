namespace VegoAPI.Utils
{
    public static class ArrayWrapperExtension
    {
        public static T[] WrapToArray<T>(this T self)
            => new T[] { self };
    }
}
