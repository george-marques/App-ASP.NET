public static class MvcConfig
{

    public static IServiceCollection AddMvcConfiguration(this IServiceCollection services)
    {
        services.AddControllersWithViews(options =>
        {
            options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "O valor fornecido é inválido.");
            options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => "O campo é obrigatório.");
            options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "O campo é obrigatório.");
            options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "É necessário fornecer um corpo na solicitação HTTP.");
            options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => "O valor fornecido é inválido.");
            options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "O valor fornecido é inválido.");
            options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "O campo deve ser um número.");
            options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => "O valor fornecido é inválido.");
            options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => "O valor fornecido é inválido para " + x);
            options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => "O campo " + x + " deve ser um número.");
            options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "O campo " + x + " não pode ser nulo.");
        });
        return services;
    }
}
