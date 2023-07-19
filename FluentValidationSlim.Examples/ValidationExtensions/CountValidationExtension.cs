using FluentValidationSlim.Infra;
using System.Collections;

namespace FluentValidationSlim.Examples.ValidationExtensions
{
    internal static class CountValidationExtension
    {

        public static ValidationResultSlim MaxCount<TList>(
            this ValidationContextSlim<TList> context, int minCount, int maxCount)
            where TList : IEnumerable
        {
            var result = new ValidationResultSlim(context);
            context.MaxCount(result, minCount, maxCount);
            return result;
        }

        public static bool MaxCount<TList>(
            this ValidationContextSlim<TList> context,
            ValidationResultSlim result,
            int minCount,
            int maxCount)
            where TList : IEnumerable
        {
            var x = context.PropertyValue;
            var count = x?.Cast<object>().Count() ?? 0;
            if (!(minCount <= count && count <= maxCount))
            {
                result.Error(context, ValidationMessages.CountItemsLimit2, minCount, maxCount);
                return false;
            }

            return true;
        }
    }
}
