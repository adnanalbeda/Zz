namespace WebApplication.Core;

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Zz;
using Zz.App;
using Zz.App.Core;

public class ValidationPipeline<TRequest, TResult>
    : AppProcessHandler<TRequest>,
        IAppRequestPipelineBehavior<TRequest, TResult>
    where TRequest : IAppRequest<TResult>, IMayValidate
{
    private IEnumerable<IValidator<TRequest>>? validators;
    protected IEnumerable<IValidator<TRequest>> Validators =>
        validators ??= GetRequiredService<IEnumerable<IValidator<TRequest>>>();

    public ValidationPipeline(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        UseLogger<ValidationPipeline<TRequest, TResult>>();
    }

    public async Task<IRequestResult<TResult>> Handle(
        TRequest request,
        RequestHandlerDelegate<IRequestResult<TResult>> next,
        CancellationToken cancellationToken
    )
    {
        this.AppProcessLogger.LogInformation("VALIDATION_PIPELINE :: Processing...");

        if (!Validators.Any())
        {
            this.AppProcessLogger.LogInformation("VALIDATION_PIPELINE :: NONE -> Next...");

            return await next();
        }

        var errors = new List<ValidationFailure>();
        try
        {
#if DEBUG
            this.AppProcessLogger.LogTrace(
                "VALIDATING :: {REQUEST}",
                JsonSerializer.Serialize(request)
            );
#endif

            foreach (var validator in Validators)
            {
                var err = await validator.ValidateAsync(request);
                if (err.Errors is null)
                    continue;
                errors.AddRange(err.Errors);
            }
        }
        catch (Exception ex)
        {
            this.AppProcessLogger.LogError(
                "VALIDATION_PIPELINE :: FAULTED_INTERNALLY :: {EX_MESSAGE}",
                ex.Message
            );
        }
        if (errors.Any())
        {
            var res = await Result.InvalidRequestAsync(
                "INVALID_ITEM",
                errors.GroupBy(
                    x => x.PropertyName,
                    x => (x.ErrorMessage, x.ErrorCode),
                    (propertyName, errors) =>
                        new InvalidRequestResultContent.Error(
                            propertyName,
                            errors.Select(x => new InvalidRequestResultContent.Error.ErrorDetail(
                                x.ErrorMessage,
                                x.ErrorCode
                            ))
                        )
                )
            );

#if DEBUG
            this.AppProcessLogger.LogTrace(
                "VALIDATION FAILED :: ERRORS : {ERRORS}",
                JsonSerializer.Serialize(res.Content.Data.Errors)
            );
#endif

            this.AppProcessLogger.LogWarning("VALIDATION_PIPELINE :: INVALID :: THE_END");
            return await res.ToReqResAsync<TResult>();
        }

        this.AppProcessLogger.LogInformation("VALIDATION_PIPELINE :: VALID -> Next...");
        return await next();
    }
}
