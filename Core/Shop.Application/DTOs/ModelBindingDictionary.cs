using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace Shop.Application.DTOs
{
    /// <summary>
    /// A custom model binder that binds JSON-formatted dictionary data submitted from forms.
    /// </summary>
    /// <typeparam name="Tkey">The type of the dictionary key.</typeparam>
    /// <typeparam name="TValue">The type of the dictionary value.</typeparam>
    public class ModelBindingDictionary<Tkey, TValue> : IModelBinder
    {
        /// <summary>
        /// Converts a string value to the specified type.
        /// </summary>
        /// <typeparam name="T">The target type to convert to.</typeparam>
        /// <param name="value">The string value to convert.</param>
        /// <returns>The converted value of the specified type.</returns>
        public T GetValue<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Binds the model by deserializing a JSON string into a dictionary object.
        /// </summary>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>A completed task representing the binding operation.</returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            try
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var dictionary = JsonSerializer.Deserialize<Dictionary<Tkey, TValue>>(value, options);

              

                bindingContext.Result = ModelBindingResult.Success(dictionary);
            }
            catch (Exception)
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }
    }
}
