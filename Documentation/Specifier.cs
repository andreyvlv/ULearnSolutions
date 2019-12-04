using System;
using System.Linq;
using System.Reflection;

namespace Documentation
{
    public class Specifier<T> : ISpecifier
    {
        public string GetApiDescription()
        {                     
            var descAttr = typeof(T).GetCustomAttributes<ApiDescriptionAttribute>().FirstOrDefault();
            return descAttr.Description;
        }

        public string[] GetApiMethodNames()
        {
            var methodsInfo = typeof(T).GetMethods();            
            return methodsInfo
                .Where(mi => mi.GetCustomAttributes<ApiMethodAttribute>().Any())
                .Select(x => x.Name)
                .ToArray();
        }

        public string GetApiMethodDescription(string methodName)
        {
            var method = GetMethodInfo(typeof(T), methodName);
            return method?.GetCustomAttributes(false)
                .OfType<ApiDescriptionAttribute>()
                .FirstOrDefault().Description;       
        }

        public string[] GetApiMethodParamNames(string methodName)
        {
            var method = GetMethodInfo(typeof(T), methodName);
            var parametersInfo = method.GetParameters();
            return parametersInfo                
                .Select(x => x.Name)
                .ToArray();
        }

        public string GetApiMethodParamDescription(string methodName, string paramName)
        {           
            var method = GetMethodInfo(typeof(T), methodName);           
            var param = GetParameterInfo(method, paramName);
            return param.GetCustomAttributes(false).OfType<ApiDescriptionAttribute>().FirstOrDefault()?.Description;
        }

        public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
        {            
            var method = GetMethodInfo(typeof(T), methodName);
            var param = GetParameterInfo(method, paramName);
            var attrs = param?.GetCustomAttributes(false);
            return new ApiParamDescription
            {
                MinValue = attrs?.OfType<ApiIntValidationAttribute>()
                    .FirstOrDefault()?.MinValue,
                MaxValue = attrs?.OfType<ApiIntValidationAttribute>()
                    .FirstOrDefault()?.MaxValue,
                ParamDescription = new CommonDescription(paramName, attrs?.OfType<ApiDescriptionAttribute>()
                    .FirstOrDefault()?.Description),
                Required = attrs?.OfType<ApiRequiredAttribute>().
                    FirstOrDefault() is null ? false 
                    : attrs.OfType<ApiRequiredAttribute>().FirstOrDefault().Required
            };            
        }

        public ApiMethodDescription GetFullApiMethodDescription(string methodName)
        {
            var mi = GetMethodInfo(typeof(T), methodName);
            var isApiMethod = mi.GetCustomAttributes<ApiMethodAttribute>().Any();

            var desc = GetApiMethodDescription(methodName);
            var paramNames = GetApiMethodParamNames(methodName);
            var paramApiDesc = paramNames.Select(x => GetApiMethodParamFullDescription(methodName, x)).ToArray();
                                
            var apiMD = new ApiMethodDescription
            {
                MethodDescription = new CommonDescription(methodName, desc),
                ParamDescriptions = paramApiDesc,
                ReturnDescription = GetReturnTypeDescription(mi)
            };

            return isApiMethod ? apiMD : null;
        }

        ApiParamDescription GetReturnTypeDescription(MethodInfo mi)
        {
            var retAttr = mi.ReturnTypeCustomAttributes.GetCustomAttributes(false);
            var reqAttr = retAttr?.OfType<ApiRequiredAttribute>().FirstOrDefault();
            var intValAttr = retAttr?.OfType<ApiIntValidationAttribute>().FirstOrDefault();

            return retAttr.Length == 0 ? null : new ApiParamDescription
            {
                Required = reqAttr is null ? false : reqAttr.Required,
                MaxValue = intValAttr?.MaxValue,
                MinValue = intValAttr?.MinValue,
                ParamDescription = new CommonDescription()
            };
        }

        MethodInfo GetMethodInfo(Type type, string methodName)
        {
            var methodsInfo = type.GetMethods();
            return methodsInfo.Where(mi => mi.Name == methodName).FirstOrDefault();
        }

        ParameterInfo GetParameterInfo(MethodInfo method, string paramName)
        {
            var parametersInfo = method.GetParameters();
            return parametersInfo.Where(pi => pi.Name == paramName).FirstOrDefault();
        }
    }
}